namespace Profile.Data.Models
{
    public class EducationDocument : Document
    {
        public string Name { get; set; } = "";
        public Guid DocumentTypeId { get; set; } = Guid.Empty;
        public int[] NextEducationLevels { get; set; } = new int[0];
    }
}
