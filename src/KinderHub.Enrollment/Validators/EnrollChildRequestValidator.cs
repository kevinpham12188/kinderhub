using FluentValidation;
using KinderHub.Enrollment.DTOs.Requests;

namespace KinderHub.Enrollment.Validators
{
    public class EnrollChildRequestValidator : AbstractValidator<EnrollChildRequestDto>
    {
        public EnrollChildRequestValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(dob => dob < DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be in the past")
            .Must(dob =>
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var ageInMonths = ((today.Year - dob.Year) * 12) + today.Month - dob.Month;
                return ageInMonths < 60; // Must be under 5 years old (60 months)
            }).WithMessage("Child must be under 5 years old");

            RuleFor(x => x.ClassroomId)
            .NotEmpty().WithMessage("Classroom ID is required");

            RuleFor(x => x.ParentId)
            .NotEmpty().WithMessage("Parent ID is required");
        }
    }
}