namespace MyProject.Models.Model;

public class CategoryProduct
{
    public int Id { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
}