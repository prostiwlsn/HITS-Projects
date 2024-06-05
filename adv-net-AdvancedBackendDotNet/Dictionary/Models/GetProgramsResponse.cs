using Dictionary.Data.Models;

namespace Dictionary.Models
{
    public class GetProgramsResponse
    {
        public List<EducationProgram> Programs { get; set; }

        public Pagination Pagination { get; set; }
    }
}
