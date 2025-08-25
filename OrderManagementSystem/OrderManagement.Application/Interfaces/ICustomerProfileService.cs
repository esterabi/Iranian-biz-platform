using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Interfaces;

public interface ICustomerProfileService
{
    Task<CustomerProfileDto> CreateAsync(CustomerProfileDto profileDto);
    Task<CustomerProfileDto?> GetByUserIdAsync(Guid userId);
}