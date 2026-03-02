using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.DTOs.CreateDTOs;

public class CreateTagDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
}
