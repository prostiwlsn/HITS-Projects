namespace Dictionary.Models
{
    public class ProgramDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string EducationForm { get; set; }
        public string Language { get; set; }
        public FacultyDto Faculty { get; set; }
        public EducationLevelDto EducationLevel { get; set; }
    }

    public class FacultyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class EducationLevelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
