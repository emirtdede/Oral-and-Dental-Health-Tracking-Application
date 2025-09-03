using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Entities;
using DisSagligiTakip.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisSagligiTakip.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Profile/Edit
        [HttpGet]
        public IActionResult Edit()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            var vm = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,               // e-postayı ekranda sadece göstereceğiz (readonly)
                BirthDate = user.BirthDate
            };
            return View(vm);
        }

        // POST: /Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProfileViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            // Eposta'yı burada değiştirmiyoruz (yetki/akış gerektirir). İstenirse ek adımlarla yapılır.
            user.FullName = vm.FullName.Trim();
            user.BirthDate = vm.BirthDate;

            _context.SaveChanges();

            // Navbar’daki isim için session ve claim’i güncelle
            HttpContext.Session.SetString("FullName", user.FullName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync("MyCookieAuth", principal); // await gerekmez, fire-and-forget

            TempData["Success"] = "Profil bilgileriniz güncellendi.";
            return RedirectToAction(nameof(Edit));
        }
    }
}
