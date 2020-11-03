using System;
using System.Collections.Generic;

namespace Payments.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Guid { get; set; }

        public List<CreditCardDTO> Cards { get; set; }

        public List<AccountDTO> Accounts { get; set; }

        public bool IsBlocked { get; set; }

        public PictureDTO Picture { get; set; }

        //public ResidenceDTO Residence { get; set; }
    }
}
