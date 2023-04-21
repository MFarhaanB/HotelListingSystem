using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace HotelListingSystem.ViewModel
{
    public class Email
    {
        public void SendEmail(string Email, string Name, string status)
        {
            MailMessage mail = new MailMessage();
            MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
            mail.From = from;
            mail.Subject = "Hotel Validation";
            mail.IsBodyHtml = true;
            mail.Body = "Hi  " + Name + "<br/> Your hotel status is  " + status;
            mail.To.Add(Email);

            //mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("africanmagicsystem@gmail.com", "zbpabilmryequenp");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);
            //Clean-up.          
            //Dispose of email.
            mail.Dispose();

        }
    }
}