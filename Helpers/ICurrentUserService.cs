using ChatApplication.Models;

namespace ChatApplication.Helpers
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        Task<AppUser> GetUser();
    }
}
