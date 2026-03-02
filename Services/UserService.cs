using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using TodoList.Data;
using TodoList.Models.DTOs.CreateDTOs;
using TodoList.Models.DTOs.ResponseDTOs;
using TodoList.Models.DTOs.UpdateDTOs;
using BCrypt.Net;

namespace TodoList.Services;

public class UserService : IUserService
{
    readonly AppDbContext db;

    public UserService(AppDbContext context)
    {
        db = context;
    }
    public async Task<ResponseUserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        User? existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == createUserDto.Email);

        if (existingUser != null)
            throw new InvalidOperationException("Пользователь с таким email уже существует.");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        User user = new User
        {
            Name = createUserDto.UserName,
            Email = createUserDto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return new ResponseUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return false;

        db.Users.Remove(user);
        await db.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ResponseUserDto>> GetAllUsersAsync()
    {
        return await db.Users.AsNoTracking().Select(u => new ResponseUserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CreatedAt = u.CreatedAt
        }).ToListAsync();
    }

    public async Task<ResponseUserDto?> GetUserByIdAsync(int id)
    {
        User? user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return null;

        return new ResponseUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<ResponseUserDto?> UpdateUserAsync(int id, UpdateUserDto updateUser)
    {
        User? user = await db.Users.FindAsync(id);

        if (user == null)
            return null;

        if (!string.IsNullOrWhiteSpace(updateUser.UserName))
            user.Name = updateUser.UserName;

        if (!string.IsNullOrWhiteSpace(updateUser.Email))
            user.Email = updateUser.Email;

        if (!string.IsNullOrWhiteSpace(updateUser.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUser.Password);

        await db.SaveChangesAsync();

        return new ResponseUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}