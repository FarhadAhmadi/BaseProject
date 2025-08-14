using BaseProject.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BaseProject.Application.DTOs.AuthIdentity.Media;

public class MediaCreateRequestDto
{
    public MediaType Type { get; set; }

    public string MediaLink { get; set; }

    public string Caption { get; set; }

    public int Duration { get; set; }

    public DateTime DateCreated { get; set; }

    public IFormFile MediaFile { get; set; }
}
