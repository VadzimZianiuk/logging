using LogSender.Models;
using System.Net.Mail;

namespace LogSender
{
    public class EmailClient
    {
        public void Send(Sender sender, string toAddress, string subject, string body)
        {
            using var mail = new MailMessage { From = new MailAddress(sender.From) };
            mail.To.Add(toAddress);
            mail.Subject = subject;
            mail.IsBodyHtml = false;
            if (sender.UseAttachment)
            {
                mail.Attachments.Add(Attachment.CreateAttachmentFromString(body, "text"));
            }
            else
            {
                mail.Body = body;
            }

            using var client = new SmtpClient(sender.Smtp, sender.Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(sender.From, sender.Password),
                EnableSsl = sender.Ssl
            };
            client.Send(mail);
        }
    }
}
