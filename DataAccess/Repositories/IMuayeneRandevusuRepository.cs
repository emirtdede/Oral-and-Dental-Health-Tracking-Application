using DisSagligiTakip.Entities;

namespace DisSagligiTakip.DataAccess.Repositories
{
    public interface IMuayeneRandevusuRepository
    {
        Task<MuayeneRandevusu?> GetByIdAsync(int id);
        Task<List<MuayeneRandevusu>> ListAsync(
            DateTime? start = null, DateTime? end = null,
            int? hastaId = null, int? hekimId = null);

        Task AddAsync(MuayeneRandevusu entity);
        Task UpdateAsync(MuayeneRandevusu entity);
        Task DeleteAsync(MuayeneRandevusu entity);

        Task<bool> HasOverlapForDoctorAsync(int hekimUserId, DateTime start, DateTime end, int? excludeId = null);
        Task<bool> HasOverlapForPatientAsync(int hastaUserId, DateTime start, DateTime end, int? excludeId = null);
    }
}
