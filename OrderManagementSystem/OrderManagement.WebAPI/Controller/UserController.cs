using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// ثبت‌نام کاربر جدید
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.UserName) || string.IsNullOrWhiteSpace(userDto.Password))
            return BadRequest("ایمیل و رمز عبور الزامی هستند.");

        var result = await _userService.RegisterAsync(userDto);
        return Ok(result);
    }

    /// <summary>
    /// ورود کاربر
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        if (string.IsNullOrWhiteSpace(userDto.UserName) || string.IsNullOrWhiteSpace(userDto.Password))
            return BadRequest("ایمیل و رمز عبور الزامی هستند.");

        var result = await _userService.LoginAsync(userDto.UserName, userDto.Password);
        if (result == null)
            return Unauthorized("اطلاعات ورود نامعتبر است.");

        return Ok(result);
    }

    /// <summary>
    /// دریافت لیست همه کاربران
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }
    
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var users = await _userService.GetByIdAsync(id);
        return Ok(users);
    }
}