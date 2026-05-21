using FluentValidation.TestHelper;
using KinderHub.Identity.DTOs.Requests;
using KinderHub.Identity.Models.Enums;
using KinderHub.Identity.Validators;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator;
    public RegisterRequestValidatorTests()
    {
        _validator = new RegisterRequestValidator();
    }

    #region FirstName Tests
    [Fact]
    public void FirstName_WhenValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "Anna",
            LastName = "Smith",
            Email = "annasmith@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void FirstName_WhenLongerThan100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = new string('A', 101),
            LastName = "Smith",
            Email = "annasmith@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name cannot exceed 100 characters");
    }

    [Fact]
    public void FirstName_WhenContainsSpecialCharacters_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John@",
            LastName = "Doe",
            Email = "johndoe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123", 
            Role = UserRole.Parent
        };
        
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name cannot contain special characters");
    }

    [Fact]
    public void FirstName_WhenContainsHyphenOrApostrophe_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "O'Connor-Smith",
            LastName = "Doe",
            Email = "johndoe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
    }
    #endregion

    #region LastName Tests
    [Fact]
    public void LastName_WhenValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "Anna",
            LastName = "Smith",
            Email = "annasmith@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void LastName_WhenLongerThan100Characters_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "Anna",
            LastName = new string('S', 101),
            Email = "annasmith@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name cannot exceed 100 characters");
    }

    [Fact]
    public void LastName_WhenContainsSpecialCharacters_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe@",
            Email = "johndoe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName)
            .WithErrorMessage("Last name cannot contain special characters");
    }
    #endregion

    #region Email Tests
    [Fact]
    public void Email_WhenEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Fact]
    public void Email_WhenInvalidFormat_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email-format",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format");
    }

    [Fact]
    public void Email_WhenLongerThan255Characters_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = new string('a', 256) + "@example.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email cannot exceed 255 characters");
    }

    [Fact]
    public void Email_WhenValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
    #endregion

    #region PhoneNumber Tests
    [Fact]
    public void PhoneNumber_WhenEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Phone number is required");
    }

    [Fact]
    public void PhoneNumber_WhenInvalidFormat_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "invalid-phone-number",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Phone number only contain digits/spaces/hyphens/parentheses/plus sign");
    }

    [Fact]
    public void PhoneNumber_WhenValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }
    #endregion

    #region Password Tests
    [Fact]
    public void Password_WhenEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required");
    }

    [Fact]
    public void Password_WhenLessThan8Characters_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "1234567",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters");
    }

    [Fact]
    public void Password_WhenMissingUppercase_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "securepassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain an uppercase letter");
    }

    [Fact]
    public void Password_WhenMissingSpecialCharacter_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one special character");
    }

    [Fact]
    public void Password_WhenValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123!",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    #endregion

    #region Role Tests
    [Fact]
    public void Role_WhenInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123!",
            Role = (UserRole)999 // Invalid role
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage("Role must be admin, teacher, or parent");
    }

    [Fact]
    public void Role_WhenValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123!",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }
    #endregion

    #region Overall Tests
    [Fact]
    public void AllFields_WhenValid_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123!",
            Role = UserRole.Parent
        };
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    #endregion
}