using Payments.DAL.EF;
using Payments.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.DAL.Repositories
{
    public class PictureRepository : EFRepository<Picture>
    {
        public PictureRepository(ApplicationContext context) : base(context) { }
    }
}
