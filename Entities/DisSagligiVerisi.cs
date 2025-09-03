using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisSagligiTakip.Entities
{
    public class DisSagligiVerisi
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Kaydı oluşturan kullanıcının ID'si (hasta kendi kaydı / doktor-asistan başkasının kaydı)</summary>
        public int UserId { get; set; }

        /// <summary>Kayıt bir hasta adına oluşturulduysa (doktor/asistan tarafından), o hastanın UserId'si</summary>
        public int? HastaUserId { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [MaxLength(2000)]
        public string Aciklama { get; set; } = string.Empty;

        /// <summary>Diş hekiminin adı (serbest giriş)</summary>
        [MaxLength(100)]
        public string? DisHekimiAdi { get; set; }

        /// <summary>
        /// Görünümler "DisHekimi" kullanıyorsa kırılmasın diye alias property.
        /// Veritabanında bir alan oluşturmaz.
        /// </summary>
        [NotMapped]
        public string? DisHekimi
        {
            get => DisHekimiAdi;
            set => DisHekimiAdi = value;
        }

        /// <summary>Görsele ilişkin sanal dosya yolu (~/uploads/...)</summary>
        public string? GorselYolu { get; set; }

        /// <summary>Kaydın oluşturulma tarihi</summary>
        public DateTime Tarih { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public User? KaydiOlusturanKullanici { get; set; }

        [ForeignKey(nameof(HastaUserId))]
        public User? Hasta { get; set; }
    }
}
