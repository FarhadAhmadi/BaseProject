using Microsoft.AspNetCore.Http;

namespace BaseProject.Application.DTOs.Common.AuthIdentity.UsersIdentity;

public class UserUpdateRequestDto
{
    public string UserId { get; set; }

    public string UserName { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Avatar { get; set; }

    public IFormFile MediaFile { get; set; }

}
