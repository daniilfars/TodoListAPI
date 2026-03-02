using TodoList.Models.DTOs.ResponseDTOs;

namespace TodoList.Models.DTOs.Jwt;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public ResponseUserDto? User { get; set; }
}
