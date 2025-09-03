using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Entities
{
    public class Note
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [MaxLength(100)]
        public string Baslik { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        public string Icerik { get; set; } = string.Empty;

        public string? GorselYolu { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;

        public DateTime? GuncellenmeTarihi { get; set; }

        public User? User { get; set; }
    }
}
