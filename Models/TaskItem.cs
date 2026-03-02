namespace TodoList.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted{ get; set; } 

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public List<Tag> Tags { get; set; } = new();
}
