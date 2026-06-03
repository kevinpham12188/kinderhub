using KinderHub.Enrollment.Models.Enums;

namespace KinderHub.Enrollment.DTOs.Responses
{
    public class ClassroomDetailResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AgeGroup { get; set; } = string.Empty;
        public int TeacherCount { get; set; }
        public int CurrentEnrollment { get; set; }
        public int MaxCapacity { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}