using KinderHub.Identity.Models.Enums;

namespace KinderHub.Identity.DTOs.Responses
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}