using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Entities;
using DisSagligiTakip.Helpers;
using DisSagligiTakip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.Controllers
{
    [Authorize]
    public class DisSagligiController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string _uploadRoot;

        public DisSagligiController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _uploadRoot = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(_uploadRoot))
                Directory.CreateDirectory(_uploadRoot);
        }

        private int? CurrentUserId => HttpContext.Session.GetInt32("UserId");
        private string? CurrentRole => User.FindFirstValue(ClaimTypes.Role);
        private bool IsPatient => string.Equals(CurrentRole, nameof(UserRoles.Patient), StringComparison.OrdinalIgnoreCase);

        // -------------------- LIST --------------------
        public IActionResult Index()
        {
            var uid = CurrentUserId;
            if (uid == null) return RedirectToAction("Login", "Auth");

            var list = _context.DisSagligiVerileri
                .AsNoTracking()
                .Where(v =>
                    // hasta kendi kayıtlarını görür (hasta kendisi oluşturmuşsa veya onun adına oluşturulmuşsa)
                    (IsPatient && (v.UserId == uid || v.HastaUserId == uid))
                    // diğer roller kendi oluşturduklarını görür
                    || (!IsPatient && v.UserId == uid))
                .OrderByDescending(v => v.Tarih)
                .ToList();

            return View(list);
        }

        // -------------------- CREATE (GET) --------------------
        public IActionResult Create()
        {
            var vm = new DisSagligiVerisiCreateVM();
            FillDropdowns(vm);
            return View(vm);
        }

        // -------------------- CREATE (POST) --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DisSagligiVerisiCreateVM vm)
        {
            var uid = CurrentUserId;
            if (uid == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                FillDropdowns(vm);
                return View(vm);
            }

            // Hasta ise hedef hasta = kendisi; değilse ekrandan gelen değer (boş da olabilir)
            var hedefHastaId = IsPatient ? uid : vm.HastaUserId;

            string? imagePath = null;
            if (vm.Gorsel != null && vm.Gorsel.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.Gorsel.FileName)}";
                var fullPath = Path.Combine(_uploadRoot, fileName);
                using var fs = new FileStream(fullPath, FileMode.Create);
                vm.Gorsel.CopyTo(fs);
                imagePath = $"/uploads/{fileName}";
            }

            var entity = new DisSagligiVerisi
            {
                UserId = uid.Value,
                HastaUserId = hedefHastaId,
                DisHekimiAdi = vm.DisHekimiAdi,
                Aciklama = vm.Aciklama,
                GorselYolu = imagePath,
                Tarih = DateTime.Now
            };

            _context.DisSagligiVerileri.Add(entity);
            _context.SaveChanges();

            TempData["Success"] = "Kayıt başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // -------------------- EDIT (GET) --------------------
        public IActionResult Edit(int id)
        {
            var uid = CurrentUserId;
            if (uid == null) return RedirectToAction("Login", "Auth");

            var e = _context.DisSagligiVerileri.FirstOrDefault(v => v.Id == id);
            if (e == null) return NotFound();

            // Güvenlik: Kendi oluşturmadığı kaydı düzenleyemesin (opsiyonel politika)
            if (e.UserId != uid && IsPatient) return Forbid();

            var vm = new DisSagligiVerisiCreateVM
            {
                Id = e.Id,
                HastaUserId = e.HastaUserId,
                DisHekimiAdi = e.DisHekimiAdi,
                Aciklama = e.Aciklama,
                MevcutGorselYolu = e.GorselYolu
            };

            FillDropdowns(vm);
            return View(vm);
        }

        // -------------------- EDIT (POST) --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DisSagligiVerisiCreateVM vm)
        {
            var uid = CurrentUserId;
            if (uid == null) return RedirectToAction("Login", "Auth");

            var e = _context.DisSagligiVerileri.FirstOrDefault(v => v.Id == vm.Id);
            if (e == null) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.MevcutGorselYolu = e.GorselYolu;
                FillDropdowns(vm);
                return View(vm);
            }

            e.Aciklama = vm.Aciklama;
            e.DisHekimiAdi = vm.DisHekimiAdi;
            e.HastaUserId = IsPatient ? uid : vm.HastaUserId;

            if (vm.Gorsel != null && vm.Gorsel.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.Gorsel.FileName)}";
                var fullPath = Path.Combine(_uploadRoot, fileName);
                using var fs = new FileStream(fullPath, FileMode.Create);
                vm.Gorsel.CopyTo(fs);
                e.GorselYolu = $"/uploads/{fileName}";
            }

            _context.SaveChanges();

            TempData["Success"] = "Kayıt güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // -------------------- DELETE --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var e = _context.DisSagligiVerileri.FirstOrDefault(v => v.Id == id);
            if (e == null) return NotFound();

            _context.DisSagligiVerileri.Remove(e);
            _context.SaveChanges();
            TempData["Success"] = "Kayıt silindi.";
            return RedirectToAction(nameof(Index));
        }

        // -------------------- Helpers --------------------
        private void FillDropdowns(DisSagligiVerisiCreateVM vm)
        {
            // Hastalar (sadece Patient rolündekiler)
            vm.Hastalar = _context.Users
                .AsNoTracking()
                .Where(u => u.Role == UserRoles.Patient)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FullName })
                .ToList();

            // Doktor isimleri: rolü Doctor olan kullanıcı adları
            vm.Doktorlar = _context.Users
                .AsNoTracking()
                .Where(u => u.Role == UserRoles.Doctor)
                .Select(u => new SelectListItem { Value = u.FullName, Text = u.FullName })
                .ToList();
        }
    }
}
