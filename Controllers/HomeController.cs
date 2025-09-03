using System.Diagnostics;
using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Entities;
using DisSagligiTakip.Helpers;
using DisSagligiTakip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Ana Sayfa
        public IActionResult Index()
        {
            return View();
        }

        // ✅ Gizlilik Sayfası
        public IActionResult Privacy()
        {
            return View();
        }

        // ✅ Hata Sayfası (anonim erişime açık)
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ✅ Admin Panel – Sadece Admin ve SuperAdmin kullanıcılar erişebilir
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult AdminPanel()
        {
            var users = _context.Users
                .AsNoTracking()
                .ToList();

            return View("AdminPanel", users);
        }

        // ✅ Kullanıcı Rolünü Düzenle (GET)
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public IActionResult EditRole(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("AdminPanel");
            }

            if (user.Role == UserRoles.SuperAdmin)
            {
                TempData["Error"] = "SuperAdmin rolüne sahip bir kullanıcının rolü değiştirilemez.";
                return RedirectToAction("AdminPanel");
            }

            ViewBag.Roles = Enum.GetNames(typeof(UserRoles)).ToList();
            return View(user);
        }

        // ✅ Kullanıcı Rolünü Düzenle (POST)
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public IActionResult EditRole(int id, string role)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("AdminPanel");
            }

            if (user.Role == UserRoles.SuperAdmin)
            {
                TempData["Error"] = "SuperAdmin rolüne sahip bir kullanıcının rolü değiştirilemez.";
                return RedirectToAction("AdminPanel");
            }

            if (!Enum.TryParse<UserRoles>(role, out var newRole))
            {
                TempData["Error"] = "Geçersiz rol seçimi.";
                return RedirectToAction("EditRole", new { id });
            }

            user.Role = newRole;
            _context.SaveChanges();

            TempData["Success"] = "Kullanıcının rolü başarıyla güncellendi.";
            return RedirectToAction("AdminPanel");
        }
    }
}
