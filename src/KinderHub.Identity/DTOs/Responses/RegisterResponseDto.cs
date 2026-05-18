using KinderHub.Identity.Models.Enums;

namespace KinderHub.Identity.DTOs.Responses
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}