using Microsoft.EntityFrameworkCore;
using PhonebookApi.Models.Models.Entry;
using PhonebookApi.Models.Models.Phonebook;
using System;

namespace Models.Database
{
  public class DatabaseContext : DbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> contextOptions) : base(contextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Seed initial data
      // modelBuilder.SeedDatabase();
    }

    public DbSet<Phonebook> Phonebooks { get; set; }
  }
}