using KinderHub.Identity.Models.Enums;

namespace KinderHub.Identity.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role{ get; set; }
        public bool IsActive { get; set;} = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ParentProfile? ParentProfile{ get; set; }
        public TeacherProfile? TeacherProfile{ get; set; }
    }

    
}