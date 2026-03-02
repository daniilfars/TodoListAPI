using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.DTOs.UpdateDTOs;

public class UpdateTaskDto
{
    [StringLength(200)]
    public string? Title { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool? IsCompleted { get; set; }

    public int? ProjectId { get; set; }

    public List<int>? TagIds { get; set; }
}
