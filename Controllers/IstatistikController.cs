using System.Security.Claims;
using DisSagligiTakip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisSagligiTakip.Controllers
{
    [Authorize]
    public class IstatistikController : Controller
    {
        private readonly IIstatistikService _svc;
        public IstatistikController(IIstatistikService svc) => _svc = svc;

        private int? CurrentUserId => HttpContext.Session.GetInt32("UserId");
        private string CurrentRole => User.FindFirstValue(ClaimTypes.Role) ?? "Patient";

        public async Task<IActionResult> Index()
        {
            var uid = CurrentUserId; if (uid == null) return RedirectToAction("Login", "Auth");
            var vm = await _svc.BuildLast7DaysAsync(uid.Value, CurrentRole);
            return View(vm);
        }
    }
}
