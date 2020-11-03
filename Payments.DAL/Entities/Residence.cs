using Payments.DAL.Entities;
using Payments.DAL.Interfaces;
using System.Collections.Generic;

namespace Payments.DAL.Entities
{
    public class Residence : State
    {
        public int ResidenceId { get; set; }

        public string Region { get; set; }

        public string District { get; set; }

        public string Settlement { get; set; }

        public int PostalCode { get; set; }

        public string Street { get; set; }

        public ICollection<ClientProfile> Users { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }


    }
}
