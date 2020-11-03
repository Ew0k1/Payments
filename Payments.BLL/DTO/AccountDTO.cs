namespace Payments.BLL.DTO
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public int Limit { get; set; }

        public string UserProfileId { get; set; }

        public int CreditCardId { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsDeleted { get; set; }
    }
}
