using Microsoft.EntityFrameworkCore;
using MyProject.Models.Configuration;

namespace MyProject.Infrastructure.MyProjectDB;

public class MyProjectDb : DbContext
{
    public MyProjectDb(DbContextOptions options): base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new CategoryproductConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new OrderProductConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
    }
}