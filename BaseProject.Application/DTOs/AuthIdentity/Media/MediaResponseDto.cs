using BaseProject.Domain.Enums;

namespace BaseProject.Application.DTOs.AuthIdentity.Media;

public class MediaResponseDto
{
    public int MediaId { get; set; }

    public MediaType Type { get; set; }

    public string PathMedia { get; set; }

    public string Caption { get; set; }

    public int Duration { get; set; }

    public DateTime DateCreated { get; set; }

    public Status Status { get; set; }

    public long FileSize { get; set; }

}
