using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Persistence.Context;

namespace OrderManagement.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> RegisterAsync(UserDto userDto)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            Role = userDto.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        userDto.Id = user.Id;
        userDto.Password = string.Empty;
        return userDto;
    }

    public async Task<UserDto?> LoginAsync(string userName, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = user.Role,
            Password = string.Empty
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Role = u.Role
            }).ToListAsync();
    }

    public async Task<UserDto?> GetByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user != null)
        {
            return new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role
            };
        }

        return null;
    }
}