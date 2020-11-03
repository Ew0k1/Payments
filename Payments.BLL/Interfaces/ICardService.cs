using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using System.Collections.Generic;

namespace Payments.BLL.Interfaces
{
    public interface ICardService
    {
        OperationDetails Create(CreditCardDTO card, string userId);

        IEnumerable<CreditCardDTO> FindUserCards(string userId);

        CreditCardDTO Find(int cardId);

        void BlockCard(int cardId);

        void UnblockCard(int cardId);

        void DeleteCard(int cardId);

        void SoftDelete(int cardId);

        void RestoreCard(int cardId);
    }
}
