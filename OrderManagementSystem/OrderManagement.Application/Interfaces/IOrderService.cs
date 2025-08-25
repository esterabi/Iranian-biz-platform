using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> PlaceOrderAsync(OrderDto orderDto);
    Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(Guid customerId);
    Task<OrderDto?> GetByIdAsync(Guid orderId);
    Task<List<OrdersListDto>?> GetAllAsync();
}