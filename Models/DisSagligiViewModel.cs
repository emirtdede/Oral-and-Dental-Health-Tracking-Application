using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Models
{
    public class DisSagligiViewModel
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Diş hekimi adı zorunludur.")]
        [Display(Name = "Diş Hekimi")]
        public string DisHekimi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        public string Aciklama { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tarih bilgisi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime Tarih { get; set; }

        // Opsiyonel: Kullanıcı adı göstermek için kullanılabilir
        public string? KullaniciAdi { get; set; }
    }
}
