using DisSagligiTakip.DataAccess;
using DisSagligiTakip.Helpers;
using DisSagligiTakip.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.Services
{
    public class IstatistikService : Interfaces.IIstatistikService
    {
        private readonly AppDbContext _ctx;
        public IstatistikService(AppDbContext ctx) => _ctx = ctx;

        public async Task<SonYediGunIstatistikViewModel> BuildLast7DaysAsync(int currentUserId, string currentRole)
        {
            var (startDate, endDate, days) = DateRangeHelper.Last7Days();
            var isPatient = currentRole.Equals("Patient", StringComparison.OrdinalIgnoreCase);
            var isDoctor = currentRole.Equals("Doctor", StringComparison.OrdinalIgnoreCase);
            var isStaff = currentRole is "Admin" or "SuperAdmin" or "Assistant";

            // Filtre: hasta ise kendi verileri; doktor ise kendi oluşturdukları + kendi randevuları; staff tüm veriler
            IQueryable<Entities.FircalamaKaydi> qF = _ctx.FircalamaKayitlari.AsNoTracking().Where(x => x.Tarih >= startDate && x.Tarih < endDate);
            IQueryable<Entities.Note> qN = _ctx.Notes.AsNoTracking().Where(x => x.Tarih >= startDate && x.Tarih < endDate);
            IQueryable<Entities.DisSagligiVerisi> qD = _ctx.DisSagligiVerileri.AsNoTracking().Where(x => x.Tarih >= startDate && x.Tarih < endDate);
            IQueryable<Entities.MuayeneRandevusu> qR = _ctx.Set<Entities.MuayeneRandevusu>().AsNoTracking()
                .Where(x => x.BaslangicZamani >= startDate && x.BaslangicZamani < endDate);

            if (isPatient)
            {
                qF = qF.Where(x => x.UserId == currentUserId);
                qN = qN.Where(x => x.UserId == currentUserId);
                qD = qD.Where(x => x.UserId == currentUserId || x.HastaUserId == currentUserId);
                qR = qR.Where(x => x.HastaUserId == currentUserId);
            }
            else if (isDoctor)
            {
                qR = qR.Where(x => x.HekimUserId == currentUserId);
            }

            // Günlük dağılım (Firçalama)
            var fircGroup = await qF.GroupBy(x => x.Tarih.Date)
                .Select(g => new { Gun = g.Key, Adet = g.Count() }).ToListAsync();

            var gunluk = new List<(DateOnly Gun, int Adet)>();
            foreach (var d in days)
            {
                var adet = fircGroup.FirstOrDefault(x => x.Gun.Date == d.ToDateTime(TimeOnly.MinValue))?.Adet ?? 0;
                gunluk.Add((d, adet));
            }

            var vm = new SonYediGunIstatistikViewModel
            {
                FircalamaToplam = await qF.CountAsync(),
                NotToplam = await qN.CountAsync(),
                DisKaydiToplam = await qD.CountAsync(),
                RandevuToplam = await qR.CountAsync(),
                FircalamaGunluk = gunluk
            };
            return vm;
        }
    }
}
