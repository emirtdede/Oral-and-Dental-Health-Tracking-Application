using DisSagligiTakip.Entities;
using DisSagligiTakip.Entities.Enums;

namespace DisSagligiTakip.Services.Interfaces
{
    public interface IMuayeneRandevuService
    {
        Task<MuayeneRandevusu?> GetAsync(int id);
        Task<List<MuayeneRandevusu>> ListAsync(
            int currentUserId, string currentRole,
            DateTime? start = null, DateTime? end = null,
            int? hastaId = null, int? hekimId = null);

        Task<(bool ok, string? error, MuayeneRandevusu? created)> CreateAsync(
            int olusturanUserId, string currentRole,
            int hastaUserId, int? hekimUserId,
            DateTime baslangic, DateTime bitis,
            string? konum, string? aciklama);

        Task<(bool ok, string? error)> UpdateAsync(
            int id, int currentUserId, string currentRole,
            int hastaUserId, int? hekimUserId,
            DateTime baslangic, DateTime bitis,
            RandevuDurumu durum, string? konum, string? aciklama);

        Task<(bool ok, string? error)> DeleteAsync(int id, int currentUserId, string currentRole);
    }
}
