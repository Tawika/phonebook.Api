using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookApi.Extensions
{
  public static class MigrationManager
  {
    public static IHost MigrateDatabase(this IHost host)
    {
      using (IServiceScope scope = host.Services.CreateScope())
      {
        using DatabaseContext appContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        try
        {
          appContext.Database.EnsureCreated();
        }
        catch (Exception)
        {
          throw;
        }
      }
      return host;
    }
  }
}