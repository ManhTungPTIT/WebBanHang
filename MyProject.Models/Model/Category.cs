namespace MyProject.Models.Model;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? CreateOn { get; set; }
    public DateTime? UpdateOn { get; set; }
    public DateTime? DeleteOn { get; set; }
    
    public ICollection<CategoryProduct> CategoryProducts { get; set; }
}