using KinderHub.Identity.DTOs.Responses;
using KinderHub.Identity.Models;

namespace KinderHub.Identity.Services.Interfaces
{
    public interface ITokenService
    {
         TokenResult GenerateToken(User user);
    }
}