using Common.Models;
using Dictionary.Data;
using Dictionary.Data.Models;
using Dictionary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dictionary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private DictionaryDbContext _dbContext { get; set; }
        public DictionaryController(DictionaryDbContext dictionaryDbContext) 
        {
            _dbContext = dictionaryDbContext;
        }

        //isDeleted

        [HttpGet]
        [Route("/document_types")]
        public async Task<IActionResult> GetDocumentTypes() => Ok(_dbContext.DocumentTypes
            .Include(document => document.NextEducationLevels)
            .Include(document => document.EducationLevel)
            .Select(document => new DocumentTypeDto{ Id = document.Id, 
                EducationLevel = new EducationLevelInfoModel { Id = document.EducationLevel.Id, Name = document.EducationLevel.Name }, 
                Name = document.Name, CreateTime = document.CreateTime, 
                NextEducationLevels = document.NextEducationLevels.Select( level => new EducationLevelInfoModel { Id = level.Id, Name = level.Name }).ToList() })
            .ToList());

        [HttpGet]
        [Route("/education_levels")]
        public async Task<IActionResult> GetEducationLevels() => Ok(_dbContext.EducationLevels
            .Select(level => new { level.Id, level.Name}).ToList());

        [HttpGet]
        [Route("/faculties")]
        public async Task<IActionResult> GetFaculties() => Ok(_dbContext.Faculties
            .Select(faculty => new FacultyInfoModel { Id= faculty.Id, Name = faculty.Name, CreateTime = faculty.CreateTime }).ToList());

        [HttpGet]
        [Route("/programs")]
        public async Task<IActionResult> GetPrograms(
            [FromQuery] Guid? facultyId,
            [FromQuery] int? educationLevelId,
            [FromQuery] string? educationForm,
            [FromQuery] string? educationLanguage,
            [FromQuery] string? programName,
            [FromQuery] uint size = 10,
            [FromQuery] uint current = 1)
        {
            var programsQuery = _dbContext.Programs.Include(program => program.Faculty).Include(program => program.EducationLevel).AsQueryable();

            if (facultyId != null)
                programsQuery = programsQuery.Where(program => program.Faculty.Id == facultyId);

            if (educationLevelId != null)
                programsQuery = programsQuery.Where(program => program.EducationLevel.Id == educationLevelId);

            if (educationForm != null)
                programsQuery = programsQuery.Where(program => program.EducationForm.Contains(educationForm));

            if (educationLanguage != null)
                programsQuery = programsQuery.Where(program => program.Language.Contains(educationLanguage));

            if (programName != null)
                programsQuery = programsQuery
                    .Where(program => (program.Name + " " + program.Code).Contains(programName));

            current = current > 0 ? current - 1 : 0;

            var programs = await programsQuery
                .Skip((int)(current * size))
                .Take((int)size)
                .Select(program => new ProgramDto
                {
                    Id = program.Id,
                    Name = program.Name,
                    Code = program.Code,
                    EducationForm = program.EducationForm,
                    Language = program.Language,
                    Faculty = new FacultyDto
                    {
                        Id = program.Faculty.Id,
                        Name = program.Faculty.Name
                    },
                    EducationLevel = new EducationLevelDto
                    {
                        Id = program.EducationLevel.Id,
                        Name = program.EducationLevel.Name
                    }
                }).ToListAsync();
            int count = (int)(_dbContext.Programs.Count() / size);

            return Ok(new { programs, pagination = new { size, current = current + 1, count = count+1 } });
        }
    }
}
