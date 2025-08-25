using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.WebAPI.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // 📝 ثبت سفارش جدید
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
    {
        var result = await _orderService.PlaceOrderAsync(orderDto);
        return Ok(result);
    }

    // 📦 دریافت سفارش با شناسه
    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var result = await _orderService.GetByIdAsync(orderId);
        if (result == null)
            return NotFound($"Order with ID {orderId} not found.");

        return Ok(result);
    }

    // 📋 دریافت سفارش‌های یک مشتری
    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetOrdersByCustomer(Guid customerId)
    {
        var result = await _orderService.GetOrdersByCustomerAsync(customerId);
        return Ok(result);
    }
}
