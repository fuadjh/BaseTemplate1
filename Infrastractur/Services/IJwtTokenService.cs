using Infrastructure.IdentityModels;

namespace Infrastructure.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
