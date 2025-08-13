using BaseProject.Application.DTOs.User;

namespace BaseProject.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<UserSignInResponseDto> SignIn(UserSignInRequestDto request);
        Task<UserSignUpResponseDto> SignUp(UserSignUpRequestDto request, CancellationToken token);
        void Logout();
        Task<string> RefreshToken();
        Task<UserProfileResponseDto> GetProfile();
    }
}
