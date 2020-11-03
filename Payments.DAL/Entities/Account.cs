using Payments.DAL.Entities.Cards;
using Payments.DAL.Interfaces;
using System.Collections.Generic;

namespace Payments.DAL.Entities
{
    public class Account : State
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public int Limit { get; set; }

        public string ClientProfileId { get; set; }

        public virtual ClientProfile ClientProfile { get; set; }

        public int CreditCardId { get; set; }

        public virtual CreditCard CreditCard { get; set; }

        public virtual ICollection<Payment> PaymentsRecieve { get; set; }

        public virtual ICollection<Payment> PaymentsSent { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }
    }
}
