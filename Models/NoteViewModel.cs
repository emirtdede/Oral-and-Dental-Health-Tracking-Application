using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DisSagligiTakip.Models
{
    public class NoteViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        [StringLength(5000, ErrorMessage = "İçerik en fazla 5000 karakter olabilir.")]
        public string Icerik { get; set; }

        // Yeni görsel dosyası yüklemek için
        public IFormFile? Gorsel { get; set; }

        // Eski görsel yolunu korumak için (Edit ekranında görünür)
        public string? MevcutGorselYolu { get; set; }
    }
}
