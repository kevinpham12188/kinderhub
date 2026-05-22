using KinderHub.Enrollment.Models.Enums;

namespace KinderHub.Enrollment.Helpers
{
    public class EnrollmentRules
    {
        public static AgeGroup CalculateAgeGroup(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTimeOffset.UtcNow.DateTime);
            var ageInMonths = ((today.Year - dateOfBirth.Year) * 12) + today.Month - dateOfBirth.Month;

            return ageInMonths switch
            {
                < 18 => AgeGroup.Infant,
                < 24 => AgeGroup.Toddler,
                < 36 => AgeGroup.Twaddler,
                _ => AgeGroup.Preschool     
            };
        }

        public static int CalculateMaxCapacity(int teacherCount, AgeGroup ageGroup)
        {
            var ratio = ageGroup switch
            {
                AgeGroup.Infant => 4,
                AgeGroup.Toddler => 6,
                AgeGroup.Twaddler => 6,
                AgeGroup.Preschool => 12,
                _ => throw new ArgumentException($"Unknown age group: {ageGroup}")
            };

            return teacherCount * ratio;
        }
    }
}