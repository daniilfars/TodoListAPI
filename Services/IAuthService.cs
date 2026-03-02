using TodoList.Models.DTOs.Jwt;

namespace TodoList.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponse?> LoginAsync(LoginDto loginDto);
}