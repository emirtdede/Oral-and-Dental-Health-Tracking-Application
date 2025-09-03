using System.Collections.Generic;
using DisSagligiTakip.Entities;

namespace DisSagligiTakip.ViewModels
{
    public class HastaDetayViewModel
    {
        /// <summary>Hasta nesnesi</summary>
        public User Hasta { get; set; } = null!;

        /// <summary>Geriye dönük uyumluluk: Görünümler Model.User kullanıyorsa Hasta'yı döndürür.</summary>
        public User User => Hasta;

        /// <summary>Hasta ile ilişkili diş kayıtları</summary>
        public List<DisSagligiVerisi> DisKayitlari { get; set; } = new();

        /// <summary>Hasta ile ilişkili fırçalama kayıtları</summary>
        public List<FircalamaKaydi> FircalamaKayitlari { get; set; } = new();
    }
}
