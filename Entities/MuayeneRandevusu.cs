using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DisSagligiTakip.Entities.Enums;

namespace DisSagligiTakip.Entities
{
    public class MuayeneRandevusu
    {
        public int Id { get; set; }

        [Required]
        public int HastaUserId { get; set; }

        public int? HekimUserId { get; set; } // Doctor olabilir

        [Required]
        public int OlusturanUserId { get; set; }

        [Required]
        public DateTime BaslangicZamani { get; set; } // local time

        [Required]
        public DateTime BitisZamani { get; set; }

        [Required]
        public RandevuDurumu Durum { get; set; } = RandevuDurumu.Scheduled;

        [MaxLength(200)]
        public string? Konum { get; set; }

        [MaxLength(2000)]
        public string? Aciklama { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigations
        [ForeignKey(nameof(HastaUserId))]
        public User? Hasta { get; set; }

        [ForeignKey(nameof(HekimUserId))]
        public User? Hekim { get; set; }

        [ForeignKey(nameof(OlusturanUserId))]
        public User? Olusturan { get; set; }
    }
}
