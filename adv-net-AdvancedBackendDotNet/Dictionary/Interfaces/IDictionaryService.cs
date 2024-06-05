using Common.Models;
using Common.Result;
using Dictionary.Data.Models;

namespace Dictionary.Interfaces
{
    public interface IDictionaryService
    {
        Result<EducationLevelInfoModel> GetEducationLevelInfo(int id);
        Result<DocumentTypeInfoModel> GetDocumentTypeInfo(Guid id);
        Result<FacultyInfoModel> GetFacultyInfo(Guid id);
        Result<ProgramInfoModel> GetProgramInfo(Guid id);
    }
}
