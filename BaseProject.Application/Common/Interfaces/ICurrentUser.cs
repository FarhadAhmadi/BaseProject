using BaseProject.Domain.Entities.Auth;

namespace BaseProject.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        public string GetCurrentUserId();
        Task<ApplicationUser> GetCurrentUser();
    }
}
