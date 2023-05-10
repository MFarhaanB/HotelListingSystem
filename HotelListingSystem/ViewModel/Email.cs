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
        public void SendEmail(string Email, string subject, string Name, string status, bool isVerify = true)
        {
			try
			{
                MailMessage mail = new MailMessage();
                MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
                mail.From = from;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                if (isVerify)
                {
                    mail.Body = "Hi  " + Name + "<br/>Your hotel status is  " + status + "<br/><br/>Your's Sincerely<br/><strong>Hotel Listing Team</strong> ";
                }
                else
                {
                    mail.Body = "Hi  " + Name + "<br/><br/>"+ status + " <br/><br/>Your's Sincerely<br/><strong>Hotel Listing Team</strong> ";
                }
                mail.To.Add(Email);

                //mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential("HotelListVX@gmail.com", "ujzzmzrxomafbwkb");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
                //Dispose of email.
                mail.Dispose();
            }
			catch (Exception ex)
			{
                //alternate logic
            }
        }
        public void SendEmail(string Email, string subject, string Name, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
                mail.From = from;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = "Hi  " + Name + "<br/>" + body + "<br/><br/>Your's Sincerely<br/><strong>Hotel Listing Team</strong> ";

                mail.To.Add(Email);

                //mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential("HotelListVX@gmail.com", "ujzzmzrxomafbwkb");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
                //Dispose of email.
                mail.Dispose();
            }
            catch (Exception ex)
            {
                //alternate logic
            }
        }

    }
}