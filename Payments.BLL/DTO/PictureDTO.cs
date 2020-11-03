using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.DTO
{
    public class PictureDTO
    {
        public string Name { get; set; }

        public byte[] Image { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBlocked { get; set; }
    }
}
