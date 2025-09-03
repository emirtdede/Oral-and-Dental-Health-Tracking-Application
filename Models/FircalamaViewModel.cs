using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Models
{
    public class FircalamaViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Tarih alanı zorunludur.")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Aciklama { get; set; }

        public IFormFile? Gorsel { get; set; }  // Yeni görsel yüklenecekse

        public string? MevcutGorselYolu { get; set; }  // Güncelleme için mevcut görsel
    }
}
