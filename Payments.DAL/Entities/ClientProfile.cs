using Payments.DAL.Entities.Cards;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payments.DAL.Entities
{
    public class ClientProfile : State
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Picture Picture { get; set; }

        public virtual ICollection<CreditCard> Cards { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }
    }
}
