using DisSagligiTakip.Entities;
using DisSagligiTakip.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DisSagligiTakip.DataAccess
{
    public static class SeedData
    {
        private const string SuperAdminEmail = "DisSagligiTakipYonetici@gmail.com";
        private const string SuperAdminPassword = "TRtekYazilim4134";

        public static void Initialize(AppDbContext context)
        {
            try
            {
                if (!context.Database.CanConnect())
                    return;

                if (context.Users.Any(u => u.Email == SuperAdminEmail))
                    return;

                var superAdminUser = new User
                {
                    FullName = "Yönetici Kullanıcı",
                    Email = SuperAdminEmail,
                    BirthDate = new DateTime(1990, 1, 1),
                    PasswordHash = PasswordHelper.HashPassword(SuperAdminPassword),
                    Role = UserRoles.SuperAdmin,
                    IsEmailConfirmed = true,
                    EmailVerificationToken = null,
                    PasswordResetToken = null,
                    PasswordResetTokenExpires = null
                };

                context.Users.Add(superAdminUser);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SeedData error: " + ex.Message);
            }
        }
    }
}
