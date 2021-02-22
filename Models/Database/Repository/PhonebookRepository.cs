using Microsoft.EntityFrameworkCore;
using Models.Database;
using PhonebookApi.Models.Extensions;
using PhonebookApi.Models.Models.Entry;
using PhonebookApi.Models.Models.Phonebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookApi.Models.Database.Repository
{

  public class PhonebookRepository : RepositoryBase<Phonebook>
  {
   

    public PhonebookRepository(DatabaseContext context): base(context)
    {
    }

    public async Task<List<Phonebook>> GetAllPhonebooks()
    {
      return await GetAll("Entries").ToListAsync().ConfigureAwait(false);
    }

    public async Task<Phonebook> GetPhonebookWithPagedEntries(int pageSize, int pageNo, string searchStr, string sortColumn, bool sortDesc)
    {
      Phonebook searchResult = await this.RepositoryContext.Phonebooks.Include(c => c.Entries.Where(d => string.IsNullOrEmpty(searchStr) ||
                                                                           (!string.IsNullOrEmpty(searchStr) && (EF.Functions.Like(d.Name, $"%{searchStr}%") || EF.Functions.Like(d.PhoneNumber, $"%{searchStr}%")))
                                                                      )).AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false);

      searchResult.Entries = searchResult.Entries.AsQueryable().OrderByMember(sortColumn, sortDesc).Skip(pageSize * pageNo).Take(pageSize).ToList();

      return searchResult;
    }
  }
}