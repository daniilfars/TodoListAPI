using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using TodoList.Models;
using TodoList.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext db;

    public TaskService(AppDbContext context)
    {
        db = context;
    }
    public async Task<ResponseTaskDto> CreateTaskAsync(CreateTaskDto createTask)
    {
        var projectExists = await db.Projects.AnyAsync(p => p.Id == createTask.ProjectId);
        if (!projectExists)
            throw new InvalidOperationException("Проект с указанным id не существует.");

        TaskItem task = new TaskItem
        {
            Title = createTask.Title,
            Description = createTask.Description,
            CreatedAt = DateTime.UtcNow,
            ProjectId = createTask.ProjectId,
            IsCompleted = false
        };

        if(createTask.TagIds != null && createTask.TagIds.Any())
        {
            List<Tag> tags = await db.Tags.Where(t => createTask.TagIds.Contains(t.Id)).ToListAsync();

            if (tags.Count != createTask.TagIds.Count)
                throw new InvalidOperationException("Один или несколько тегов не найдены.");

            task.Tags = tags;
        }

        await db.Tasks.AddAsync(task);
        await db.SaveChangesAsync();

        List<ResponseTagDto> tagDtos = new List<ResponseTagDto>();

        if (task.Tags != null && task.Tags.Any())
        {
            tagDtos = task.Tags.Select(t => new ResponseTagDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();
        }

        return new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            ProjectId = task.ProjectId,
            Tags = tagDtos
        };
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        TaskItem? task = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return false;

        db.Remove(task);
        await db.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ResponseTaskDto>> GetAllTasksAsync()
    {
        return await db.Tasks.AsNoTracking().Include(t => t.Tags).OrderByDescending(t => t.CreatedAt).Select(task => new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            ProjectId = task.ProjectId,
            Tags = task.Tags.Select(t => new ResponseTagDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
        }).ToListAsync();
    }

    public async Task<ResponseTaskDto?> GetTaskByIdAsync(int id)
    {
        TaskItem? task = await db.Tasks.AsNoTracking().Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return null;

        return new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            ProjectId = task.ProjectId,
            Tags = task.Tags.Select(t => new ResponseTagDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList()
        };
    }

    public async Task<ResponseTaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTask)
    {
        TaskItem? task = await db.Tasks
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            return null;

        if (!string.IsNullOrWhiteSpace(updateTask.Title))
            task.Title = updateTask.Title;

        if (!string.IsNullOrWhiteSpace(updateTask.Description))
            task.Description = updateTask.Description;

        if (updateTask.IsCompleted.HasValue)
            task.IsCompleted = updateTask.IsCompleted.Value;

        if (updateTask.ProjectId.HasValue)
        {
            var projectExists = await db.Projects.AnyAsync(p => p.Id == updateTask.ProjectId.Value);
            if (!projectExists)
                throw new InvalidOperationException("Проект с указанным ID не существует.");

            task.ProjectId = updateTask.ProjectId.Value;
        }

        if (updateTask.TagIds != null)
        {
            if (!updateTask.TagIds.Any())
            {
                task.Tags.Clear();
            }
            else
            {
                var newTags = await db.Tags
                    .Where(t => updateTask.TagIds.Contains(t.Id))
                    .ToListAsync();

                if (newTags.Count != updateTask.TagIds.Count)
                    throw new InvalidOperationException("Один или несколько тегов не найдены.");

                task.Tags.Clear();
                foreach (var tag in newTags)
                {
                    task.Tags.Add(tag);
                }
            }
        }

        await db.SaveChangesAsync();

        return new ResponseTaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            CreatedAt = task.CreatedAt,
            IsCompleted = task.IsCompleted,
            ProjectId = task.ProjectId,
            Tags = task.Tags.Select(t => new ResponseTagDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList()
        };
    }
}