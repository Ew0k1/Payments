using Payments.DAL.EF;
using Payments.DAL.Entities;

namespace Payments.DAL.Repositories
{
    public class AccountRepository : EFRepository<Account>
    {
        public AccountRepository(ApplicationContext context) : base(context) { }
    }
}
