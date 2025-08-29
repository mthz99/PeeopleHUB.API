using PeopleHub.Domain.Entities;

namespace PeopleHub.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
    }
}