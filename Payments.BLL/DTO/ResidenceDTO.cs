using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.DTO
{
    public class ResidenceDTO
    {
        public int Id { get; set; }

        public string Region { get; set; }

        public string District { get; set; }

        public string Settlement { get; set; }

        public int PostalCode { get; set; }

        public string Street { get; set; }
    }
}
