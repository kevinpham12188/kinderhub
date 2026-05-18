using KinderHub.Identity.DTOs;
using KinderHub.Identity.DTOs.Responses;

namespace KinderHub.Identity.Services.Interfaces
{
    public interface IAuthService
    {
         Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    }
}