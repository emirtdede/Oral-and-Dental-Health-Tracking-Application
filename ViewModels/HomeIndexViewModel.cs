namespace DisSagligiTakip.ViewModels
{
    public class HomeIndexViewModel
    {
        public string FullName { get; set; } = "Kullanıcı";
        public string Role { get; set; } = "Patient";
        public bool IsAuthenticated { get; set; }

        public bool IsPatient { get; set; }
        public bool IsDoctor { get; set; }
        public bool IsAssistant { get; set; }
        public bool IsAdmin { get; set; }
    }
}
