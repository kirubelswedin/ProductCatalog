using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Resources.Shared.Models;

public class Product
{
    public string Id { get; set; } = null!;
    
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
    
    [Range(0.01, float.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public float Price { get; set; } 
    
    public string FormattedPrice => Price.ToString("C2", CultureInfo.CurrentCulture); 
    
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")] 
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    public Category Category { get; set; } = null!;
}