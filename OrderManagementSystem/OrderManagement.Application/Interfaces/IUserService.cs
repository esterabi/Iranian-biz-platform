using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserDto userDto);
    Task<UserDto?> LoginAsync(string email, string password);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(Guid userId);
}