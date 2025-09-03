namespace DisSagligiTakip.ViewModels
{
    public class RandevuDetayViewModel
    {
        public int Id { get; set; }
        public string HastaAd { get; set; } = "";
        public string HekimAd { get; set; } = "";
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string Durum { get; set; } = "Scheduled";
        public string? Konum { get; set; }
        public string? Aciklama { get; set; }
    }
}
