using TodoList.Models;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;

namespace TodoList.Services;

public interface ITagService
{
    Task<IEnumerable<ResponseTagDto>> GetAllTagsAsync();
    Task<ResponseTagDto?> GetTagByIdAsync(int id);
    Task<ResponseTagDto> CreateTagAsync(CreateTagDto createTag);
    Task<ResponseTagDto?> UpdateTagAsync(int id, UpdateTagDto updateTag);
    Task<bool> DeleteTagAsync(int id);
    Task<List<ResponseTaskDto>> GetTasksByTagAsync(int tagId);
}
