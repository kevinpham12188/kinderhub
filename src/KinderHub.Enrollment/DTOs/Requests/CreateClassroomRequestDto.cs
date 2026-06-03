using KinderHub.Enrollment.Models.Enums;

namespace KinderHub.Enrollment.DTOs.Requests
{
    public class CreateClassroomRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public AgeGroup AgeGroup { get; set; }
    }
}