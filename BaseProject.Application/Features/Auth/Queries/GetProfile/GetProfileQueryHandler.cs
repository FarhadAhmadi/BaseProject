using AutoMapper;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Interfaces;
using MediatR;

namespace BaseProject.Application.Features.Auth.Queries.GetProfile
{
    public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileResponse>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppLogger _appLogger;
        public GetProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser, IAppLogger appLogger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _appLogger = appLogger;
        }
        public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetCurrentUserId();
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            _appLogger.Debug("Profile retrieved | UserId: {UserId}", userId);
            return _mapper.Map<ProfileResponse>(user);
        }
    }
}
