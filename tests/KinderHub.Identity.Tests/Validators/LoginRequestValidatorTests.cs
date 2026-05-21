using KinderHub.Identity.DTOs.Requests;
using KinderHub.Identity.Validators;
using FluentValidation.TestHelper;


public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator;
    public LoginRequestValidatorTests()
    {
        _validator = new LoginRequestValidator();
    }

    #region Email Tests
    [Fact]
    public void Email_WhenEmpty_ShouldHaveValidatorError()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "",
            Password = "ValidPassword123!"
        };

        // Act 
        var result = _validator.TestValidate(request);        
        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Email)
            .WithErrorMessage("Email is required");
    }

    [Fact]
    public void Email_WhenInvalidFormat_ShouldHaveValidatorError()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "invalid-email",
            Password = "ValidPassword123!"
        };

        // Act 
        var result = _validator.TestValidate(request);  

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Email)
            .WithErrorMessage("Invalid email format");
    }

    [Fact]
    public void Email_WhenValid_ShouldNotHaveValidatorError()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "valid@example.com", 
            Password = "ValidPassword123!"
        };

        // Act 
        var result = _validator.TestValidate(request);  

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Email);
    }
    #endregion

    #region Password Tests
    [Fact]
    public void Password_WhenEmpty_ShouldHaveValidatorError()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "valid@example.com",
            Password = ""
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Password)
            .WithErrorMessage("Password is required");
    }

    [Fact]
    public void Password_WhenTooShort_ShouldHaveValidatorError()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "valid@example.com",
            Password = "short"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.Password).WithErrorMessage("Password must be at least 8 characters long");
    }
    
    [Fact]
    public void Password_WhenValid_ShouldNotHaveValidatorError()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "valid@example.com",
            Password = "ValidPassword123!"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(r => r.Password);
    }
    #endregion

    #region All Valid Tests
    [Fact]
    public void WhenAllFieldsAreValid_ShouldNotHaveAnyValidatorErrors()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "valid@example.com",
            Password = "ValidPassword123!"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    #endregion
}