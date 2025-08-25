using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Persistence.Context;

namespace OrderManagement.Application.Services;

public class CustomerProfileService : ICustomerProfileService
{
    private readonly AppDbContext _context;

    public CustomerProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerProfileDto> CreateAsync(CustomerProfileDto profileDto)
    {
        var profile = await _context.CustomerProfiles
            .FirstOrDefaultAsync(p => p.UserId == profileDto.UserId);

        if (profile == null)
        {
            profile = CreateNewProfile(profileDto);
            _context.CustomerProfiles.Add(profile);
        }
        else
        {
            UpdateExistingProfile(profile, profileDto);
            _context.CustomerProfiles.Update(profile);
        }

        await _context.SaveChangesAsync();
        profileDto.Id = profile.Id;
        return profileDto;
    }

    private CustomerProfile CreateNewProfile(CustomerProfileDto profileDto)
    {
        return new CustomerProfile
        {
            UserId = profileDto.UserId,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Mobile = profileDto.Mobile,
            Address = profileDto.Address,
            Gender = profileDto.Gender
        };
    }

    private void UpdateExistingProfile(CustomerProfile profile, CustomerProfileDto profileDto)
    {
        profile.FirstName = profileDto.FirstName;
        profile.LastName = profileDto.LastName;
        profile.Mobile = profileDto.Mobile;
        profile.Address = profileDto.Address;
        profile.Gender = profileDto.Gender;
    }
    public async Task<CustomerProfileDto?> GetByUserIdAsync(Guid userId)
    {
        var profile = await _context.CustomerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return null;

        return new CustomerProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Mobile = profile.Mobile,
            Address = profile.Address,
            Gender = profile.Gender
        };
    }
}