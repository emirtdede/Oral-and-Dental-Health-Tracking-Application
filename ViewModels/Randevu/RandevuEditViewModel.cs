using System.ComponentModel.DataAnnotations;
using DisSagligiTakip.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DisSagligiTakip.ViewModels
{
    public class RandevuEditViewModel : RandevuCreateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public RandevuDurumu Durum { get; set; } = RandevuDurumu.Scheduled;
    }
}
