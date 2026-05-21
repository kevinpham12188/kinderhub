using BCrypt.Net;
using KinderHub.Identity.DTOs.Requests;
using KinderHub.Identity.DTOs.Responses;
using KinderHub.Identity.Exceptions;
using KinderHub.Identity.Models;
using KinderHub.Identity.Models.Enums;
using KinderHub.Identity.Repositories.Interfaces;
using KinderHub.Identity.Services;
using KinderHub.Identity.Services.Interfaces;
using Moq;
using Shouldly;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _mockRepo;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockRepo = new Mock<IAuthRepository>();
        _mockTokenService = new Mock<ITokenService>();
        _authService = new AuthService(_mockRepo.Object, _mockTokenService.Object);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsConflictException()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane2@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Teacher
        };

        _mockRepo.Setup(r => r.EmailExistsAsync(request.Email)).ReturnsAsync(true);
       
        // Act
        var exception = await Should.ThrowAsync<ConflictException>(
            async () => await _authService.RegisterAsync(request)
        );

        // Assert
        exception.Message.ShouldBe("A user with this email already exists");
        
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailDoesNotExist_ReturnsRegisterResponseDto()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FirstName = "Kevin",
            LastName = "Pham",
            Email = "kevinpham@kinderhub.com",
            PhoneNumber = "555-555-5555",
            Password = "SecurePassword123",
            Role = UserRole.Admin
        };

        _mockRepo.Setup(r => r.EmailExistsAsync(request.Email)).ReturnsAsync(false);
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);

        // Act
        var response = await _authService.RegisterAsync(request);

        // Assert
        response.Email.ShouldBe(request.Email);
        response.Role.ShouldBe(request.Role);
        response.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "kevinpham121@kinderhub.com",
            Password = "WrongPassword"
        };

        _mockRepo.Setup(r => r.FindByEmailAsync(request.Email)).ReturnsAsync((User?)null);
        // Act
        var exception = await Should.ThrowAsync<UnauthorizedException>(
            async () => await _authService.LoginAsync(request)
        );

        //Assert
        exception.Message.ShouldBe("Invalid email or password");
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsWrong_ThrowsUnauthorizedException()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "kevinpham121@kinderhub.com",
            Password = "WrongPassword"
        };

        _mockRepo.Setup(r => r.FindByEmailAsync(request.Email)).ReturnsAsync(new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
            Role = UserRole.Admin
        });
        // Act
        var exception = await Should.ThrowAsync<UnauthorizedException>(
            async () => await _authService.LoginAsync(request)
        );
        //Assert
        exception.Message.ShouldBe("Invalid email or password");
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsLoginResponseDto()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "jane2@kinderhub.com",
            Password = "SecurePass123!"
        };

        _mockRepo.Setup(r => r.FindByEmailAsync(request.Email)).ReturnsAsync(new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Teacher
        });

        _mockTokenService.Setup(t => t.GenerateToken(It.IsAny<User>()))
            .Returns(new TokenResult
            {
                Token = "fake-jwt-token",
                ExpiresAt = DateTimeOffset.UtcNow.AddHours(1)
            });


        // Act
        var result = await _authService.LoginAsync(request);

        //Assert
        result.Email.ShouldBe(request.Email);
        result.Role.ShouldBe(UserRole.Teacher);
        result.Token.ShouldBe("fake-jwt-token");
        result.Expiration.ShouldBeInRange(DateTimeOffset.UtcNow.AddMinutes(59), DateTimeOffset.UtcNow.AddMinutes(61));
    }   
}