using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models
{
    public class Account
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal Balance { get; set; }
        public User User { get; set; }
    }
}
