using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PhonebookApi.Models.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookApi.Models.Models.Phonebook.Query
{
  public class PhonebookQueryService
  {
    public const string entries = "Entries";
    public const string errorMessage_NoPhonebookFound = "No phonebook was found.";
    public const string errorMessage_PageSize = "Page Size cannot be 0.";
    public const string errorMessage_SortColumn = "Sort column doesn't exist.";

    private readonly PhonebookRepository _repository;

    public PhonebookQueryService(PhonebookRepository repository)
    {
      _repository = repository;
    }

    public async Task<List<Phonebook>> GetPhonebookAsync()
    {
      return await _repository.GetAllPhonebooks().ConfigureAwait(false);
    }

    public async Task<Phonebook> GetPhonebookWithPagedEntries(int pageSize, int pageNo, string searchStr, string sortColumn, bool sortDesc)
    {
      // Check pageSize
      if (pageSize == 0) throw new Exception(errorMessage_PageSize);

      // Check sortColumn 
      string propertyName = ActualPropertyName(typeof(Entry.Entry), sortColumn);
      return string.IsNullOrEmpty(propertyName)
          ? throw new ArgumentNullException(nameof(sortColumn))
          : await this._repository.GetPhonebookWithPagedEntries(pageSize, pageNo, searchStr, propertyName, sortDesc).ConfigureAwait(false);
    }

    public async Task<bool> GetPhonebookExistsAsync()
    {
      return await this._repository.GetAll(entries).AnyAsync().ConfigureAwait(false);
    }

    public async Task<byte[]> DownloadPhonebook()
    {
      ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

      using ExcelPackage package = new ExcelPackage();

      ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(entries);

      Phonebook phonebook = await this._repository.GetAll(entries).FirstOrDefaultAsync().ConfigureAwait(false);

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

    private static string ActualPropertyName(Type type, string name)
    {
      return type.GetProperties().Where(c => c.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)).Select(d => d.Name).FirstOrDefault();
    }
  }
}