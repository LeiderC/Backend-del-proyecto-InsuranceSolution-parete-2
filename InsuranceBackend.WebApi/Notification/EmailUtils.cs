using System;
using System.Collections.Generic;
using System.IO;
using InsuranceBackend.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace InsuranceBackend.WebApi.Notification
{
    public class EmailUtils
    {
        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message,
        MimeKit.Text.TextFormat textFormat, List<DigitalizedFile> digitalizedFiles)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Cc.Add(message.Cc);
            mimeMessage.Subject = message.Subject;
            if (digitalizedFiles != null && digitalizedFiles.Count > 0)
            {
                var multipart = new Multipart("mixed");
                foreach (var df in digitalizedFiles)
                {
                    var folderName = Path.Combine("Resources", "DigitalizedFiles");
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var filePath = Path.Combine(uploads, df.FileRoute);
                    if (!System.IO.File.Exists(filePath))
                        throw new Exception("No existe el archivo para adjuntar");
                    string ext = Path.GetExtension(df.FileName).Replace(".", "");
                    var attachment = new MimePart(ext, ext)
                    {
                        Content = new MimeContent(File.OpenRead(filePath), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = df.FileName
                    };
                    multipart.Add(attachment);
                }
                multipart.Add(new TextPart(textFormat)
                { Text = message.Content });
                mimeMessage.Body = multipart;
            }
            else
            {
                mimeMessage.Body = new TextPart(textFormat)
                { Text = message.Content };
            }
            return mimeMessage;
        }

        public void SendMail(NotificationMetadata _notificationMetadata, string receiver,
        string receiverName, string subject, MimeKit.Text.TextFormat textFormat, string content,
        string ccName, string cc, List<DigitalizedFile> digitalizedFiles)
        {
            EmailMessage message = new EmailMessage();
            message.Sender = new MailboxAddress("Happy Gigas", _notificationMetadata.Sender);
            message.Reciever = new MailboxAddress(receiverName, receiver);
            message.Cc = new MailboxAddress(ccName, cc);
            message.Subject = subject;
            message.Content = content;
            var mimeMessage = CreateMimeMessageFromEmailMessage(message, textFormat, digitalizedFiles);
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