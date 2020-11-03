using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.Interfaces
{
    public interface IPaymentService
    {
        OperationDetails Create(PaymentDTO paymentDTO);

        IEnumerable<PaymentDTO> FindSentPayments(string userId);

        IEnumerable<PaymentDTO> FindSentPayments(string userId, int paymentCount, int sectorNumber);

        IEnumerable<PaymentDTO> FindReceivedPayments(string userId);

        IEnumerable<PaymentDTO> FindReceivedPayments(string userId, int paymentCount, int sectorNumber);

        PaymentDTO Find(int paymentId);

        void SendPayment(int paymentId);
    }
}
