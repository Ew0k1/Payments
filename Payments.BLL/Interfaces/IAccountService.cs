using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using System.Collections.Generic;

namespace Payments.BLL.Interfaces
{
    public interface IAccountService
    {
        void BlockAccount(int accountId);

        void Create(AccountDTO accountDTO);

        void DeleteAccount(int accountId);

        AccountDTO Find(int accountId);

        AccountDTO FindByNumber(int accountNumber);

        IEnumerable<AccountDTO> FindUserAccounts(string userId);

        OperationDetails ChangeLimit(int accountId, int newLimit);

        void RestoreAccount(int accountId);

        void SoftDelete(int accountId);

        void UnblockAccount(int accountId);





    }
}
