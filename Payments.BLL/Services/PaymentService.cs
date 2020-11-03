using AutoMapper;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        IUnitOfWork Database { get; set; }

        IMapper PaymentMapper { get; set; }

        public PaymentService(IUnitOfWork uow)
        {
            Database = uow;
            PaymentMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Account, AccountDTO>();
                cfg.CreateMap<Payment, PaymentDTO>();
            }).CreateMapper();
        }

        public OperationDetails Create(PaymentDTO paymentDTO)
        {
            if (paymentDTO != null)
            {
                var sender = Database.AccountRepository.Find(paymentDTO.SenderId);

                if (sender == null)
                {
                    return new OperationDetails(false, "Sender does not exist", "Sender");
                }
                if (sender.Limit < paymentDTO.Amount)
                {
                    if (sender.Limit != 0)
                        return new OperationDetails(false, "You have exceeded the limit", "Amount");
                }
                if (sender.Balance < paymentDTO.Amount)
                {
                    return new OperationDetails(false, "Insufficient funds", "Amount");
                }
                var resipient = Database.AccountRepository.Find(paymentDTO.Recipient.Id);

                if (resipient == null)
                {
                    return new OperationDetails(false, "Resipient does not exist", "Recipient");
                }

                Payment payment = new Payment()
                {
                    Amount = paymentDTO.Amount,
                    Date = paymentDTO.Date,
                    Details = paymentDTO.Details,
                    Name = paymentDTO.Name,
                    RecipientId = paymentDTO.Recipient.Id,
                    SenderId = paymentDTO.SenderId
                };

                sender.Balance -= payment.Amount;
                resipient.Balance += payment.Amount;
                Database.PaymentRepository.Create(payment);
                Database.AccountRepository.Update(sender);
                Database.AccountRepository.Update(resipient);
                Database.Save();

                return new OperationDetails(true, "Payment created successfully", "");
            }
            else
            {
                return new OperationDetails(false, "Payment is incorrect ", "");
            }

        }

        public IEnumerable<PaymentDTO> FindSentPayments(string userId)
        {
            return PaymentMapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDTO>>(
                Database.PaymentRepository.Find(p => p.Sender.ClientProfileId == userId));
        }

        public IEnumerable<PaymentDTO> FindSentPayments(string userId, int paymentCount, int sectorNumber)
        {
            return PaymentMapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDTO>>(
                Database.PaymentRepository.Find(p => p.Sender.ClientProfileId == userId).Skip((sectorNumber - 1) * paymentCount).Take(paymentCount));
        }

        public IEnumerable<PaymentDTO> FindReceivedPayments(string userId)
        {
            return PaymentMapper.Map<List<Payment>, List<PaymentDTO>>(
                Database.PaymentRepository.Find(p => p.Recipient.ClientProfileId == userId).ToList());
        }

        public IEnumerable<PaymentDTO> FindReceivedPayments(string userId, int paymentCount, int sectorNumber)
        {
            return PaymentMapper.Map<IEnumerable<Payment>, IEnumerable<PaymentDTO>>(
                Database.PaymentRepository.Find(p => p.Recipient.ClientProfileId == userId).Skip((sectorNumber - 1) * paymentCount).Take(paymentCount));

        }

        public PaymentDTO Find(int paymentId)
        {
            Payment payment = Database.PaymentRepository.Find(paymentId);

            return PaymentMapper.Map<Payment, PaymentDTO>(payment);
        }

        public void SendPayment(int paymentId)
        {
            Payment payment = Database.PaymentRepository.Find(paymentId);

            payment.Date = DateTime.Now;
            payment.IsSent = true;

            Database.PaymentRepository.Update(payment);
            Database.PaymentRepository.SaveChanges();
        }

    }
}
