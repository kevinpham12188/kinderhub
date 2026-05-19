using KinderHub.Identity.DTOs.Requests;
using KinderHub.Identity.DTOs.Responses;

namespace KinderHub.Identity.Services.Interfaces
{
    public interface IAuthService
    {
         Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
         Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}