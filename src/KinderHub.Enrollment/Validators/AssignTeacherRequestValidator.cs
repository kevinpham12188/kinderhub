using FluentValidation;
using KinderHub.Enrollment.DTOs.Requests;

namespace KinderHub.Enrollment.Validators
{
    public class AssignTeacherRequestValidator : AbstractValidator<AssignTeacherRequestDto>
    {
        public AssignTeacherRequestValidator()
        {
           RuleFor(x => x.TeacherId)
           .NotEmpty().WithMessage("Teacher ID is required");
        }
    }
}