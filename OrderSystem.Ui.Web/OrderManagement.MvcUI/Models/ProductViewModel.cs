using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.MvcUI.Models;

public class ProductViewModel
{

    public Guid Id { get; set; }

    [Required]
    [Display(Name = "نام کالا")]
    public string Name { get; set; }
    [Required]
    [Display(Name = "توضیحات")]
    public string Description { get; set; }
    [Display(Name = "تصویر کالا")]
    public IFormFile Image { get; set; }
    public string? ImageUrl { get; set; }
    [Required]
    [Display(Name = "دسته بندی")]
    public string Category { get; set; } 
    [Required]
    [Display(Name = "موجودی انبار")]
    public int StockQuantity { get; set; }
    [Required]
    [Display(Name = "قیمت فروش - تومان")]
    public double Price { get; set; }
}

public class ProductEditViewModel
{

    public Guid Id { get; set; }

    [Required(ErrorMessage = "لطفا نام کالا را وارد کنید")]
    [Display(Name = "نام کالا")]
    public string Name { get; set; }
    [Required]
    [Display(Name = "توضیحات")]
    public string Description { get; set; }
    [Display(Name = "تصویر کالا")]
    public IFormFile? Image { get; set; }
    public string ImageUrl { get; set; }
    [Required]
    [Display(Name = "دسته بندی")]
    public string Category { get; set; } 
    [Required]
    [Display(Name = "موجودی انبار")]
    public int StockQuantity { get; set; }
    [Required]
    [Display(Name = "قیمت فروش - تومان")]
    public double Price { get; set; }
}