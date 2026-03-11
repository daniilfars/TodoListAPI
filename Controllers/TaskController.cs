using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using TodoList.Models.RequestFeatures;
using TodoList.Services;

namespace TodoList.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService taskService;

    public TaskController(ITaskService _taskService)
    {
        taskService = _taskService;
    }

    /*// GET: api/task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseTaskDto>>> GetAll()
    {
        var items = await taskService.GetAllTasksAsync();
        return Ok(items);
    }*/

    // GET: api/task/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseTaskDto>> GetById(int id)
    {
        var item = await taskService.GetTaskByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    // POST: api/task
    [HttpPost]
    public async Task<ActionResult<ResponseTaskDto>> Create(CreateTaskDto createDto)
    {
        try
        {
            var newItem = await taskService.CreateTaskAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/task/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTaskDto updateDto)
    {
        try
        {
            var updatedItem = await taskService.UpdateTaskAsync(id, updateDto);
            if (updatedItem == null)
                return NotFound();
            return Ok(updatedItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/task/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await taskService.DeleteTaskAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    // GET: api/task
    [HttpGet]
    public async Task<ActionResult<ResponsePaged<ResponseTaskDto>>> GetTasks([FromQuery] TaskQueryParameters parameters)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized(new { message = "User ID not found in token" });

        var result = await taskService.GetTasksAsync(parameters, userId);
        return Ok(result);
    }
}