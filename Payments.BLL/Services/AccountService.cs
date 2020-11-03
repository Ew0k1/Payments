using AutoMapper;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.BLL.Services
{
    public class AccountService : IAccountService
    {
        const int startNumber = 1000000000;

        IUnitOfWork Database { get; set; }

        IMapper AccountMapper { get; set; }

        public AccountService(IUnitOfWork uow)
        {
            Database = uow;
            AccountMapper = new MapperConfiguration(
                  cfg => cfg.CreateMap<Account, AccountDTO>()).CreateMapper();
        }

        public void BlockAccount(int accountId)
        {
            Database.AccountRepository.Block(accountId);
            Database.AccountRepository.SaveChanges();
        }

        public void Create(AccountDTO accountDTO)
        {
            var acc = new Account()
            {
                Name = accountDTO.Name,
                Balance = 0m,
                ClientProfileId = accountDTO.UserProfileId,
                CreditCardId = accountDTO.CreditCardId,
                Limit = 0,
            };
            Database.AccountRepository.Create(acc);
            Database.AccountRepository.SaveChanges();
            acc.Number = acc.Id + startNumber;
            Database.AccountRepository.Update(acc);
            Database.AccountRepository.SaveChanges();
        }

        public void DeleteAccount(int accountId)
        {
            Database.CardRepository.Delete(accountId);
            Database.CardRepository.SaveChanges();
        }

        public AccountDTO Find(int accountId)
        {
            return AccountMapper.Map<Account, AccountDTO>(Database.AccountRepository.Find(accountId));
        }

        public AccountDTO FindByNumber(int accountNumber)
        {
            return AccountMapper.Map<Account, AccountDTO>(Database.AccountRepository.Find(a => a.Number == accountNumber).FirstOrDefault());
        }

        public IEnumerable<AccountDTO> FindUserAccounts(string userId)
        {
            return AccountMapper.Map<List<Account>, List<AccountDTO>>(Database.AccountRepository.Find(
                x => x.ClientProfileId == userId).ToList());
        }

        public OperationDetails ChangeLimit(int accountId, int newLimit)
        {
            var account = Database.AccountRepository.Find(accountId);

            if (account != null)
            {
                if (newLimit < 0)
                {
                    return new OperationDetails(false, "Limit cannot be negative", "Limit");
                }
                else
                {
                    account.Limit = newLimit;
                    Database.AccountRepository.Update(account);
                    Database.AccountRepository.SaveChanges();
                    return new OperationDetails(true, "Limit changed successfully", "");
                }
            }
            else
            {
                return new OperationDetails(false, "Account doesn`t exist", "");
            }
        }


        public void RestoreAccount(int accountId)
        {
            Database.AccountRepository.Restore(accountId);
            Database.AccountRepository.SaveChanges();

        }

        public void SoftDelete(int accountId)
        {
            Database.AccountRepository.SoftDelete(accountId);
            Database.AccountRepository.SaveChanges();
        }

        public void UnblockAccount(int accountId)
        {
            Database.AccountRepository.Unblock(accountId);
            Database.AccountRepository.SaveChanges();
        }
    }
}
