using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Payments.DAL.Identity
{
    class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var from = ""; // set email 
            var pass = ""; // set password

            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Port = 587,
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(from, pass)
            };

            // создаем письмо: message.Destination - адрес получателя
            var mail = new MailMessage()
            {
               
                From = new MailAddress(from),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
            mail.To.Add(message.Destination);

            await client.SendMailAsync(mail);
        }
    }
}
