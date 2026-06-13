using DisSagligiTakip.Entities;
using System.Collections.Generic;

namespace DisSagligiTakip.ViewModels
{
    public class UserWithRolesViewModel
    {
        // Kullanıcı listesi (tüm kullanıcılar)
        public List<User> Users { get; set; } = new();

        // Seçilen kullanıcının ID'si (gizli alan ile taşınacak)
        public string SelectedUserId { get; set; } = string.Empty;

        // Yeni atanacak rol (dropdown'dan seçilecek)
        public string NewRole { get; set; } = string.Empty;

        // Seçilebilir roller listesi (dropdown için)
        public List<string> AvailableRoles { get; set; } = new();
    }
}
