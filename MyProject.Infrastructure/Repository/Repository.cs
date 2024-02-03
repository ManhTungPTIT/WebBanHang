using Microsoft.EntityFrameworkCore;
using MyProject.Infrastrcture;
using MyProject.Infrastructure.MyProjectDB;

namespace MyProject.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MyProjectDb Context;
    private DbSet<T> _set;

    public Repository(MyProjectDb context)
    {
        Context = context;
        _set = this.Context.Set<T>();
    }
}