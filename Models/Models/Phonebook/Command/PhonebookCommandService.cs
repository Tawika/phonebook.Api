namespace PhonebookApi.Models.Models.Phonebook.Command
{
  using global::Models.Database;
  using PhonebookApi.Models.Database.Repository;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public class PhonebookCommandService
  {
    private readonly PhonebookRepository _repository;

    public PhonebookCommandService(PhonebookRepository repository)
    {
      _repository = repository;
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

      this._repository.Add(newPhonebook);

      return await this._repository.SaveChanges().ConfigureAwait(false);
    }
  }
}