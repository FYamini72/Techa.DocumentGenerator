using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Techa.DocumentGenerator.Application.Repositories;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities;

namespace Techa.DocumentGenerator.Application.Services.Implementations
{
    public class AttachmentFileService : BaseService<AttachmentFile>, IAttachmentFileService
    {
        private readonly IBaseRepository<AttachmentFile> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _strRootPath;

        public AttachmentFileService(IBaseRepository<AttachmentFile> repository
            , IWebHostEnvironment webHostEnvironment
            , IBaseRepository<EventLog> eventLogRepository
            , IConfiguration configuration
            , IHttpContextAccessor httpContext) : base(repository, eventLogRepository, configuration, httpContext)
        {
            this._repository = repository;
            this._webHostEnvironment = webHostEnvironment;
            this._strRootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/StaticFiles");
        }

        public async Task<AttachmentFile> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            if (file != null && file.Length > 0)
            {

                string filename = $"{Guid.NewGuid().ToString().Trim()}{Path.GetExtension(file.FileName)}";
                var size = file.Length;
                string strFilePathName = Path.Combine(_strRootPath, filename);

                using (var fileStream = new FileStream(strFilePathName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream, cancellationToken);
                }

                return new AttachmentFile()
                {
                    FileName = filename,
                    Size = size
                };
            }

            return null;
        }

        public void DeleteFile(int id)
        {
            var file = _repository.GetById(id);
            DeleteFile(file);
        }

        public async Task DeleteFile(int id, CancellationToken cancellationToken)
        {
            var file = await _repository.GetByIdAsync(cancellationToken, id);
            DeleteFile(file);
        }

        public void DeleteFile(AttachmentFile file)
        {
            if (file != null)
            {
                var strFilePath = Path.Combine(_strRootPath, file.FileName);

                if (File.Exists(strFilePath))
                    File.Delete(strFilePath);
            }
        }

        public override void Delete(AttachmentFile entity, bool saveNow = true)
        {
            base.Delete(entity, saveNow);
            DeleteFile(entity);
        }

        public override async Task DeleteAsync(AttachmentFile entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            await base.DeleteAsync(entity, cancellationToken, saveNow);
            DeleteFile(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken, bool saveNow = true)
        {
            var obj = await GetByIdAsync(cancellationToken, id);
            await DeleteAsync(obj, cancellationToken, saveNow);
        }

        public void Delete(int id, bool saveNow = true)
        {
            var obj = GetById(id);
            Delete(obj);
        }

        public byte[] GetFileBytes(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            string filePath = Path.Combine(_strRootPath, fileName);
            var fileBytes = File.ReadAllBytes(filePath);

            return fileBytes;
        }
    }
}
