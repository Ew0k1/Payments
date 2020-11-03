using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Details { get; set; }

        public DateTime Date { get; set; }

        public int SenderId { get; set; }

        public AccountDTO Sender { get; set; }

        public int RecipientId { get; set; }

        public AccountDTO Recipient { get; set; }

        public bool IsSent { get; set; }

    }
}
