namespace TodoList.Models.DTOs.ResponseDTOs;

public class ResponseTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public int ProjectId { get; set; }
    public List<ResponseTagDto>? Tags { get; set; }
}
