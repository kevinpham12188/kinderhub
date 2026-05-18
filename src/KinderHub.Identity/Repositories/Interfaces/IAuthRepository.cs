using KinderHub.Identity.Models;

namespace KinderHub.Identity.Repositories.Interfaces
{
    public interface IAuthRepository
    {
         Task<User> CreateAsync(User user);
         Task<bool> EmailExistsAsync(string email);
         Task<User?> FindByEmailAsync(string email);
    }
}