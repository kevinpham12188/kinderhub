using KinderHub.Enrollment.Models.Enums;

namespace KinderHub.Enrollment.DTOs.Responses
{
    public class ClassroomResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AgeGroup { get; set; } = string.Empty;
        public int MaxCapacityPerTeacher { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}