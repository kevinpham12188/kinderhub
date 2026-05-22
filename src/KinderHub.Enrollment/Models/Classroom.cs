using KinderHub.Enrollment.Models.Enums;

namespace KinderHub.Enrollment.Models
{
    public class Classroom
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AgeGroup AgeGroup { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<Child> Children { get; set; } = new List<Child>();
        public ICollection<ClassroomTeacher> ClassroomTeachers { get; set; } = new List<ClassroomTeacher>();
    }
}