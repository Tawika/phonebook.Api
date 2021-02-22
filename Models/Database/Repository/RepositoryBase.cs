using Microsoft.EntityFrameworkCore;
using Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookApi.Models.Database.Repository
{
  public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
  {
    protected DatabaseContext RepositoryContext { get; set; }

    public RepositoryBase(DatabaseContext context)
    {
      RepositoryContext = context;
    }

    public void Add(T entity)
    {
      this.RepositoryContext.Add(entity);
    }

    public IQueryable<T> GetAll(string include)
    {
      if (!string.IsNullOrEmpty(include)) return this.RepositoryContext.Set<T>().Include(include).AsNoTracking();
      return this.RepositoryContext.Set<T>().AsNoTracking();
    }

    public async Task<bool> SaveChanges()
    {
      return await this.RepositoryContext.SaveChangesAsync() > 0;
    }
  }
}