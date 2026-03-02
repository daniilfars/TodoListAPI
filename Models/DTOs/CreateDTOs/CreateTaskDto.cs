using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.DTOs.CreateDTOs;

public class CreateTaskDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int ProjectId { get; set; }

    public List<int>? TagIds { get; set; }
}
