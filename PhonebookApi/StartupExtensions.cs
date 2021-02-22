using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Database;
using PhonebookApi.Models.Models.Phonebook.Query;
using PhonebookApi.Models.Models.Phonebook.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhonebookApi.Models.Database.Repository;
using Microsoft.Extensions.Configuration;

namespace PhonebookApi
{
  public static class StartupExtensions
  {
    public static void AddDataServices(this IServiceCollection services)
    {
      services.AddScoped<PhonebookRepository>();
      services.AddScoped<PhonebookQueryService>();
      services.AddScoped<PhonebookCommandService>();
    }

    public static void AddDatabaseServicesInMemorySQLlite(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<DatabaseContext>(opt => opt.UseSqlite(configuration.GetConnectionString("Main")));
    }
  }
}