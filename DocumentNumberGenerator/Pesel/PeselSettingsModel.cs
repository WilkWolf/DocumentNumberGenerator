
namespace DocumentNumberGenerator.Pesel
{
    class PeselSettingsModel
    {
        public string Date { get; set; }
        public bool UseMonth { get; set; }
        public bool UseDay { get; set; }
        public bool UseYear { get; set; }
        public int Gender { get; set; }
        public int Count { get; set; }
    }
}
