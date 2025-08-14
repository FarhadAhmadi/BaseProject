using BaseProject.Application.DTOs.AuthIdentity.Media;

namespace BaseProject.Application.Common.Interfaces
{
    public interface IMediaService
    {
        Task RemoveMediaAsync(string mediaId, CancellationToken cancellationToken);
        Task UpdateMediaAsync(string mediaId, MediaCreateRequestDto request, CancellationToken cancellationToken);
    }
}
