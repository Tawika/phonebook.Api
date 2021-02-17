using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookApi.ModelsTest.Initializer
{
  [CollectionDefinition("Database collection")]
  public class DatabaseCollection : ICollectionFixture<Initializer>
  {
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
  }
}
