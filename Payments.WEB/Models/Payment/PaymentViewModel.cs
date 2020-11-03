using Payments.WEB.Models.Card;
using System;

namespace Payments.WEB.Models.Payment
{
    public class PaymentViewModel
    {
        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Details { get; set; }

        public DateTime Date { get; set; }

        public AccountViewModel Sender { get; set; }

        public AccountViewModel Recipient { get; set; }

        public bool IsSent { get; set; }
    }
}