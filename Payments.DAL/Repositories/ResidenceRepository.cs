using Payments.DAL.EF;
using Payments.DAL.Entities;

namespace Payments.DAL.Repositories
{
    public class ResidenceRepository : EFRepository<Residence>
    {
        public ResidenceRepository(ApplicationContext context) : base(context) { }
    }
}
