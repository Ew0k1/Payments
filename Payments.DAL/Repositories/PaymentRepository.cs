using Payments.DAL.EF;
using Payments.DAL.Entities;
using System;
using System.Collections.Generic;

namespace Payments.DAL.Repositories
{
    public class PaymentRepository : EFRepository<Payment>
    {
        public PaymentRepository(ApplicationContext context) : base(context) { }
               
    }
}
