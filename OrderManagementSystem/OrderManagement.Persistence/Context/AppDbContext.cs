using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<CustomerOrderView> CustomerOrderView{ get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerOrderView>(entity => 
        {
            entity.HasNoKey(); 
            entity.ToView("CustomerOrder");
        });
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("d1e8a8b5-5f8e-4a2a-bf5e-4e3c7d8e9f10"), // استفاده از GUID ثابت
                UserName = "admin@order.com",
                Password = HashPassword("a123"),
                CreateDate = DateTime.UtcNow,
                Role = "Admin"
            },
            new User
            {
                Id = Guid.Parse("a2b5c8d9-e1f2-4a3b-b5c6-7d8e9f10a11b"), // استفاده از GUID ثابت
                UserName = "customer@order.com",
                Password = HashPassword("c123"),
                CreateDate = DateTime.UtcNow,
                Role = "Customer"
            }
        );
        
        modelBuilder.Entity<CustomerProfile>().HasData(
            new CustomerProfile
            {
                Id = Guid.Parse("b3c6d9e1-f2a3-4b5c-6d7e-8f9a10b11c12"),
                UserId = Guid.Parse("a2b5c8d9-e1f2-4a3b-b5c6-7d8e9f10a11b"), // مطابقت با User مشتری
                FirstName = "Customer Name",
                LastName = "Customer Last Name",
                Address = "123 Main Street",
                Gender = "Male",
                Mobile = "09123456789" // اصلاح شماره موبایل به فرمت معتبر
            },
            new CustomerProfile
            {
                Id = Guid.Parse("b3c6d9e1-f2a3-4b5c-6d7e-8f9a10b11a12"),
                UserId = Guid.Parse("d1e8a8b5-5f8e-4a2a-bf5e-4e3c7d8e9f10"), // مطابقت با User مشتری
                FirstName = "Customer Name",
                LastName = "Customer Last Name",
                Address = "123 Main Street",
                Gender = "Male",
                Mobile = "09123456789" // اصلاح شماره موبایل به فرمت معتبر
            }
        );
        
        base.OnModelCreating(modelBuilder);
    }
    
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}