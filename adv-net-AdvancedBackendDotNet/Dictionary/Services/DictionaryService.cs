using Common.Models;
using Common.Result;
using Dictionary.Data;
using Dictionary.Data.Models;
using Dictionary.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Services
{
    public class DictionaryService : IDictionaryService
    {
        private DictionaryDbContext _dbContext;
        public DictionaryService(DictionaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Result<DocumentTypeInfoModel> GetDocumentTypeInfo(Guid id)
        {
            Console.WriteLine("xdd");
            var document = _dbContext.DocumentTypes
                         .Include(d => d.NextEducationLevels)
                         .FirstOrDefault(d => d.Id == id);
            if (document == null)
            {
                Console.WriteLine("null");
                return new Result<DocumentTypeInfoModel> { Success = false, ResultMessage = ResultMessage.NotFound };
            }
            Console.WriteLine("next");
            Console.WriteLine(document.Name);
            Console.WriteLine(document.EducationLevelId);
            var nextLevels = document.NextEducationLevels.Select(level => level.Id).ToList();
            Console.WriteLine("xdd");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(nextLevels));
            var load = new DocumentTypeInfoModel
            {
                Name = document.Name,
                EducationLevelId = document.EducationLevelId,
                NextEducationLevels = nextLevels
            };
            Console.WriteLine("last");
            Console.WriteLine(load.Name);

            return new Result<DocumentTypeInfoModel> { Success = true, Load = load };
        }

        public Result<EducationLevelInfoModel> GetEducationLevelInfo(int id)
        {
            var level = _dbContext.EducationLevels.Find(id);
            if (level == null)
                return new Result<EducationLevelInfoModel> { Success = false, ResultMessage = ResultMessage.NotFound };

            var load = new EducationLevelInfoModel
            {
                Name = level.Name,
            };

            return new Result<EducationLevelInfoModel> { Success = true, Load = load };
        }

        public Result<FacultyInfoModel> GetFacultyInfo(Guid id)
        {
            var faculty = _dbContext.Faculties.Find(id);
            if (faculty == null)
                return new Result<FacultyInfoModel> { Success = false, ResultMessage = ResultMessage.NotFound };

            var load = new FacultyInfoModel
            {
                Name = faculty.Name,
            };

            return new Result<FacultyInfoModel> { Success = true, Load = load };
        }

        public Result<ProgramInfoModel> GetProgramInfo(Guid id)
        {
            var program = _dbContext.Programs.Include(p => p.Faculty).Include(p => p.EducationLevel).FirstOrDefault( p => p.Id == id);
            if (program == null)
                return new Result<ProgramInfoModel> { Success = false, ResultMessage = ResultMessage.NotFound };

            var load = new ProgramInfoModel
            {
                Name = program.Name,
                Code = program.Code,
                Language = program.Language,
                EducationForm = program.EducationForm,
                FacultyId = program.Faculty.Id,
                EducationLevelId = program.EducationLevel.Id,
            };

            return new Result<ProgramInfoModel> { Success = true, Load = load };
        }
    }
}
