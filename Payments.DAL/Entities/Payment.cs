using Payments.DAL.Interfaces;
using System;

namespace Payments.DAL.Entities
{
    public class Payment : State
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Details { get; set; }

        public DateTime Date { get; set; }

        public int SenderId { get; set; }

        public virtual Account Sender { get; set; }

        public int RecipientId { get; set; }

        public virtual Account Recipient { get; set; }

        public bool IsSent { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }
    }
}
