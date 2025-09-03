using System.ComponentModel.DataAnnotations;
using DisSagligiTakip.Helpers;

namespace DisSagligiTakip.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public UserRoles Role { get; set; } = UserRoles.Patient;

        // ✅ E-posta doğrulama alanları
        public string? EmailVerificationToken { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;

        // ✅ Parola sıfırlama alanları
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
    }
}
