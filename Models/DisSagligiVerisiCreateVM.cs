using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Models
{
    /// <summary>
    /// Diş sağlığı kaydı oluşturma/düzenleme için ViewModel
    /// </summary>
    public class DisSagligiVerisiCreateVM
    {
        public int? Id { get; set; }                 // Edit için

        // Kayıt kimin için? (Patient dışındaki roller seçebilir)
        public int? HastaUserId { get; set; }

        // Doktor adı: listeden seçilebilir veya serbest yazılabilir
        [MaxLength(100)]
        public string? DisHekimiAdi { get; set; }

        [Required, MaxLength(2000)]
        public string Aciklama { get; set; } = string.Empty;

        public IFormFile? Gorsel { get; set; }       // Yeni/Değiştirilecek görsel
        public string? MevcutGorselYolu { get; set; } // Edit için mevcut görsel

        // Dropdown veri kaynakları
        public IEnumerable<SelectListItem> Hastalar { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Doktorlar { get; set; } = new List<SelectListItem>();
    }
}
