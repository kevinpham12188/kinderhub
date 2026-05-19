using KinderHub.Identity.DTOs;
using KinderHub.Identity.DTOs.Requests;
using KinderHub.Identity.DTOs.Responses;
using KinderHub.Identity.Exceptions;
using KinderHub.Identity.Models;
using KinderHub.Identity.Repositories.Interfaces;
using KinderHub.Identity.Services.Interfaces;


namespace KinderHub.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        public AuthService(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _authRepository.FindByEmailAsync(request.Email);
            if(user == null)
            {
                throw new UnauthorizedException("Invalid email or password");
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid email or password");
            }
            var tokenResult = _tokenService.GenerateToken(user);
            return new LoginResponseDto
            {
                Token = tokenResult.Token,
                Email = user.Email,
                Role = user.Role,
                Expiration = tokenResult.ExpiresAt
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if(await _authRepository.EmailExistsAsync(request.Email))
            {
                throw new ConflictException("A user with this email already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user  = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = hashedPassword,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role
            };
            await _authRepository.CreateAsync(user);
            return new RegisterResponseDto { Id = user.Id, Email = user.Email, Role = user.Role };
        }
    }
}