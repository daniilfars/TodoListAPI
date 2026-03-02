using System.ComponentModel.DataAnnotations;

namespace TodoList.Models.DTOs.UpdateDTOs;

public class UpdateTagDto
{
    [StringLength(50)]
    public string? Name { get; set; }
}
