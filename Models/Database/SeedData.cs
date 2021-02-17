using Microsoft.EntityFrameworkCore;
using PhonebookApi.Models.Models.Phonebook;

namespace Models.Database
{
  public static class SeedData
  {
    public static void SeedDatabase(this ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Phonebook>().HasData(
        new Phonebook { Id = 1, Name = "Tawika's Phonebook"}
        );
    }
  }
}