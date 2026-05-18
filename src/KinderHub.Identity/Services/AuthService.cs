using KinderHub.Identity.DTOs;
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
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
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