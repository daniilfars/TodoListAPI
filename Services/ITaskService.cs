using TodoList.Models;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;

namespace TodoList.Services;

public interface ITaskService
{
    Task<IEnumerable<ResponseTaskDto>> GetAllTasksAsync();
    Task<ResponseTaskDto?> GetTaskByIdAsync(int id);
    Task<ResponseTaskDto> CreateTaskAsync(CreateTaskDto createTask);
    Task<ResponseTaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTask);
    Task<bool> DeleteTaskAsync(int id);
}
