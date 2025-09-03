using DisSagligiTakip.DataAccess;
using DisSagligiTakip.DataAccess.Repositories;
using DisSagligiTakip.Entities;
using DisSagligiTakip.Entities.Enums;
using DisSagligiTakip.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.Services
{
    public class MuayeneRandevuService : Interfaces.IMuayeneRandevuService
    {
        private readonly AppDbContext _ctx;
        private readonly IMuayeneRandevusuRepository _repo;

        public MuayeneRandevuService(AppDbContext ctx, IMuayeneRandevusuRepository repo)
        {
            _ctx = ctx;
            _repo = repo;
        }

        public Task<MuayeneRandevusu?> GetAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<List<MuayeneRandevusu>> ListAsync(
            int currentUserId, string currentRole,
            DateTime? start = null, DateTime? end = null,
            int? hastaId = null, int? hekimId = null)
        {
            var isPatient = string.Equals(currentRole, UserRoles.Patient.ToString(), StringComparison.OrdinalIgnoreCase);
            var isDoctorOrStaff = currentRole is nameof(UserRoles.Doctor) or nameof(UserRoles.Assistant) or nameof(UserRoles.Admin) or nameof(UserRoles.SuperAdmin);

            if (isPatient)
            {
                // Hasta kendi randevularını görür
                return await _repo.ListAsync(start, end, hastaId: currentUserId, hekimId: null);
            }

            if (string.Equals(currentRole, nameof(UserRoles.Doctor), StringComparison.OrdinalIgnoreCase))
            {
                // Doktor varsayılan olarak kendi randevularını görür
                return await _repo.ListAsync(start, end, hastaId, hekimId: currentUserId);
            }

            if (isDoctorOrStaff)
            {
                // Admin/SuperAdmin/Assistant tümünü filtreyle görebilir
                return await _repo.ListAsync(start, end, hastaId, hekimId);
            }

            return new List<MuayeneRandevusu>();
        }

        public async Task<(bool ok, string? error, MuayeneRandevusu? created)> CreateAsync(
            int olusturanUserId, string currentRole,
            int hastaUserId, int? hekimUserId,
            DateTime baslangic, DateTime bitis,
            string? konum, string? aciklama)
        {
            if (baslangic >= bitis)
                return (false, "Başlangıç zamanı, bitişten önce olmalıdır.", null);

            // Hasta kendi adına randevu açabilir; Doctor/Assistant/Admin herkes için açabilir
            var isPatient = currentRole == nameof(UserRoles.Patient);
            if (isPatient && hastaUserId != olusturanUserId)
                return (false, "Hasta yalnızca kendi adına randevu oluşturabilir.", null);

            // Çakışma kontrolleri
            if (hekimUserId.HasValue)
            {
                var overlapHekim = await _repo.HasOverlapForDoctorAsync(hekimUserId.Value, baslangic, bitis);
                if (overlapHekim) return (false, "Hekim için bu saatlerde başka randevu var.", null);
            }
            var overlapHasta = await _repo.HasOverlapForPatientAsync(hastaUserId, baslangic, bitis);
            if (overlapHasta) return (false, "Hasta için bu saatlerde başka randevu var.", null);

            var entity = new MuayeneRandevusu
            {
                HastaUserId = hastaUserId,
                HekimUserId = hekimUserId,
                OlusturanUserId = olusturanUserId,
                BaslangicZamani = baslangic,
                BitisZamani = bitis,
                Konum = konum,
                Aciklama = aciklama,
                Durum = RandevuDurumu.Scheduled
            };

            await _repo.AddAsync(entity);
            return (true, null, entity);
        }

        public async Task<(bool ok, string? error)> UpdateAsync(
            int id, int currentUserId, string currentRole,
            int hastaUserId, int? hekimUserId,
            DateTime baslangic, DateTime bitis,
            RandevuDurumu durum, string? konum, string? aciklama)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return (false, "Randevu bulunamadı.");

            var isPatient = currentRole == nameof(UserRoles.Patient);
            // Hasta sadece kendi randevusunu düzenleyebilsin
            if (isPatient && entity.HastaUserId != currentUserId)
                return (false, "Bu randevuyu düzenleme yetkiniz yok.");

            if (baslangic >= bitis)
                return (false, "Başlangıç zamanı, bitişten önce olmalıdır.");

            // Çakışma kontrolleri (kendini hariç tut)
            if (hekimUserId.HasValue)
            {
                var overlapHekim = await _repo.HasOverlapForDoctorAsync(hekimUserId.Value, baslangic, bitis, excludeId: id);
                if (overlapHekim) return (false, "Hekim için bu saatlerde başka randevu var.");
            }
            var overlapHasta = await _repo.HasOverlapForPatientAsync(hastaUserId, baslangic, bitis, excludeId: id);
            if (overlapHasta) return (false, "Hasta için bu saatlerde başka randevu var.");

            // Güncelle
            entity.HastaUserId = hastaUserId;
            entity.HekimUserId = hekimUserId;
            entity.BaslangicZamani = baslangic;
            entity.BitisZamani = bitis;
            entity.Durum = durum;
            entity.Konum = konum;
            entity.Aciklama = aciklama;

            await _repo.UpdateAsync(entity);
            return (true, null);
        }

        public async Task<(bool ok, string? error)> DeleteAsync(int id, int currentUserId, string currentRole)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return (false, "Randevu bulunamadı.");

            var isPatient = currentRole == nameof(UserRoles.Patient);

            // Hasta ise sadece kendi randevusunu silebilir
            if (isPatient && entity.HastaUserId != currentUserId)
                return (false, "Bu randevuyu silme yetkiniz yok.");

            await _repo.DeleteAsync(entity);
            return (true, null);
        }
    }
}
