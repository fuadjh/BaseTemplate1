using Infrastructure.Identity;

namespace Infrastructure.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
