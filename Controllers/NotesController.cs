using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Entities;
using DisSagligiTakip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public NotesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Not Listeleme
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var notes = _context.Notes
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Tarih)
                .ToList();

            return View(notes);
        }

        // ✅ Not Oluşturma (GET)
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Not Oluşturma (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoteViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid) return View(model);

            string? gorselYolu = null;
            if (model.Gorsel != null && model.Gorsel.Length > 0)
            {
                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

                var dosyaAdı = $"{Guid.NewGuid()}_{model.Gorsel.FileName}";
                var fullPath = Path.Combine(uploadsPath, dosyaAdı);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.Gorsel.CopyToAsync(stream);
                }

                gorselYolu = "/uploads/" + dosyaAdı;
            }

            var note = new Note
            {
                UserId = userId.Value,
                Baslik = model.Baslik,
                Icerik = model.Icerik,
                Tarih = DateTime.Now,
                GorselYolu = gorselYolu
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Not başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }

        // ✅ Not Düzenleme (GET)
        public IActionResult Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return NotFound();

            var viewModel = new NoteViewModel
            {
                Id = note.Id,
                Baslik = note.Baslik,
                Icerik = note.Icerik,
                MevcutGorselYolu = note.GorselYolu
            };

            return View(viewModel);
        }

        // ✅ Not Düzenleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NoteViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!ModelState.IsValid) return View(model);

            var note = _context.Notes.FirstOrDefault(n => n.Id == model.Id && n.UserId == userId);
            if (note == null) return NotFound();

            note.Baslik = model.Baslik;
            note.Icerik = model.Icerik;

            if (model.Gorsel != null && model.Gorsel.Length > 0)
            {
                // Eski görseli sil
                if (!string.IsNullOrEmpty(note.GorselYolu))
                {
                    var eskiYol = Path.Combine(_env.WebRootPath, note.GorselYolu.TrimStart('/'));
                    if (System.IO.File.Exists(eskiYol))
                        System.IO.File.Delete(eskiYol);
                }

                var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
                var dosyaAdı = $"{Guid.NewGuid()}_{model.Gorsel.FileName}";
                var fullPath = Path.Combine(uploadsPath, dosyaAdı);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.Gorsel.CopyToAsync(stream);
                }

                note.GorselYolu = "/uploads/" + dosyaAdı;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Not başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // ✅ Not Detayları
        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return NotFound();

            return View(note);
        }

        // ✅ Not Silme (GET → İsteğe Bağlı)
        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return NotFound();

            return View(note);
        }

        // ✅ Not Silme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return NotFound();

            // Görseli sil
            if (!string.IsNullOrEmpty(note.GorselYolu))
            {
                var tamYol = Path.Combine(_env.WebRootPath, note.GorselYolu.TrimStart('/'));
                if (System.IO.File.Exists(tamYol))
                    System.IO.File.Delete(tamYol);
            }

            _context.Notes.Remove(note);
            _context.SaveChanges();

            TempData["Success"] = "Not başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}
