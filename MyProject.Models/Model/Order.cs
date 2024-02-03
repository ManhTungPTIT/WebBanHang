namespace MyProject.Models.Model;

public class Order
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public int Quantity { get; set; }
    
    public ICollection<OrderProduct> OrderProducts { get; set; }
}