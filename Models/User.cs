namespace TodoList.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; 
    public DateTime CreatedAt { get; set; }

    public List<Project>? Projects { get; set; } = new();
}
