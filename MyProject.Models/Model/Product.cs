namespace MyProject.Models.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string Image { get; set; }
    public DateTime? CreateOn { get; set; }
    public DateTime? UpdateOn { get; set; }
    public DateTime? DeleteOn { get; set; }
    
    public ICollection<CategoryProduct> CategoryProducts { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
}