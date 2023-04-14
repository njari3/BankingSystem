using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
