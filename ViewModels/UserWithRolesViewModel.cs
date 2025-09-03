using DisSagligiTakip.Entities;
using System.Collections.Generic;

namespace DisSagligiTakip.ViewModels
{
    public class UserWithRolesViewModel
    {
        // Kullanıcı listesi (tüm kullanıcılar)
        public List<User> Users { get; set; }

        // Seçilen kullanıcının ID'si (gizli alan ile taşınacak)
        public string SelectedUserId { get; set; }

        // Yeni atanacak rol (dropdown'dan seçilecek)
        public string NewRole { get; set; }

        // Seçilebilir roller listesi (dropdown için)
        public List<string> AvailableRoles { get; set; }
    }
}
