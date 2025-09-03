using System.Security.Claims;
using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Entities.Enums;
using DisSagligiTakip.Services.Interfaces;
using DisSagligiTakip.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using DisSagligiTakip.Helpers;

namespace DisSagligiTakip.Controllers
{
    [Authorize]
    public class RandevularController : Controller
    {
        private readonly AppDbContext _ctx;
        private readonly IMuayeneRandevuService _svc;

        public RandevularController(AppDbContext ctx, IMuayeneRandevuService svc)
        {
            _ctx = ctx;
            _svc = svc;
        }

        private int? CurrentUserId => HttpContext.Session.GetInt32("UserId");
        private string CurrentRole => User.FindFirstValue(ClaimTypes.Role) ?? "Patient";

        // ---------------- LIST ----------------
        // Liste (rol bazlı)
        [HttpGet]
        public async Task<IActionResult> Index(int? hastaId, int? hekimId, DateTime? start, DateTime? end)
        {
            var uid = CurrentUserId;
            if (uid == null) return RedirectToAction("Login", "Auth");

            var list = await _svc.ListAsync(uid.Value, CurrentRole, start, end, hastaId, hekimId);

            var vm = new RandevuIndexViewModel
            {
                Filtre = new RandevuFilterViewModel
                {
                    HastaUserId = hastaId,
                    HekimUserId = hekimId,
                    Start = start,
                    End = end,
                    Hastalar = GetHastaItems(),
                    Hekimler = GetHekimItems()
                },
                Randevular = list.Select(x => new RandevuListItemViewModel
                {
                    Id = x.Id,
                    HastaAd = x.Hasta?.FullName ?? $"Hasta #{x.HastaUserId}",
                    HekimAd = x.Hekim?.FullName ?? "-",
                    Baslangic = x.BaslangicZamani,
                    Bitis = x.BitisZamani,
                    Durum = x.Durum.ToString(),
                    Konum = x.Konum
                }).ToList()
            };

            return View(vm);
        }

        // Hastanın kendi randevuları
        [HttpGet]
        public async Task<IActionResult> My()
        {
            var uid = CurrentUserId; if (uid == null) return RedirectToAction("Login", "Auth");
            var list = await _svc.ListAsync(uid.Value, "Patient");
            return View("Index", new RandevuIndexViewModel
            {
                Filtre = new RandevuFilterViewModel { Hastalar = GetHastaItems(), Hekimler = GetHekimItems() },
                Randevular = list.Select(x => new RandevuListItemViewModel
                {
                    Id = x.Id,
                    HastaAd = x.Hasta?.FullName ?? $"Hasta #{x.HastaUserId}",
                    HekimAd = x.Hekim?.FullName ?? "-",
                    Baslangic = x.BaslangicZamani,
                    Bitis = x.BitisZamani,
                    Durum = x.Durum.ToString(),
                    Konum = x.Konum
                }).ToList()
            });
        }

        // ---------------- CALENDAR ----------------
        // Görsel takvim sayfası (FullCalendar)
        [HttpGet]
        public IActionResult Takvim()
        {
            // Views/Randevular/Calendar.cshtml
            return View("Calendar");
        }

        // JSON Calendar feed (FullCalendar events kaynağı)
        [HttpGet]
        public async Task<IActionResult> Calendar(DateTime? start, DateTime? end)
        {
            var uid = CurrentUserId; if (uid == null) return Unauthorized();

            var list = await _svc.ListAsync(uid.Value, CurrentRole, start, end);
            var events = list.Select(x => new
            {
                id = x.Id,
                title = (x.Hasta?.FullName ?? $"Hasta #{x.HastaUserId}") + (x.Hekim != null ? $" / {x.Hekim.FullName}" : ""),
                start = x.BaslangicZamani,
                end = x.BitisZamani,
                status = x.Durum.ToString()
            });

            return Json(events);
        }

        // ---------------- CREATE ----------------
        // CREATE GET — takvimden seçili saatle gelebilmek için start/end parametreleri destekler
        [HttpGet]
        public IActionResult Create(DateTime? start, DateTime? end)
        {
            var uid = CurrentUserId; if (uid == null) return RedirectToAction("Login", "Auth");

            var bas = start ?? DateTime.Now.AddHours(1);
            var bit = end ?? bas.AddHours(1);
            if (bit <= bas) bit = bas.AddHours(1);

            var model = new RandevuCreateViewModel
            {
                BaslangicZamani = bas,
                BitisZamani = bit,
                Hastalar = GetHastaItems(limitToCurrentIfPatient: true),
                Hekimler = GetHekimItems()
            };
            return View(model);
        }

