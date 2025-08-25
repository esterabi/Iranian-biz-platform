using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Persistence.Context;

namespace OrderManagement.Application.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto> PlaceOrderAsync(OrderDto orderDto)
    {
        var order = new Order
        {
            CustomerId = orderDto.CustomerId,
            Status = "Registered",
            CreatedAt = orderDto.CreatedAt,
            Items = orderDto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return new OrderDto { Id = order.Id, Status = order.Status };
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(Guid customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            }).ToListAsync();
    }

    public async Task<OrderDto?> GetByIdAsync(Guid orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    public async Task<List<OrdersListDto>?> GetAllAsync()
    {
        var result = _context.CustomerOrderView
            .AsNoTracking()
            .ToList();

        var ordersList = result.Select(o => new OrdersListDto()
        {
            Quantity = o.Quantity,
            CustomerName = $"{o.FirstName} {o.LastName}",
            ProductName = o.ProductName,
            CreatedAt = o.CreatedAt,
            Status = o.Status,
            CustomerId = o.CustomerId,
            OrderId = o.OrderId,
            Price = o.Price,
            Description = o.Description,
            Fee = o.Fee
        }).ToList();
        
        return ordersList;
    }
}
