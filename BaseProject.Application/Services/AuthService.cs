using AutoMapper;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Constants;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork,
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
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.UserName == request.UserName)
                ?? throw UserException.BadRequestException(UserErrorMessage.UserNotExist);

            if (!StringHelper.Verify(request.Password, user.Password))
            {
                throw UserException.BadRequestException(UserErrorMessage.PasswordIncorrect);
            }

            var token = _tokenService.GenerateToken(user);
            _cookieService.Set(token);

            var response = _mapper.Map<UserSignInResponseDto>(user);
            response.Token = token;

            return response;
        }

        public async Task<UserSignUpResponseDto> SignUp(UserSignUpRequestDto request, CancellationToken token)
        {
            var isUserNameExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName);
            if (isUserNameExist)
                throw UserException.UserAlreadyExistsException(request.UserName);

            var isEmailExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.Email);
            if (isEmailExist)
                throw UserException.UserAlreadyExistsException(request.Email);

            var user = _mapper.Map<User>(request);
            user.Password = user.Password.Hash();
            await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.UserRepository.AddAsync(user), token);

            var response = _mapper.Map<UserSignUpResponseDto>(user);

            return response;
        }

        public void Logout()
        {
            try
            {
                _ = _cookieService.Get();
                _cookieService.Delete();
            }
            catch { }
        }

        public async Task<UserProfileResponseDto> GetProfile()
        {
            var userId = _currentUser.GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);

            var result = _mapper.Map<UserProfileResponseDto>(user);
            return result;
        }

        public async Task<string> RefreshToken()
        {
            var user = await _userRepository.GetByIdAsync(_currentUser.GetCurrentUserId());
            var accessToken = _tokenService.GenerateToken(user);
            _cookieService.Set(accessToken);

            return accessToken;
        }
    }

}
