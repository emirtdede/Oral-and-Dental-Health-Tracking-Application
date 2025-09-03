using DisSagligiTakip.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.DataAccess.Repositories
{
    public class MuayeneRandevusuRepository : IMuayeneRandevusuRepository
    {
        private readonly AppDbContext _ctx;
        public MuayeneRandevusuRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<MuayeneRandevusu?> GetByIdAsync(int id) =>
            await _ctx.Set<MuayeneRandevusu>()
                      .Include(x => x.Hasta)
                      .Include(x => x.Hekim)
                      .Include(x => x.Olusturan)
                      .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<MuayeneRandevusu>> ListAsync(
            DateTime? start = null, DateTime? end = null,
            int? hastaId = null, int? hekimId = null)
        {
            var q = _ctx.Set<MuayeneRandevusu>()
                        .Include(x => x.Hasta).Include(x => x.Hekim)
                        .AsQueryable();

            if (start.HasValue) q = q.Where(x => x.BaslangicZamani >= start.Value);
            if (end.HasValue) q = q.Where(x => x.BitisZamani <= end.Value);
            if (hastaId.HasValue) q = q.Where(x => x.HastaUserId == hastaId.Value);
            if (hekimId.HasValue) q = q.Where(x => x.HekimUserId == hekimId.Value);

            return await q.OrderBy(x => x.BaslangicZamani).ToListAsync();
        }

        public async Task AddAsync(MuayeneRandevusu entity)
        {
            _ctx.Set<MuayeneRandevusu>().Add(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(MuayeneRandevusu entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _ctx.Set<MuayeneRandevusu>().Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(MuayeneRandevusu entity)
        {
            _ctx.Set<MuayeneRandevusu>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> HasOverlapForDoctorAsync(int hekimUserId, DateTime start, DateTime end, int? excludeId = null)
        {
            if (hekimUserId <= 0) return false;
            var q = _ctx.Set<MuayeneRandevusu>().Where(x => x.HekimUserId == hekimUserId);
            if (excludeId.HasValue) q = q.Where(x => x.Id != excludeId.Value);
            // Overlap: start < otherEnd && end > otherStart
            return await q.AnyAsync(x => start < x.BitisZamani && end > x.BaslangicZamani);
        }

        public async Task<bool> HasOverlapForPatientAsync(int hastaUserId, DateTime start, DateTime end, int? excludeId = null)
        {
            var q = _ctx.Set<MuayeneRandevusu>().Where(x => x.HastaUserId == hastaUserId);
            if (excludeId.HasValue) q = q.Where(x => x.Id != excludeId.Value);
            return await q.AnyAsync(x => start < x.BitisZamani && end > x.BaslangicZamani);
        }
    }
}
