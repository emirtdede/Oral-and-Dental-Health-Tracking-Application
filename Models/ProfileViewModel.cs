using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "E-Posta")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Ekranda readonly göstereceğiz

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
