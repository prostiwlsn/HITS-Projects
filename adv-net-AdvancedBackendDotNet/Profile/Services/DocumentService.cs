using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Profile.Data;
using Profile.Data.Models;
using Profile.Interfaces;
using System;

//using Profile.Models;
using System.Collections.Generic;
using System.Linq;

namespace Profile.Services
{
    public class DocumentService : INotRestrictedDocumentService
    {
        private readonly ProfileDbContext _profileDbContext;
        private readonly IConfiguration _configuration;
        private readonly IBus _bus;
        public DocumentService(ProfileDbContext profileDbContext, IConfiguration configuration, IBus bus)
        {
            _profileDbContext = profileDbContext;
            _configuration = configuration;
            _bus = bus;
        }

        public async Task<Result> DeleteFile(Guid fileId, Guid userId)
        {
            var file = await _profileDbContext.Files.Include(file => file.Document).FirstOrDefaultAsync(file => file.FileId == fileId);

            if (file == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "File not found" };
            else if (userId != file.Document.UserId)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Unauthorized" };

            _profileDbContext.Remove(file);
            _profileDbContext.SaveChanges();

            File.Delete(file.Path);

            return new Result { Success = true };
        }

        public async Task<Result> DeleteFile(Guid fileId)
        {
            var file = _profileDbContext.Files.Find(fileId);

            if (file == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "File not found" };

            _profileDbContext.Remove(file);
            _profileDbContext.SaveChanges();

            File.Delete(file.Path);

            return new Result { Success = true };
        }

