using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Entities;
using DisSagligiTakip.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.Controllers
{
    // Hastaları görebilen herkes (Admin, SuperAdmin, Doctor, Assistant)
    [Authorize(Roles = "Admin,SuperAdmin,Doctor,Assistant")]
    public class HastaController : Controller
    {
        private readonly AppDbContext _context;

        public HastaController(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 Arama destekli liste
        public IActionResult Index(string? q)
        {
            var query = _context.Users
                .Where(u => u.Role == Helpers.UserRoles.Patient);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(u => u.FullName.Contains(q) || u.Email.Contains(q));
            }

            var hastalar = query
                .OrderBy(u => u.FullName)
                .AsNoTracking()
                .ToList();

            ViewBag.Q = q;
            return View(hastalar);
        }

        // 🟢 Hasta detay + kayıtlar (düzenleme bağlantıları açık)
        public IActionResult Detay(int id)
        {
            var hasta = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id && u.Role == Helpers.UserRoles.Patient);
            if (hasta == null) return NotFound();

            var kayitlar = _context.DisSagligiVerileri
                .Where(v => v.HastaUserId == id || v.UserId == id)
                .OrderByDescending(v => v.Tarih)
                .AsNoTracking()
                .ToList();

            var vm = new HastaDetayViewModel
            {
                Hasta = hasta,
                DisKayitlari = kayitlar
            };

            return View(vm);
        }
    }
}
