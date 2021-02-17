using Microsoft.EntityFrameworkCore;
using Models.Database;
using OfficeOpenXml;
using PhonebookApi.Models.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookApi.Models.Models.Phonebook.Query
{
  public class PhonebookQueryService
  {
    public const string entries = "Entries";
    public const string errorMessage_NoPhonebookFound = "No phonebook was found.";
    public const string errorMessage_PageSize = "Page Size cannot be 0.";
    public const string errorMessage_SortColumn = "Sort column doesn't exist.";

    private readonly DatabaseContext _database;

    public PhonebookQueryService(DatabaseContext database)
    {
      _database = database;
    }

    public async Task<List<Phonebook>> GetPhonebookAsync()
    {
      return await this._database.Phonebooks.Include("Entries").AsNoTracking().ToListAsync().ConfigureAwait(false);
    }

    public async Task<Phonebook> GetPhonebookWithPagedEntries(int pageSize, int pageNo, string searchStr, string sortColumn, bool sortDesc)
    {
      // Check pageSize
      if (pageSize == 0) throw new Exception(errorMessage_PageSize);
      
      Phonebook searchResult = await this._database.Phonebooks.Include(c => c.Entries.Where(d => string.IsNullOrEmpty(searchStr) ||
                                                                           (!string.IsNullOrEmpty(searchStr) && (EF.Functions.Like(d.Name, $"%{searchStr}%") || EF.Functions.Like(d.PhoneNumber, $"%{searchStr}%")))
                                                                      )).AsNoTracking().FirstOrDefaultAsync().ConfigureAwait(false);

      // Check sortColumn 
      string propertyName = ActualPropertyName(typeof(Entry.Entry), sortColumn);
      if (string.IsNullOrEmpty(propertyName)) throw new Exception(errorMessage_SortColumn);

      // Replace current entries with ordered entries
      searchResult.Entries = searchResult.Entries.AsQueryable().OrderByMember(propertyName, sortDesc).Skip(pageSize * pageNo).Take(pageSize).ToList();

      return searchResult;
    }

    public async Task<byte[]> DownloadPhonebook()
    {
      ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

      using ExcelPackage package = new ExcelPackage();

      ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(entries);

      Phonebook phonebook = await this._database.Phonebooks.Include(c => c.Entries).FirstOrDefaultAsync().ConfigureAwait(false);

      _ = phonebook ?? throw new Exception(errorMessage_NoPhonebookFound);

      // Add header
      workSheet.Cells["A1"].Value = "Name";
      workSheet.Cells["B1"].Value = "Phone Number";

      int row = 2;
      int col = 1;

      // Add data
      foreach (Entry.Entry e in phonebook.Entries)
      {
        // Add Name
        workSheet.Cells[row, col++].Value = e.Name;

        // Add Phone Number
        workSheet.Cells[row, col].Value = e.PhoneNumber;

        // Reset
        row++;
        col = 1;
      }

      return package.GetAsByteArray();
    }

    public async Task<bool> GetPhonebookExistsAsync()
    {
      return await this._database.Phonebooks.AsNoTracking().AnyAsync().ConfigureAwait(false);
    }

    private static string ActualPropertyName(Type type, string name)
    {
      return type.GetProperties().Where(c => c.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)).Select(d => d.Name).FirstOrDefault();
    }
  }
}