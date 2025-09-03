using DisSagligiTakip.ViewModels;

namespace DisSagligiTakip.Services.Interfaces
{
    public interface IIstatistikService
    {
        Task<SonYediGunIstatistikViewModel> BuildLast7DaysAsync(int currentUserId, string currentRole);
    }
}
