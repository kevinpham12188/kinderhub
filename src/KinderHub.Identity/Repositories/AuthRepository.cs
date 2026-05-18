using KinderHub.Identity.Repositories.Interfaces;
using KinderHub.Identity.Models;
using Microsoft.EntityFrameworkCore;
using KinderHub.Identity.Data;

namespace KinderHub.Identity.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IdentityDbContext _context;
        
        public AuthRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());            
        }

        
    }
}