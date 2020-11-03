using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payments.WEB.Models.Card
{
    public class CardViewModel
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string Number { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }
    }
}