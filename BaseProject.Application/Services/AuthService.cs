using AutoMapper;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Constants;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using CorrelationId.Abstractions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;

namespace BaseProject.Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            ITokenService tokenService,
                            ICurrentUser currentUser,
                            IUserRepository userRepository,
                            ICookieService cookieService,
                            ICorrelationContextAccessor correlationContextAccessor) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICookieService _cookieService = cookieService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ICurrentUser _currentUser = currentUser;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICorrelationContextAccessor _correlationContextAccessor = correlationContextAccessor;

        public async Task<UserSignInResponseDto> SignIn(UserSignInRequestDto request)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
            var correlationId = _correlationContextAccessor.CorrelationContext?.CorrelationId ?? traceId;

            Log.Information("SignIn started | UserName: {UserName} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                request.UserName, traceId, correlationId);

            try
            {
                var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.UserName == request.UserName);

                if (user == null)
                {
                    Log.Warning("User not found | UserName: {UserName} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                        request.UserName, traceId, correlationId);
                    throw UserException.BadRequestException(UserErrorMessage.UserNotExist);
                }

                Log.Debug("User found | UserId: {UserId} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    user.Id, traceId, correlationId);

                if (!StringHelper.Verify(request.Password, user.Password))
                {
                    Log.Warning("Password verification failed | UserId: {UserId} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                        user.Id, traceId, correlationId);
                    throw UserException.BadRequestException(UserErrorMessage.PasswordIncorrect);
                }

                Log.Debug("Password verified successfully | UserId: {UserId} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    user.Id, traceId, correlationId);

                var token = _tokenService.GenerateToken(user);
                Log.Debug("Token generated | UserId: {UserId} | TokenLength: {TokenLength} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    user.Id, token.Length, traceId, correlationId);

                _cookieService.Set(token);
                Log.Debug("Token set in cookie | UserId: {UserId} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    user.Id, traceId, correlationId);

                var response = _mapper.Map<UserSignInResponseDto>(user);
                response.Token = token;

                Log.Information("SignIn completed successfully | UserId: {UserId} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    user.Id, traceId, correlationId);

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SignIn failed | UserName: {UserName} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    request.UserName, traceId, correlationId);
                throw;
            }
        }

        public async Task<UserSignUpResponseDto> SignUp(UserSignUpRequestDto request, CancellationToken token)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
            var correlationId = _correlationContextAccessor.CorrelationContext?.CorrelationId ?? traceId;

            Log.Information("SignUp started | UserName: {UserName} | Email: {Email} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                request.UserName, request.Email, traceId, correlationId);

            try
            {
                if (await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName))
                {
                    Log.Warning("Username already exists | UserName: {UserName} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                        request.UserName, traceId, correlationId);
                    throw UserException.UserAlreadyExistsException(request.UserName);
                }

                if (await _unitOfWork.UserRepository.AnyAsync(x => x.Email == request.Email))
                {
                    Log.Warning("Email already exists | Email: {Email} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                        request.Email, traceId, correlationId);
                    throw UserException.UserAlreadyExistsException(request.Email);
                }

                var user = _mapper.Map<User>(request);
                user.Password = user.Password.Hash();
                await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.UserRepository.AddAsync(user), token);

                var response = _mapper.Map<UserSignUpResponseDto>(user);
                Log.Information("SignUp completed successfully | UserId: {UserId} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    user.Id, traceId, correlationId);

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SignUp failed | UserName: {UserName} | Email: {Email} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    request.UserName, request.Email, traceId, correlationId);
                throw;
            }
        }

        public void Logout()
        {
            try
            {
                _cookieService.Delete();
                Log.Information("User logged out successfully | UserId: {UserId}", _currentUser.GetCurrentUserId());
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Logout failed | UserId: {UserId}", _currentUser.GetCurrentUserId());
            }
        }

        public async Task<UserProfileResponseDto> GetProfile()
        {
            Log.Debug("GetProfile called");
            var userId = _currentUser.GetCurrentUserId();
            Log.Debug("Current UserId from claims: {UserId}", userId);
            var sw = Stopwatch.StartNew();
            var user = await _userRepository.GetByIdAsync(userId);
            sw.Stop();
            Log.Debug("User fetch took {ElapsedMilliseconds} ms", sw.ElapsedMilliseconds);

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