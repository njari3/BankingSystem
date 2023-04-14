using BankingSystem.Enums;

namespace BankingSystem.Dtos
{
    public class AccountMoneyTransferDto
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public MoneyTransfer MoneyTransfer { get; set; }

    }
}
