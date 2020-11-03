using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payments.WEB.Models.Account
{
    public class PictureViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Image { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }
    }
}