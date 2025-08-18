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
        public GetProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetCurrentUserId();
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            Log.Debug("Profile retrieved | UserId: {UserId}", userId);
            return _mapper.Map<ProfileResponse>(user);
        }
    }
}
