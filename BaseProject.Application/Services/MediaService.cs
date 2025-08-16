using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.DTOs.AuthIdentity.Media;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BaseProject.Application.Services
{
    public class MediaService(
    IFileService storageService,
    ILogger<MediaService> logger,
    IUnitOfWork unitOfWork) : IMediaService
    {
        private readonly IFileService _storageService = storageService;
        private readonly ILogger<MediaService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        // Xóa media
        public async Task RemoveMediaAsync(string mediaId, CancellationToken cancellationToken)
        {
            var media = await _unitOfWork.Media.GetFirstOrDefaultAsync<Media>(
                filter: x => x.MediaId == mediaId
            );

            if (media == null)
            {
                _logger.LogWarning($"Cannot find media with id {mediaId}");
                throw new KeyNotFoundException($"Cannot find media with id {mediaId}");
            }

            if (!string.IsNullOrEmpty(media.PathMedia))
            {
                await _storageService.DeleteFileAsync(new DTOs.File.DeleteFileRequestDto { FileName = media.PathMedia });
                _logger.LogInformation($"File {media.PathMedia} has been deleted.");
            }
            await _unitOfWork.ExecuteInTransactionAsync(() =>
                _unitOfWork.Media.Delete(media), cancellationToken
            );
            _logger.LogInformation($"Media with id {mediaId} has been removed.");
        }

        // C?p nh?t media
        public async Task UpdateMediaAsync(string mediaId, MediaCreateRequestDto request, CancellationToken cancellationToken)
        {
            var media = await _unitOfWork.Media.GetFirstOrDefaultAsync<Media>(filter: x => x.MediaId == mediaId);
            if (media == null)
            {
                _logger.LogWarning($"Cannot find media with id {mediaId}");
                throw new KeyNotFoundException($"Cannot find media with id {mediaId}");
            }
            var pathMedia = await _storageService.AddFileAsync(request.MediaFile);

            if (request.MediaFile != null)
            {
                media.PathMedia = pathMedia.Path;
                media.FileSize = request.MediaFile.Length;
            }
            await _unitOfWork.ExecuteInTransactionAsync(() =>
               _unitOfWork.Media.Update(media), cancellationToken
           );

            _logger.LogInformation($"Media with id {mediaId} has been updated.");
        }
    }

}
