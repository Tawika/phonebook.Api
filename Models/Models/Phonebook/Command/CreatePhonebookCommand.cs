namespace PhonebookApi.Models.Models.Phonebook.Command
{
  using PhonebookApi.Models.Models.Entry.Command;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;

  public class CreatePhonebookCommand
  {
    [StringLength(500)]
    public string Name { get; set; }

    public List<CreateEntry> Entries { get; set; } = new List<CreateEntry>();
  }
}
