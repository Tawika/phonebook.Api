namespace PhonebookApi.Models.Models.Phonebook.Command
{
  using global::Models.Database;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public class PhonebookCommandService
  {
    private readonly DatabaseContext _database;

    public PhonebookCommandService(DatabaseContext database)
    {
      _database = database;
    }

    public async Task<bool> SavePhonebookAsync(CreatePhonebookCommand createPhonebookCommand)
    {
      Phonebook newPhonebook = new Phonebook()
      {
        Name = createPhonebookCommand.Name,
      };

      createPhonebookCommand.Entries.ForEach(e =>
      {
        Entry.Entry newEntry = new Entry.Entry
        {
          Name = e.Name,
          PhoneNumber = e.PhoneNumber,
        };

        newPhonebook.Entries.Add(newEntry);
      });

      this._database.Phonebooks.Add(newPhonebook);

      return await this._database.SaveChangesAsync().ConfigureAwait(false) > 0;
    }
  }
}