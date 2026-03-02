using TodoList.Models;

namespace TodoList.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
