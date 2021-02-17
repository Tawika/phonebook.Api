namespace PhonebookApi.Models.Models.Phonebook
{
  using PhonebookApi.Models.Models.Entry;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class Phonebook
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(500)]
    public string Name { get; set; }

    public List<Entry> Entries { get; set; } = new List<Entry>();
  }
}