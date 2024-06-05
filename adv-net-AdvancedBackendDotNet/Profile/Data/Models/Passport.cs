namespace Profile.Data.Models
{
    public class Passport : Document
    {
        public int SeriesNumber { get; set; }
        public string BirthPlace { get; set; } = "";
        public DateOnly GivenDate { get; set; }
        public string GivenPlace { get; set; } = "";
    }
}
