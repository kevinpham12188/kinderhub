using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KinderHub.Identity.Configuration;
using KinderHub.Identity.Models;
using KinderHub.Identity.Models.Enums;
using KinderHub.Identity.Services;
using Microsoft.Extensions.Options;
using Shouldly;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly User _testUser;

    public TokenServiceTests()
    {
        var jwtSettings = Options.Create(new JwtSettings
        {
            SecretKey = "test-secret-key-that-is-long-enough-32chars",
            ExpirationMinutes = 60,
            Issuer = "KinderHub.Identity",
            Audience = "KinderHub.Identity"
        });
        _tokenService = new TokenService(jwtSettings);
        _testUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "jane@kinderhub.com",
            Role = UserRole.Teacher
        };
    }

    [Fact]
    public void GenerateToken_ReturnsNonEmptyToken()
    {
        //Arrange

        //Act
        var result = _tokenService.GenerateToken(_testUser);
        //Assert
        result.Token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateToken_ExpiresAtIsApproximatelyOneHourFromNow()
    {
        //Arrange

        //Act
        var result = _tokenService.GenerateToken(_testUser);    
        //Assert
        result.ExpiresAt.ShouldBeInRange(DateTimeOffset.UtcNow.AddMinutes(59), DateTimeOffset.UtcNow.AddMinutes(61));
    }

    [Fact]
    public void GenerateToken_TokenContainsCorrectClaims()
    {
        //Arrange

        //Act
        var result = _tokenService.GenerateToken(_testUser);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(result.Token);

        
        //Assert
        jwtToken.Claims.First(c => c.Type == "email").Value
        .ShouldBe(_testUser.Email);

        jwtToken.Claims.First(c => c.Type == "role").Value
        .ShouldBe(_testUser.Role.ToString());

    }

}