        // CREATE POST
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RandevuCreateViewModel vm)
        {
            var uid = CurrentUserId; if (uid == null) return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid)
            {
                vm.Hastalar = GetHastaItems(limitToCurrentIfPatient: true);
                vm.Hekimler = GetHekimItems();
                return View(vm);
            }

            var (ok, error, created) = await _svc.CreateAsync(
                olusturanUserId: uid.Value, currentRole: CurrentRole,
                hastaUserId: vm.HastaUserId, hekimUserId: vm.HekimUserId,
                baslangic: vm.BaslangicZamani, bitis: vm.BitisZamani,
                konum: vm.Konum, aciklama: vm.Aciklama);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, error ?? "Randevu oluşturulamadı.");
                vm.Hastalar = GetHastaItems(limitToCurrentIfPatient: true);
                vm.Hekimler = GetHekimItems();
                return View(vm);
            }

            TempData["Success"] = "Randevu oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------- EDIT ----------------
        // EDIT GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var uid = CurrentUserId; if (uid == null) return RedirectToAction("Login", "Auth");
            var e = await _svc.GetAsync(id);
            if (e == null) return NotFound();

            var model = new RandevuEditViewModel
            {
                Id = e.Id,
                HastaUserId = e.HastaUserId,
                HekimUserId = e.HekimUserId,
                BaslangicZamani = e.BaslangicZamani,
                BitisZamani = e.BitisZamani,
                Durum = e.Durum,
                Konum = e.Konum,
                Aciklama = e.Aciklama,
                Hastalar = GetHastaItems(limitToCurrentIfPatient: false),
                Hekimler = GetHekimItems()
            };
            return View(model);
        }

        // EDIT POST
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RandevuEditViewModel vm)
        {
            var uid = CurrentUserId; if (uid == null) return RedirectToAction("Login", "Auth");
            if (!ModelState.IsValid)
            {
                vm.Hastalar = GetHastaItems(limitToCurrentIfPatient: false);
                vm.Hekimler = GetHekimItems();
                return View(vm);
            }

            var (ok, error) = await _svc.UpdateAsync(
                id: vm.Id, currentUserId: uid.Value, currentRole: CurrentRole,
                hastaUserId: vm.HastaUserId, hekimUserId: vm.HekimUserId,
                baslangic: vm.BaslangicZamani, bitis: vm.BitisZamani,
                durum: vm.Durum, konum: vm.Konum, aciklama: vm.Aciklama);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, error ?? "Randevu güncellenemedi.");
                vm.Hastalar = GetHastaItems(limitToCurrentIfPatient: false);
                vm.Hekimler = GetHekimItems();
                return View(vm);
            }

            TempData["Success"] = "Randevu güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------- DETAILS ----------------
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var e = await _svc.GetAsync(id);
            if (e == null) return NotFound();

            var vm = new RandevuDetayViewModel
            {
                Id = e.Id,
                HastaAd = e.Hasta?.FullName ?? $"Hasta #{e.HastaUserId}",
                HekimAd = e.Hekim?.FullName ?? "-",
                Baslangic = e.BaslangicZamani,
                Bitis = e.BitisZamani,
                Durum = e.Durum.ToString(),
                Konum = e.Konum,
                Aciklama = e.Aciklama
            };
            return View(vm);
        }

        // ---------------- DELETE ----------------
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var uid = CurrentUserId; if (uid == null) return RedirectToAction("Login", "Auth");
            var (ok, error) = await _svc.DeleteAsync(id, uid.Value, CurrentRole);
            if (!ok) TempData["Error"] = error ?? "Silme başarısız.";
            else TempData["Success"] = "Randevu silindi.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------- HELPERS ----------------
        private List<SelectListItem> GetHastaItems(bool limitToCurrentIfPatient = false)
        {
            var uid = CurrentUserId ?? 0;
            var isPatient = CurrentRole == "Patient";

            var q = _ctx.Users.Where(u => u.Role == UserRoles.Patient);
            if (limitToCurrentIfPatient && isPatient) q = q.Where(u => u.Id == uid);

            return q.OrderBy(u => u.FullName)
                    .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FullName })
                    .ToList();
        }

        private List<SelectListItem> GetHekimItems()
        {
            return _ctx.Users.Where(u => u.Role == UserRoles.Doctor)
                .OrderBy(u => u.FullName)
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.FullName })
                .ToList();
        }
    }
}
