namespace KinderHub.Enrollment.DTOs.Responses
{
    public class AgeMismatchResponseDto
    {
        public Guid ChildId { get; set; }
        public string ChildName { get; set;} = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public int AgeInMonths { get; set; }
        public string CurrentAgeGroup { get; set; } = string.Empty;
        public string RecommendedAgeGroup { get; set; } = string.Empty;
        public Guid CurrentClassroomId { get; set; }
        public string CurrentClassroomName { get; set; } = string.Empty;

    }
}