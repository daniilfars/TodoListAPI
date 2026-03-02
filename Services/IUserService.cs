using TodoList.Models;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;

namespace TodoList.Services;

public interface IUserService
{
    Task<IEnumerable<ResponseUserDto>> GetAllUsersAsync();
    Task<ResponseUserDto?> GetUserByIdAsync(int id);
    Task<ResponseUserDto> CreateUserAsync(CreateUserDto createUser);
    Task<ResponseUserDto?> UpdateUserAsync(int id, UpdateUserDto updateUser);
    Task<bool> DeleteUserAsync(int id);
}
