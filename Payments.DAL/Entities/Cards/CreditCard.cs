using Payments.DAL.Entities;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;


namespace Payments.DAL.Entities.Cards
{

    public abstract class CreditCard : State
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string SecurityCodeHash { get; set; }

        public string UserProfileId { get; set; }

        public virtual ClientProfile UserProfile { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }

    }
}
