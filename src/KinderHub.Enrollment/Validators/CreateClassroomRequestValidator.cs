using FluentValidation;
using KinderHub.Enrollment.DTOs.Requests;

namespace KinderHub.Enrollment.Validators
{
    public class CreateClassroomRequestValidator : AbstractValidator<CreateClassroomRequestDto>
    {
        public CreateClassroomRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Classroom name is required")
            .MinimumLength(3).WithMessage("Classroom name must be at least 3 characters long")
            .MaximumLength(100).WithMessage("Classroom name must not exceed 100 characters");

            RuleFor(x => x.AgeGroup)
            .IsInEnum().WithMessage("Invalid age group. Valid values are: Infant, Toddler, Twaddler, Preschool");
        }
    }
}