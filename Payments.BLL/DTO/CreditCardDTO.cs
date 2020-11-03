using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.DTO
{
    public class CreditCardDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string SecurityCode { get; set; }

        public string UserProfileId { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsDeleted { get; set; }
    }
}
