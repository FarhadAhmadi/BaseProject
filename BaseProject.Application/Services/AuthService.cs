using AutoMapper;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Constants;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using Serilog;

namespace BaseProject.Application.Services
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITokenService tokenService,
        ICurrentUser currentUser,
        IUserRepository userRepository,
        ICookieService cookieService
    ) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICookieService _cookieService = cookieService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ICurrentUser _currentUser = currentUser;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserSignInResponseDto> SignIn(UserSignInRequestDto request)
        {
            Log.Information("SignIn started | UserName: {UserName}", request.UserName);

            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
                throw UserException.BadRequestException(UserErrorMessage.UserNotExist);

            Log.Debug("User found | UserId: {UserId}", user.Id);

            if (!StringHelper.Verify(request.Password, user.Password))
                throw UserException.BadRequestException(UserErrorMessage.PasswordIncorrect);

            Log.Debug("Password verified successfully | UserId: {UserId}", user.Id);

            var token = _tokenService.GenerateToken(user);
            _cookieService.Set(token);

            Log.Debug("Token generated and set in cookie | UserId: {UserId} | TokenLength: {TokenLength}", user.Id, token.Length);

            var response = _mapper.Map<UserSignInResponseDto>(user);
            response.Token = token;

            Log.Information("SignIn completed successfully | UserId: {UserId}", user.Id);

            return response;
        }

        public async Task<UserSignUpResponseDto> SignUp(UserSignUpRequestDto request, CancellationToken token)
        {
            Log.Information("SignUp started | UserName: {UserName} | Email: {Email}", request.UserName, request.Email);

            if (await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName))
                throw UserException.UserAlreadyExistsException(request.UserName);

            if (await _unitOfWork.UserRepository.AnyAsync(x => x.Email == request.Email))
                throw UserException.UserAlreadyExistsException(request.Email);

            var user = _mapper.Map<User>(request);
            user.Password = user.Password.Hash();

            await _unitOfWork.ExecuteTransactionAsync(async () =>
                await _unitOfWork.UserRepository.AddAsync(user), token
            );

            var response = _mapper.Map<UserSignUpResponseDto>(user);
            Log.Information("SignUp completed successfully | UserId: {UserId}", user.Id);

            return response;
        }

        public void Logout()
        {
            var userId = _currentUser.GetCurrentUserId();
            _cookieService.Delete();
            Log.Information("User logged out successfully | UserId: {UserId}", userId);
        }

        public async Task<UserProfileResponseDto> GetProfile()
        {
            var userId = _currentUser.GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);

            Log.Debug("Profile retrieved | UserId: {UserId}", userId);
            return _mapper.Map<UserProfileResponseDto>(user);
        }

        public async Task<string> RefreshToken()
        {
            var userId = _currentUser.GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);

            var accessToken = _tokenService.GenerateToken(user);
            _cookieService.Set(accessToken);

            Log.Information("Token refreshed successfully | UserId: {UserId} | TokenLength: {TokenLength}", userId, accessToken.Length);

            return accessToken;
        }
    }
}
