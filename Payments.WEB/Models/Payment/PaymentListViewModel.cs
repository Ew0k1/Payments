using Payments.WEB.Models.Pagination;
using System.Collections.Generic;

namespace Payments.WEB.Models.Payment
{
    public class PaymentListViewModel
    {
        public IEnumerable<PaymentViewModel> PaymentsSent { get; set; }

        public IEnumerable<PaymentViewModel> PaymentsReceived { get; set; }

        public PageInfo PageInfoSent { get; set; }

        public PageInfo PageInfoReceive{ get; set; }
    }
}