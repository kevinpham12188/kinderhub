using FluentValidation;
using KinderHub.Identity.DTOs;
using KinderHub.Identity.DTOs.Requests;

namespace KinderHub.Identity.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("First name cannot contain special characters")
                .MinimumLength(3).WithMessage("First name must be at least 3 characters")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Last name cannot contain special characters")
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");   

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^[\d\s\-()+]+$").WithMessage("Phone number only contain digits/spaces/hyphens/parentheses/plus sign");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                .Matches(@"[!@#$%^&*]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Role must be admin, teacher, or parent");
        }
    }
}