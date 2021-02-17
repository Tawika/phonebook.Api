using Models.Database;
using PhonebookApi.Models.Models.Entry;
using PhonebookApi.Models.Models.Phonebook.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookApi.ModelsTest.Tests
{
  [Collection("Database collection")]
  public class PhonebookQueryServiceTests
  {
    private readonly DatabaseContext _context;

    public PhonebookQueryServiceTests(Initializer.Initializer initializer)
    {
      _context = initializer._context;
    }

    [Fact]
    public void GetPhonebookAsync_Test_ListOfPhonebookWithOneRecord()
    {
      // Naming your tests:
      // The name of your test should consist of three parts:
      // The name of the method being tested.
      // The scenario under which it's being tested.
      // The expected behavior when the scenario is invoked.

      // Arrange
      PhonebookQueryService queryService = new PhonebookQueryService(_context);

      // Act
      var result = queryService.GetPhonebookAsync().GetAwaiter().GetResult();

      // Assert
      Assert.True(result is List<PhonebookApi.Models.Models.Phonebook.Phonebook>);
      Assert.True(result.Count == 1);
    }

    [Fact]
    public void ActualPropertyName_CheckPropertyOnTypeEntry_NamePropertyExists()
    {
      // Arrange
      Type type = typeof(PhonebookQueryService);
      Type EntryType = typeof(Entry);

      PhonebookQueryService queryService = new PhonebookQueryService(_context);
      MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(x => x.Name == "ActualPropertyName" && x.IsPrivate).First();

      // Act
      string result = (string)method.Invoke(queryService, new object[] { EntryType, "name" });

      // Assert
      Assert.True(result == "Name");
      Assert.True(result != "name");
    }

    [Fact]
    public void GetPhonebookWithPagedEntries_SearchTextIsSam_PhonebookWithSamEntry()
    {
      // Arrange
      PhonebookQueryService queryService = new PhonebookQueryService(_context);

      // Act
      PhonebookApi.Models.Models.Phonebook.Phonebook result = queryService.GetPhonebookWithPagedEntries(5, 0, "sam", "name", true).GetAwaiter().GetResult();

      // Assert
      Assert.True(result!.Entries.Count == 1);
      Assert.True(result!.Entries.First().Name.Contains("sam", StringComparison.InvariantCultureIgnoreCase));
    }

    [Fact]
    public void GetPhonebookWithPagedEntries_IncorrectSortColumn_Exception()
    {
      // Arrange
      PhonebookQueryService queryService = new PhonebookQueryService(_context);

      // Act
      Action act = () => queryService.GetPhonebookWithPagedEntries(5, 0, "sam", "hello", true).GetAwaiter().GetResult();

      // Assert
      Assert.Throws<Exception>(act);
    }
  }
}