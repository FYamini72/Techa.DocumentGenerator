using Microsoft.AspNetCore.Http;
using Techa.DocumentGenerator.Domain.Entities;

namespace Techa.DocumentGenerator.Application.Services.Interfaces
{
    public interface IAttachmentFileService : IBaseService<AttachmentFile>
    {
        void DeleteFile(int id);
        Task DeleteFile(int id, CancellationToken cancellationToken);
        void DeleteFile(AttachmentFile file);
        Task<AttachmentFile> UploadFile(IFormFile file, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken, bool saveNow = true);
        void Delete(int id, bool saveNow = true);

        byte[] GetFileBytes(string? fileName);
    }
}
