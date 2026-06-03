using FluentValidation;
using KinderHub.Enrollment.DTOs.Requests;

namespace KinderHub.Enrollment.Validators
{
    public class UpdateClassroomRequestValidator : AbstractValidator<UpdateClassroomRequestDto>
    {
        public UpdateClassroomRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Classroom name is required")
            .MinimumLength(3).WithMessage("Classroom name must be at least 3 characters long")
            .MaximumLength(100).WithMessage("Classroom name must not exceed 100 characters");
        }
    }
}