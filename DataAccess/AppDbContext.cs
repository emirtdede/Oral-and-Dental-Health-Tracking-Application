using DisSagligiTakip.Entities;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ✅ DbSet tanımlamaları
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<FircalamaKaydi> FircalamaKayitlari { get; set; }
        public DbSet<DisSagligiVerisi> DisSagligiVerileri { get; set; }

        // ➕ Randevu DbSet'i
        public DbSet<MuayeneRandevusu> MuayeneRandevulari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ User rolünü string olarak sakla
            modelBuilder.Entity<User>()
                        .Property(u => u.Role)
                        .HasConversion<string>();

            // ✅ Note ile User arasında ilişki (1-n)
            modelBuilder.Entity<Note>()
                        .HasOne(n => n.User)
                        .WithMany()
                        .HasForeignKey(n => n.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            // ✅ FircalamaKaydi ile User arasında ilişki (1-n)
            modelBuilder.Entity<FircalamaKaydi>()
                        .HasOne(f => f.User)
                        .WithMany()
                        .HasForeignKey(f => f.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            // ✅ DisSagligiVerisi: Kaydı oluşturan kullanıcı
            modelBuilder.Entity<DisSagligiVerisi>()
                        .HasOne(d => d.KaydiOlusturanKullanici)
                        .WithMany()
                        .HasForeignKey(d => d.UserId)
                        .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse bile geçmiş kayıtlar korunabilir

            // ✅ DisSagligiVerisi: Hasta (başkası adına kayıt)
            modelBuilder.Entity<DisSagligiVerisi>()
                        .HasOne(d => d.Hasta)
                        .WithMany()
                        .HasForeignKey(d => d.HastaUserId)
                        .OnDelete(DeleteBehavior.Restrict);

            // ➕ MuayeneRandevusu ilişkileri & indeksler
            modelBuilder.Entity<MuayeneRandevusu>(b =>
            {
                b.HasOne(x => x.Hasta)
                 .WithMany()
                 .HasForeignKey(x => x.HastaUserId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Hekim)
                 .WithMany()
                 .HasForeignKey(x => x.HekimUserId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.Olusturan)
                 .WithMany()
                 .HasForeignKey(x => x.OlusturanUserId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasIndex(x => x.BaslangicZamani);
                b.HasIndex(x => new { x.HekimUserId, x.BaslangicZamani });
                b.HasIndex(x => new { x.HastaUserId, x.BaslangicZamani });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
