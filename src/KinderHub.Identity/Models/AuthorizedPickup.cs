using KinderHub.Identity.Models;

namespace KinderHub.Identity.Models
{
    public class AuthorizedPickup
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ParentProfile ParentProfile { get; set; } = null!;
    }
}