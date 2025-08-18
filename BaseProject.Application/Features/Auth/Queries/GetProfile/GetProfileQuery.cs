using MediatR;

namespace BaseProject.Application.Features.Auth.Queries.GetProfile
{
    public sealed class GetProfileQuery : IRequest<ProfileResponse>
    {
    }
}
