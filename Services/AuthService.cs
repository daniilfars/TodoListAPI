using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;
using TodoList.Models.DTOs;
using TodoList.Models.DTOs.Jwt;
using TodoList.Models.DTOs.ResponseDTOs;

namespace TodoList.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto)
        {
            // Проверка на существующий email
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Пользователь с таким email уже существует.");

            // Хеширование пароля
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Name = registerDto.UserName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Генерация токена
            string token = _tokenService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                User = new ResponseUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        public async Task<AuthResponse?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                return null;

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!passwordValid)
                return null;

            string token = _tokenService.GenerateToken(user);

            return new AuthResponse
            {
                Token = token,
                User = new ResponseUserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                }
            };
        }
    }
}