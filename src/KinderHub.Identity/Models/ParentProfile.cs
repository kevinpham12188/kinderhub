using KinderHub.Identity.Models;

namespace KinderHub.Identity.Models
{
    public class ParentProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string HomeAddress { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set;} = DateTimeOffset.UtcNow;
        public User User { get; set; } = null!;
    }
}