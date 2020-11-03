using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.DAL.Interfaces
{
    public interface State
    {
        bool IsDeleted { get; set; }
        bool IsBlocked { get; set; }
    }
}
