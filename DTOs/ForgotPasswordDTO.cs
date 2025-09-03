using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }
    }
}
