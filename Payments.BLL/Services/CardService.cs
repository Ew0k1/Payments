using AutoMapper;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.DAL.Entities.Cards;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.BLL.Services
{
    public class CardService : ICardService
    {
        IUnitOfWork Database { get; set; }

        IMapper CardMapper { get; set; }

        public CardService(IUnitOfWork uow)
        {
            Database = uow;
            CardMapper = new MapperConfiguration(
                cfg => cfg.CreateMap<CreditCard, CreditCardDTO>()).CreateMapper();

        }
        public void BlockCard(int cardId)
        {
            Database.CardRepository.Block(cardId);
            Database.CardRepository.SaveChanges();
        }

        public OperationDetails Create(CreditCardDTO card, string userId)
        {
            List<CreditCard> c = Database.CardRepository.Find(x => x.Number == card.Number).ToList();
            if (c.Count==0)
            {
                DateTime expiration = new DateTime(
                Convert.ToInt32("20" + card.Year),
                Convert.ToInt32(card.Month), 1);

                if (expiration < DateTime.Now)
                {
                    return new OperationDetails(false, "Your card is expired", "");
                }

                var hash = Database.UserManager.PasswordHasher.HashPassword(card.SecurityCode);
                CreditCard creditCard = CreditCardFactory.CreateCreditCard(card, hash, userId);
                if (creditCard != null)
                {
                    Database.CardRepository.Create(creditCard);
                    Database.CardRepository.SaveChanges();
                    return new OperationDetails(true, "Card added successfully", "");
                }
                return new OperationDetails(false, "Card number is incorrect", "Number");
            }
            return new OperationDetails(false, "Card already exists", "");


        }

        public void DeleteCard(int cardId)
        {
            Database.CardRepository.Delete(cardId);
            Database.CardRepository.SaveChanges();
        }

        public IEnumerable<CreditCardDTO> FindUserCards(string userId)
        {
            return CardMapper.Map<List<CreditCard>, List<CreditCardDTO>>(Database.CardRepository.Find(
                x => x.UserProfileId == userId).ToList());
        }

        public CreditCardDTO Find(int cardId)
        {
            return CardMapper.Map<CreditCard, CreditCardDTO>(Database.CardRepository.Find(cardId));
        }

        public void SoftDelete(int cardId)
        {
            Database.CardRepository.SoftDelete(cardId);
            Database.CardRepository.SaveChanges();
        }

        public void UnblockCard(int cardId)
        {
            Database.CardRepository.Unblock(cardId);
            Database.CardRepository.SaveChanges();
        }

        public void RestoreCard(int cardId)
        {
            Database.CardRepository.Restore(cardId);
            Database.CardRepository.SaveChanges();
        }
    }
}
