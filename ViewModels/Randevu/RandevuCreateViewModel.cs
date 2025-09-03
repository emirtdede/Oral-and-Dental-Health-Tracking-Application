using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisSagligiTakip.ViewModels
{
    public class RandevuCreateViewModel
    {
        [Required]
        [Display(Name = "Hasta")]
        public int HastaUserId { get; set; }

        [Display(Name = "Hekim (opsiyonel)")]
        public int? HekimUserId { get; set; }

        [Required, Display(Name = "Başlangıç")]
        public DateTime BaslangicZamani { get; set; }

        [Required, Display(Name = "Bitiş")]
        public DateTime BitisZamani { get; set; }

        [MaxLength(200)]
        public string? Konum { get; set; }

        [MaxLength(2000)]
        public string? Aciklama { get; set; }

        // Seçimler
        public List<SelectListItem> Hastalar { get; set; } = new();
        public List<SelectListItem> Hekimler { get; set; } = new();
    }
}
