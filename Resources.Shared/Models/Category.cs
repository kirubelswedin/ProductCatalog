using System.ComponentModel.DataAnnotations;

namespace Resources.Shared.Models;

public class Category
{
    public string Id { get; set; } = null!;
    
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
}