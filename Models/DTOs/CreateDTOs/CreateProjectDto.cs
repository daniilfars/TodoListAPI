using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.DTOs.CreateDTOs;

public class CreateProjectDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public int UserId { get; set; }
}
