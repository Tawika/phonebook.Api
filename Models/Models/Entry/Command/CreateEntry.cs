using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookApi.Models.Models.Entry.Command
{
  public class CreateEntry
  {
    [StringLength(500)]
    public string Name { get; set; }

    public string PhoneNumber { get; set; }
  }
}
