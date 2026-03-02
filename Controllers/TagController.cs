using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoList.Services;
using TodoList.Models.DTOs.ResponseDTOs;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.UpdateDTOs;

namespace TodoList.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TagController : ControllerBase
{
    private readonly ITagService tagService;

    public TagController(ITagService _tagService)
    {
        tagService = _tagService;
    }

    // GET: api/tag
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseTagDto>>> GetAll()
    {
        var items = await tagService.GetAllTagsAsync();
        return Ok(items);
    }

    // GET: api/tag/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseTagDto>> GetById(int id)
    {
        var item = await tagService.GetTagByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    // POST: api/tag
    [HttpPost]
    public async Task<ActionResult<ResponseTagDto>> Create(CreateTagDto createDto)
    {
        try
        {
            var newItem = await tagService.CreateTagAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/tag/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTagDto updateDto)
    {
        try
        {
            var updatedItem = await tagService.UpdateTagAsync(id, updateDto);
            if (updatedItem == null)
                return NotFound();
            return Ok(updatedItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/tag/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await tagService.DeleteTagAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
    // GET: api/tag/5/tasks
    [HttpGet("{tagId}/tasks")]
    public async Task<ActionResult<List<ResponseTaskDto>>> GetTasksByTag(int tagId)
    {
        var tasks = await tagService.GetTasksByTagAsync(tagId);
        return Ok(tasks);
    }
}