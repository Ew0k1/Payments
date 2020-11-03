namespace Payments.WEB.Models.Card
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public int Limit { get; set; }

        public bool IsBlocked { get; set; }
    }
}