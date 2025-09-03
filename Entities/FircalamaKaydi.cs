using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Entities
{
    public class FircalamaKaydi
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Tarih bilgisi zorunludur.")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [MaxLength(500)]
        public string Aciklama { get; set; } = string.Empty;

        public string? GorselYolu { get; set; }

        // Navigation Property (isteğe bağlı)
        public User? User { get; set; }
    }
}
