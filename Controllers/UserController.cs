using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using TodoList.Services;

namespace TodoList.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService _userService)
    {
        userService = _userService;
    }

    // GET: api/user
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseUserDto>>> GetAll()
    {
        var items = await userService.GetAllUsersAsync();
        return Ok(items);
    }

    // GET: api/user/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseUserDto>> GetById(int id)
    {
        var item = await userService.GetUserByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    // POST: api/user
    [HttpPost]
    public async Task<ActionResult<ResponseUserDto>> Create(CreateUserDto createDto)
    {
        try
        {
            var newItem = await userService.CreateUserAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/user/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto updateDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            return Unauthorized();

        if (userRoleClaim != "Admin" || currentUserId != id)
            return Forbid("Нельзя редактировать других пользователей");

        try
        {
            var updatedItem = await userService.UpdateUserAsync(id, updateDto);
            if (updatedItem == null)
                return NotFound();
            return Ok(updatedItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/user/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            return Unauthorized();

        if (userRoleClaim != "Admin" || currentUserId != id)
            return Forbid("Нельзя удалять других пользователей");

        var result = await userService.DeleteUserAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}