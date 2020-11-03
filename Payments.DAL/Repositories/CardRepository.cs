using Payments.DAL.EF;
using Payments.DAL.Entities.Cards;

namespace Payments.DAL.Repositories
{
    public class CardRepository : EFRepository<CreditCard>
    {
        public CardRepository(ApplicationContext context) : base(context) { }
    }
}
