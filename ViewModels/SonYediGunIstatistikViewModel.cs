namespace DisSagligiTakip.ViewModels
{
    public class SonYediGunIstatistikViewModel
    {
        public int FircalamaToplam { get; set; }
        public int NotToplam { get; set; }
        public int DisKaydiToplam { get; set; }
        public int RandevuToplam { get; set; }

        // (Gun, Adet)
        public List<(DateOnly Gun, int Adet)> FircalamaGunluk { get; set; } = new();
    }
}
