namespace TodoList.Models.DTOs.ResponseDTOs;

public class ResponseUserDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
