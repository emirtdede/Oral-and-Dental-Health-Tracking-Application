using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Entities;
using DisSagligiTakip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.Controllers
{
    [Authorize]
    public class FircalamaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FircalamaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Listeleme
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var kayitlar = _context.FircalamaKayitlari
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.Tarih)
                .ToList();

            return View(kayitlar);
        }

        // ✅ Oluştur (GET)
        public IActionResult Create() => View();

        // ✅ Oluştur (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FircalamaViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid) return View(model);

            string? imagePath = null;
            if (model.Gorsel != null && model.Gorsel.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(model.Gorsel.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Gorsel.CopyToAsync(stream);
                }

                imagePath = "/uploads/" + uniqueFileName;
            }

            var kayit = new FircalamaKaydi
            {
                UserId = userId.Value,
                Tarih = model.Tarih,
                Aciklama = model.Aciklama,
                GorselYolu = imagePath
            };

            _context.FircalamaKayitlari.Add(kayit);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Kayıt başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        // ✅ Düzenle (GET)
        public IActionResult Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var kayit = _context.FircalamaKayitlari.FirstOrDefault(f => f.Id == id && f.UserId == userId);
            if (kayit == null) return NotFound();

            var model = new FircalamaViewModel
            {
                Id = kayit.Id,
                Tarih = kayit.Tarih,
                Aciklama = kayit.Aciklama,
                MevcutGorselYolu = kayit.GorselYolu
            };

            return View(model);
        }

        // ✅ Düzenle (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FircalamaViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || model.Id == null) return NotFound();

            var kayit = _context.FircalamaKayitlari.FirstOrDefault(f => f.Id == model.Id && f.UserId == userId);
            if (kayit == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            // Görsel güncellendi mi?
            if (model.Gorsel != null && model.Gorsel.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(model.Gorsel.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Gorsel.CopyToAsync(stream);
                }

                kayit.GorselYolu = "/uploads/" + uniqueFileName;
            }

            kayit.Tarih = model.Tarih;
            kayit.Aciklama = model.Aciklama;

            _context.FircalamaKayitlari.Update(kayit);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Kayıt başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // ✅ Silme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var kayit = _context.FircalamaKayitlari.FirstOrDefault(f => f.Id == id && f.UserId == userId);
            if (kayit == null) return NotFound();

            _context.FircalamaKayitlari.Remove(kayit);
            _context.SaveChanges();

            TempData["Success"] = "Kayıt silindi.";
            return RedirectToAction("Index");
        }

        // ✅ Detaylar (opsiyonel)
        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var kayit = _context.FircalamaKayitlari.FirstOrDefault(f => f.Id == id && f.UserId == userId);
            if (kayit == null) return NotFound();

            return View(kayit);
        }
    }
}
