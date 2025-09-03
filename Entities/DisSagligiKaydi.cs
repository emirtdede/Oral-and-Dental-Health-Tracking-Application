using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Entities
{
    public class DisSagligiKaydi
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string DisHekimi { get; set; } = string.Empty;

        [Required]
        public string Aciklama { get; set; } = string.Empty;

        [Required]
        public DateTime Tarih { get; set; }

        // İlişki kurulması için (opsiyonel, navigation property)
        public User? User { get; set; }
    }
}
