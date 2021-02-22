using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookApi.Models.Database.Repository
{
  public interface IRepositoryBase<T> where T: class
  {
    IQueryable<T> GetAll(string include);

    void Add(T entity);

    Task<bool> SaveChanges();
  }
}