        public async Task<Result> EditEducationDocumentInfo(Guid userId, EducationDocumentEditModel model)
        {
            var document = _profileDbContext.EducationDocuments.FirstOrDefault(doc => doc.UserId == userId);

            if (document == null)
                return new Result { Success = false, Message = "Document not found", ResultMessage = ResultMessage.NotFound };

            var response = _bus.Rpc.Request<GetDocumentTypeInfoRequest, Result<DocumentTypeInfoModel>>(new GetDocumentTypeInfoRequest { Id = model.Id });

            if (!response.Success)
                return response.CastToResult();

            document.Name = response.Load.Name;
            document.NextEducationLevels = response.Load.NextEducationLevels.ToArray();
            document.DocumentTypeId = model.Id;

            _bus.PubSub.Publish<UpdateApplicationMessage>(new UpdateApplicationMessage { Id = userId });

            //document.Name = model.Name;
            _profileDbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<Result> EditPassportInfo(Guid userId, PassportEditModel model)
        {
            var document = _profileDbContext.Passports.FirstOrDefault(doc => doc.UserId == userId);

            if (document == null)
                return new Result { Success = false, Message = "Document not found", ResultMessage = ResultMessage.NotFound };

            document.SeriesNumber = model.SeriesNumber;
            document.BirthPlace = model.BirthPlace;
            document.GivenPlace = model.GivenPlace;
            document.GivenDate = DateOnly.FromDateTime(model.GivenDate);

            _profileDbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<Result<FileList>> GetEducationDocumentFiles(Guid userId)
        {
            var document = await _profileDbContext.EducationDocuments.Include(document => document.Files).FirstOrDefaultAsync(doc => doc.UserId == userId);

            if (document == null)
                return new Result<FileList> { Success = false, Message = "Document not found", ResultMessage = ResultMessage.NotFound };

            FileList list = new FileList();
            list.Files = document.Files.Select(file => file.FileId).ToList();

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(list) + "xdddd");

            return new Result<FileList> { Success = true, Load = list };
        }

        public async Task<Result<EducationDocumentInfoModel>> GetEducationDocumentInfo(Guid userId)
        {
            var document = await _profileDbContext.EducationDocuments.FirstOrDefaultAsync(doc => doc.UserId == userId);

            if (document == null)
                return new Result<EducationDocumentInfoModel> { Success = false, Message = "Document not found", ResultMessage = ResultMessage.NotFound };

            var info = new EducationDocumentInfoModel
            {
                Name = document.Name,
                DocumentTypeId = document.DocumentTypeId,
                NextEducationLevels = document.NextEducationLevels,
            };

            return new Result<EducationDocumentInfoModel> { Success = true, Load = info };
        }

        public async Task<Result<string>> GetFile(Guid fileId, Guid userId)
        {
            var document = await _profileDbContext.Files.Include(file => file.Document).FirstOrDefaultAsync(doc => doc.FileId == fileId);

            if (document == null)
                return new Result<string> { Success = false, Message = "File not found", ResultMessage = ResultMessage.NotFound };
            else if (userId != document.Document.UserId)
                return new Result<string> { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Unauthorized" };

            return new Result<string> { Success = true, Load = document.Path };
        }

        public async Task<Result<GetFileResponse>> GetFile(Guid fileId)
        {
            var document = await _profileDbContext.Files.FirstOrDefaultAsync(doc => doc.FileId == fileId);

            if (document == null)
                return new Result<GetFileResponse> { Success = false, Message = "File not found", ResultMessage = ResultMessage.NotFound };

            byte[] fileBytes = File.ReadAllBytes(document.Path);

            GetFileResponse load = new GetFileResponse
            {
                Bytes = fileBytes,
                ContentType = _configuration.GetSection("ValidContentType").Value??"application/pdf",
                FileName = document.FileId.ToString() + _configuration.GetSection("ValidExtension").Value??".pdf",
            };

            return new Result<GetFileResponse> 
            {
                Success = true,
                Load = load
            };
        }

        public async Task<Result<FileList>> GetPassportFiles(Guid userId)
        {
            var document = await _profileDbContext.Passports.Include(document => document.Files).FirstOrDefaultAsync(doc => doc.UserId == userId);

            if (document == null)
                return new Result<FileList> { Success = false, Message = "Document not found", ResultMessage = ResultMessage.NotFound };

            FileList list = new FileList();
            list.Files = document.Files.Select(file => file.FileId).ToList();

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(list));

            return new Result<FileList> { Success = true, Load = list };
        }

        public async Task<Result<PassportInfoModel>> GetPassportInfo(Guid userId)
        {
            var document = await _profileDbContext.Passports.FirstOrDefaultAsync(doc => doc.UserId == userId);

            if (document == null)
                return new Result<PassportInfoModel> { Success = false, Message = "Document not found", ResultMessage = ResultMessage.NotFound };

            var info = new PassportInfoModel
            {
                SeriesNumber = document.SeriesNumber,
                GivenDate = document.GivenDate,
                GivenPlace = document.GivenPlace,
                BirthPlace = document.BirthPlace,
            };

            return new Result<PassportInfoModel> { Success = true, Load = info };
        }

        public async Task<Result> UploadEducationDocumentFile(Guid userId, FileModel model)
        {
            Guid fileId = Guid.NewGuid();

            var fileExtension = Path.GetExtension(model.File.FileName);
            if (fileExtension != ".pdf")
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "Incorrect file extension" };
            Console.WriteLine(fileExtension);

            string filePath = _configuration.GetSection("StoragePath").Value + fileId.ToString() + fileExtension;

            var document = await _profileDbContext.EducationDocuments.FirstOrDefaultAsync(pass => pass.UserId == userId);

            if (document == null)
                return new Result { Success = false, Message = "User not found", ResultMessage = ResultMessage.NotFound };

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            DocumentFile file = new DocumentFile
            {
                Document = document,
                FileId = fileId,
            };

            file.Path = filePath;

            _profileDbContext.Files.Add(file);
            _profileDbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<Result> UploadPassportFile(Guid userId, FileModel model)
        {
            Guid fileId = Guid.NewGuid();

            var passport = await _profileDbContext.Passports.FirstOrDefaultAsync(pass => pass.UserId == userId);

            Console.WriteLine(userId);

            if (passport == null)
            {
                return new Result { Success = false, Message = "User not found", ResultMessage = ResultMessage.NotFound };
            }

            Console.WriteLine(userId.ToString() + "mewowaw");

            var fileExtension = Path.GetExtension(model.File.FileName);

            if (fileExtension != ".pdf")
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "Incorrect file extension" };
            Console.WriteLine(fileExtension);

            string filePath = _configuration.GetSection("StoragePath").Value + fileId.ToString() + fileExtension;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            DocumentFile file = new DocumentFile
            {
                Document = passport,
                FileId = fileId,
            };

            file.Path = filePath;

            _profileDbContext.Files.Add(file);
            _profileDbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<Result> UploadEducationDocumentFile(UploadBytesMessage msg)
        {
            Guid fileId = Guid.NewGuid();

            var fileExtension = msg.FileExtension;

            if (fileExtension != ".pdf")
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "Incorrect file extension" };

            string filePath = _configuration.GetSection("StoragePath").Value + fileId.ToString() + fileExtension;

            var document = await _profileDbContext.EducationDocuments.FirstOrDefaultAsync(pass => pass.UserId == msg.UserId);

            if (document == null)
                return new Result { Success = false, Message = "User not found", ResultMessage = ResultMessage.NotFound };

            File.WriteAllBytes(filePath, msg.Bytes);

            DocumentFile file = new DocumentFile
            {
                Document = document,
                FileId = fileId,
            };

            file.Path = filePath;

            _profileDbContext.Files.Add(file);
            _profileDbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<Result> UploadPassportFile(UploadBytesMessage msg)
        {

            var passport = await _profileDbContext.Passports.FirstOrDefaultAsync(pass => pass.UserId == msg.UserId);

            if (passport == null)
            {
                return new Result { Success = false, Message = "User not found", ResultMessage = ResultMessage.NotFound };
            }

            Guid fileId = Guid.NewGuid();

            var fileExtension = msg.FileExtension;

            if (fileExtension != ".pdf")
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "Incorrect file extension" };

            Console.WriteLine(fileExtension);

            string filePath = _configuration.GetSection("StoragePath").Value + fileId.ToString() + fileExtension;

            File.WriteAllBytes(filePath, msg.Bytes);

            DocumentFile file = new DocumentFile
            {
                Document = passport,
                FileId = fileId,
            };

            file.Path = filePath;

            _profileDbContext.Files.Add(file);
            _profileDbContext.SaveChanges();

            return new Result { Success = true };
        }

        private async Task UploadFile(Guid userId, FileModel model, Document document, Guid fileId)
        {
            var fileExtension = Path.GetExtension(model.File.FileName);
            Console.WriteLine(fileExtension);

            string filePath = _configuration.GetSection("StoragePath").Value + fileId.ToString() + fileExtension;

            DocumentFile file = new DocumentFile
            {
                Document = document,
                FileId = fileId,
            };

            file.Path = filePath;

            _profileDbContext.Files.Add(file);
            _profileDbContext.SaveChanges();
        }
    }
}
