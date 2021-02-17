using Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookApi.ModelsTest.Initializer
{
  public class Initializer
  {
    public DatabaseContext _context { get; init; }
    public const string DatabaseName = "Test Phonebook";

    public Initializer()
    {
      _context = SetUpDatabase();
    }

    private DatabaseContext SetUpDatabase()
    {
      DbContextOptionsBuilder<DatabaseContext> builder = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
      DatabaseContext context = new DatabaseContext(builder.Options);

      PopulatePhonebook(context);
      PopulatePhonebookEntries(context);

      return context;
    }

    private static void PopulatePhonebook(DatabaseContext context)
    {
      // Add Phonebook
      PhonebookApi.Models.Models.Phonebook.Phonebook phonebook = new Models.Models.Phonebook.Phonebook()
      {
        Name = DatabaseName,
      };

      context.Phonebooks.Add(phonebook);

      context.SaveChanges();
    }

    private static void PopulatePhonebookEntries(DatabaseContext context)
    {
      // Find Phonebook
      PhonebookApi.Models.Models.Phonebook.Phonebook phonebook = context.Phonebooks.FirstOrDefault(c => c.Name.Contains(DatabaseName));

      _ = phonebook ?? throw new Exception("Error seeding phonebook.");

      phonebook.Entries.AddRange(new List<Models.Models.Entry.Entry>()
      {
        new Models.Models.Entry.Entry
        {
          Name = "Tawika Nyathi",
          PhoneNumber = "+27 83 737 5616",
        },
        new Models.Models.Entry.Entry
        {
          Name = "Sam Smith",
          PhoneNumber = "+27 12 737 5616",
        }
      }
      );

      context.SaveChanges();
    }
  }
}