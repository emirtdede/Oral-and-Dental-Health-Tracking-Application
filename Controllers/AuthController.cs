using DisSagligiTakip.DataAccess;
using DisSagligiTakip.DTOs;
using DisSagligiTakip.Entities;
using DisSagligiTakip.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DisSagligiTakip.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogService _logService;
        private readonly EmailService _emailService;

        public AuthController(AppDbContext context, ILogService logService, EmailService emailService)
        {
            _context = context;
            _logService = logService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Bu e-posta zaten kayıtlı.");
                return View(model);
            }

            var token = Guid.NewGuid().ToString();
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = UserRoles.Patient,
                IsEmailConfirmed = false,
                EmailVerificationToken = token
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var link = Url.Action("VerifyEmail", "Auth", new { userId = user.Id, token }, Request.Scheme);
            await _emailService.SendEmailAsync(user.Email, "E-Posta Doğrulama", $"Lütfen e-posta adresinizi doğrulamak için <a href='{link}'>buraya tıklayın</a>.");

            _logService.Log(user.Email, "Kayıt başarılı, e-posta doğrulama bağlantısı gönderildi.");
            TempData["Success"] = "Kayıt başarılı! Lütfen e-posta adresinizi doğrulayın.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                TempData["Error"] = "Geçersiz giriş bilgileri.";
                return View(dto);
            }

            if (!user.IsEmailConfirmed)
            {
                // Yeni doğrulama tokenı oluştur
                var token = Guid.NewGuid().ToString();
                user.EmailVerificationToken = token;
                _context.SaveChanges();

                var link = Url.Action("VerifyEmail", "Auth", new { userId = user.Id, token }, Request.Scheme);
                await _emailService.SendEmailAsync(user.Email, "E-Posta Doğrulama", $"Lütfen e-posta adresinizi doğrulamak için <a href='{link}'>buraya tıklayın</a>.");

                TempData["Error"] = "Lütfen e-posta adresinizi doğrulayın. Yeni bir doğrulama bağlantısı gönderildi.";
                return View(dto);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("FullName", user.FullName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", principal);

            _logService.Log(user.Email, "Giriş yapıldı.");
            TempData["Success"] = "Giriş başarılı!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var email = User.Identity?.Name;
            await HttpContext.SignOutAsync("MyCookieAuth");
            HttpContext.Session.Clear();

            _logService.Log(email ?? "-", "Çıkış yapıldı.");
            TempData["Success"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            TempData["Error"] = "Bu sayfaya erişim izniniz yok.";
            return RedirectToAction("Login");
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        public IActionResult AdminPanel() => View();

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return View();
            }

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddMinutes(30);
            _context.SaveChanges();

            var link = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token = user.PasswordResetToken }, Request.Scheme);
            await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için <a href='{link}'>buraya tıklayın</a>.");

            _logService.Log(user.Email, "Şifre sıfırlama bağlantısı gönderildi.");
            TempData["Success"] = "Şifre sıfırlama bağlantısı gönderildi. Lütfen e-postanızı kontrol edin.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(int userId, string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId && u.PasswordResetToken == token && u.PasswordResetTokenExpires > DateTime.UtcNow);
            if (user == null)
            {
                TempData["Error"] = "Geçersiz veya süresi dolmuş bağlantı.";
                return RedirectToAction("Login");
            }

            return View(new ResetPasswordDTO { UserId = user.Id });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var user = _context.Users.FirstOrDefault(u => u.Id == dto.UserId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return View(dto);
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpires = null;
            _context.SaveChanges();

            _logService.Log(user.Email, "Şifre sıfırlandı.");
            TempData["Success"] = "Şifreniz başarıyla sıfırlandı.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult VerifyEmail(int userId, string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId && u.EmailVerificationToken == token);
            if (user == null)
            {
                ViewBag.IsSuccess = false;
                return View();
            }

            user.IsEmailConfirmed = true;
            user.EmailVerificationToken = null;
            _context.SaveChanges();

            _logService.Log(user.Email, "E-posta doğrulandı.");
            ViewBag.IsSuccess = true;
            return View();
        }
    }
}
