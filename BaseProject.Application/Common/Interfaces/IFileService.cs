using BaseProject.Application.DTOs.File;
using Microsoft.AspNetCore.Http;

namespace BaseProject.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task DeleteFileAsync(DeleteFileRequestDto request);
        Task<FileUploadResponseDto> AddFileAsync(IFormFile file);
        string GetFileUrl(string fileName);
    }
}