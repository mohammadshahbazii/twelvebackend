using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class SendEmail
    {
        public static void Send(string To, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.c1.liara.email");
            SmtpServer.Port = 587;
            mail.From = new MailAddress("info@mail.12application.com", "سوپر اپلیکیشن دوازده");
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.IsBodyHtml = true;
            mail.Body = Body;


            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);


            SmtpServer.Credentials = new System.Net.NetworkCredential("musing_fermat_2c18k8", "b5ab2e4c-8cbd-4775-bf5d-44eac4b406ed");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
    }
}
