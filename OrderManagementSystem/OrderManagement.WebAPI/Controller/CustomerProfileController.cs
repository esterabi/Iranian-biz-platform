using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class CustomerProfileController : ControllerBase
{
    private readonly ICustomerProfileService _profileService;

    public CustomerProfileController(ICustomerProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>
    /// ایجاد پروفایل مشتری
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerProfileDto profileDto)
    {
        if (string.IsNullOrWhiteSpace(profileDto.FirstName) ||
            string.IsNullOrWhiteSpace(profileDto.LastName) ||
            string.IsNullOrWhiteSpace(profileDto.Mobile))
        {
            return BadRequest("نام، نام خانوادگی و شماره موبایل الزامی هستند.");
        }

        var createdProfile = await _profileService.CreateAsync(profileDto);
        return Ok(createdProfile);
    }

    /// <summary>
    /// دریافت پروفایل مشتری بر اساس شناسه کاربر
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var profile = await _profileService.GetByUserIdAsync(userId);
        if (profile == null)
            return NotFound($"پروفایلی برای کاربر با شناسه {userId} یافت نشد.");

        return Ok(profile);
    }
}