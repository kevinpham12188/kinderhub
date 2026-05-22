using KinderHub.Enrollment.Models.Enums;

namespace KinderHub.Enrollment.Models
{
    public class Waitlist
    {
        public Guid Id {get; set;}
        public Guid ChildId {get; set;}
        public AgeGroup AgeGroup {get; set;}
        public DateOnly RequestedDate {get; set;}
        public WaitlistStatus Status {get; set;}
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Child Child {get; set;} = null!;
    }
}