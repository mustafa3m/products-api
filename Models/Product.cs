using System.ComponentModel.DataAnnotations; 

namespace products_api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Brand { get; set; }
    public int Stock { get; set; }
}
