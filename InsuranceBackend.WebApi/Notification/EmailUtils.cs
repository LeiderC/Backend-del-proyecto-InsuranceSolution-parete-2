using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace InsuranceBackend.WebApi.Notification
{
    public class EmailUtils
    {
        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message, MimeKit.Text.TextFormat textFormat)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Cc.Add(message.Cc);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(textFormat)
            { Text = message.Content };
            return mimeMessage;
        }

        public void SendMail(NotificationMetadata _notificationMetadata, string receiver,
        string receiverName, string subject, MimeKit.Text.TextFormat textFormat, string content,
        string ccName, string cc)
        {
            EmailMessage message = new EmailMessage();
            message.Sender = new MailboxAddress("Happy Gigas", _notificationMetadata.Sender);
            message.Reciever = new MailboxAddress(receiverName, receiver);
            message.Cc = new MailboxAddress(ccName, cc);
            message.Subject = subject;
            message.Content = content;
            var mimeMessage = CreateMimeMessageFromEmailMessage(message, textFormat);
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_notificationMetadata.SmtpServer,
                _notificationMetadata.Port, true);
                smtpClient.Authenticate(_notificationMetadata.UserName,
                _notificationMetadata.Password);
                smtpClient.Send(mimeMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}