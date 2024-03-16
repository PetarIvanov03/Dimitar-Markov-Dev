using LegalTranslation.Data;
using LegalTranslation.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace LegalTranslation.Helpers
{
    public class EmailSender
    {
        private Emails AdminEmail { get; set; }
        private Emails ComapnyEmail { get; set; }
        private readonly AppDbContext _context;

        public EmailSender(AppDbContext context)
        {
            _context = context;
            AdminEmail = _context.Emails.FirstOrDefault(admin => admin.IsAdmin == true);
            ComapnyEmail = _context.Emails.FirstOrDefault(admin => admin.IsAdmin == false);
        }
        private void Rescue(string body, string subject, List<string> uploadedFilePaths)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("testpetar1337@gmail.com");
            mailMessage.To.Add("testpetar1337@gmail.com");
            mailMessage.To.Add("i.petarivanov03@gmail.com");
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("testpetar1337@gmail.com", "uehb qajg ssuf qcyy");
            smtpClient.EnableSsl = true;



            foreach (var filePath in uploadedFilePaths)
            {
                var attachment = new Attachment(filePath, System.Net.Mime.MediaTypeNames.Application.Octet);
                mailMessage.Attachments.Add(attachment);
            }

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Type (Send files) - Sent Successfully." + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Type (Send files) - Error: " + ex.Message);
            }
        }




        public void SendFiles(string body, string subject, List<string> uploadedFilePaths)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.AdminEmail.Name);
            mailMessage.To.Add(this.ComapnyEmail.Name);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(this.AdminEmail.Name, this.AdminEmail.Password);
            smtpClient.EnableSsl = true;



            foreach (var filePath in uploadedFilePaths)
            {
                var attachment = new Attachment(filePath, System.Net.Mime.MediaTypeNames.Application.Octet);
                mailMessage.Attachments.Add(attachment);
            }

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Type (Send files) - Sent Successfully." + DateTime.Now);
            }
            catch (Exception ex)
            {
                Rescue(body, subject, uploadedFilePaths);
                Console.WriteLine("Email Type (Send files) - Error: " + ex.Message);
            }

        }





        public void ForgottenPassword(string body, string subject)
        {

            Console.WriteLine("New E-mail!");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.AdminEmail.Name);
            mailMessage.To.Add(this.AdminEmail.Name);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(this.AdminEmail.Name, this.AdminEmail.Password);
            smtpClient.EnableSsl = true;

            List<string> list = new List<string>();

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Type (ForgottenPassword) - Sent Successfully." + DateTime.Now);
            }
            catch (Exception ex)
            {
                Rescue(body, subject, list);
                Console.WriteLine("Email Type (ForgottenPassword) - Error: " + ex.Message);
            }
        }

        public void NotAdmin(string body, string subject)
        {

            Console.WriteLine("New E-mail!");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(this.AdminEmail.Name);
            mailMessage.To.Add(this.AdminEmail.Name);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(this.AdminEmail.Name, this.AdminEmail.Password);
            smtpClient.EnableSsl = true;

            List<string> list = new List<string>();

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Type (NotAdmin) - Sent Successfully." + DateTime.Now);
            }
            catch (Exception ex)
            {
                Rescue(body, subject, list);
                Console.WriteLine("Email Type (NotAdmin) - Error: " + ex.Message);
            }
        }
    }
}
