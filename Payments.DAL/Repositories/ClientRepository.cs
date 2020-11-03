using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;
using System.Data.Entity;

namespace Payments.DAL.Repositories
{
    public class ClientRepository : EFRepository<ClientProfile>
    {
        public ClientRepository(ApplicationContext context) : base(context) { }

        public void Block(string id)
        {
            var item = db.Set<ClientProfile>().Find(id);
            if (item != null)
            {
                item.IsBlocked = true;
            }
            Update(item);
        }

        public void Unblock(string id)
        {
            var item = db.Set<ClientProfile>().Find(id);
            if (item != null)
            {
                item.IsBlocked = false;
            }
            Update(item);
        }
    }
}
