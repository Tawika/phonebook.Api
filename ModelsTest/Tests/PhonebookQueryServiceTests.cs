using Models.Database;
using PhonebookApi.Models.Database.Repository;
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
    private readonly PhonebookRepository _phonebookRepository;
    private readonly PhonebookQueryService _queryService;

    public PhonebookQueryServiceTests(Initializer.Initializer initializer)
    {
      _phonebookRepository = initializer._phonebookRepository;
      _queryService = new PhonebookQueryService(_phonebookRepository);
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

      // Act
      var result = _queryService.GetPhonebookAsync().GetAwaiter().GetResult();

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

      MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(x => x.Name == "ActualPropertyName" && x.IsPrivate).First();

      // Act
      string result = (string)method.Invoke(_queryService, new object[] { EntryType, "name" });

      // Assert
      Assert.True(result == "Name");
      Assert.True(result != "name");
    }

    [Fact]
    public void GetPhonebookWithPagedEntries_SearchTextIsSam_PhonebookWithSamEntry()
    {
      // Arrange

      // Act
      PhonebookApi.Models.Models.Phonebook.Phonebook result = _queryService.GetPhonebookWithPagedEntries(5, 0, "sam", "name", true).GetAwaiter().GetResult();

      // Assert
      Assert.True(result!.Entries.Count == 1);
      Assert.True(result!.Entries.First().Name.Contains("sam", StringComparison.InvariantCultureIgnoreCase));
    }

    [Fact]
    public void GetPhonebookWithPagedEntries_IncorrectSortColumn_Exception()
    {
      // Arrange

      // Act
      Action act = () => _queryService.GetPhonebookWithPagedEntries(5, 0, "sam", "hello", true).GetAwaiter().GetResult();

      // Assert
      Assert.Throws<Exception>(act);
    }

    [Fact]
    public void GetPhonebookExistsAsync_CheckIfPhonebookExists_True()
    {
      Assert.True(_queryService.GetPhonebookExistsAsync().Result);
    }
  }
}