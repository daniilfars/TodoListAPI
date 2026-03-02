using TodoList.Models;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;

namespace TodoList.Services;

public interface IProjectService
{
    Task<IEnumerable<ResponseProjectDto>> GetAllProjectsAsync();
    Task<ResponseProjectDto?> GetProjectByIdAsync(int id);
    Task<ResponseProjectDto> CreateProjectAsync(CreateProjectDto createProject);
    Task<ResponseProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto updateProject);
    Task<bool> DeleteProjectAsync(int id);
    Task<IEnumerable<ResponseProjectDto>> GetProjectsByUserIdAsync(int userId);
}
