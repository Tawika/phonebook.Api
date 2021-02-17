namespace PhonebookApi.Models.Models.Entry
{
  using Newtonsoft.Json;
  using Phonebook;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class Entry
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Phonebook))]
    public int PhonebookId { get; set; }
    
    [JsonIgnore]
    [NotMapped]
    public Phonebook Phonebook { get; set; }

    [StringLength(500)]
    public string Name { get; set; }

    public string PhoneNumber { get; set; }
  }
}
