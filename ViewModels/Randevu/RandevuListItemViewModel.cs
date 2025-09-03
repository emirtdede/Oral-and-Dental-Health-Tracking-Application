namespace DisSagligiTakip.ViewModels
{
    public class RandevuListItemViewModel
    {
        public int Id { get; set; }
        public string HastaAd { get; set; } = "";
        public string HekimAd { get; set; } = "";
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string Durum { get; set; } = "Scheduled";
        public string? Konum { get; set; }
    }

    public class RandevuFilterViewModel
    {
        public int? HastaUserId { get; set; }
        public int? HekimUserId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Hastalar { get; set; } = new();
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Hekimler { get; set; } = new();
    }

    public class RandevuIndexViewModel
    {
        public RandevuFilterViewModel Filtre { get; set; } = new();
        public List<RandevuListItemViewModel> Randevular { get; set; } = new();
    }
}
