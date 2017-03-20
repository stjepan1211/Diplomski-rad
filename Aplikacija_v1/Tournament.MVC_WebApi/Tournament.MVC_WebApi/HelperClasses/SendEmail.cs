using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Tournament.MVC_WebApi.HelperClasses
{
    public class SendEmail
    {
        public static bool Send(string fromAddress, string fromPassword, string toAddress)
        {
            try
            {
                string subject = "Hello from Tournaments application";
                string body = "From: " + "Tournament application" + "\n";
                body += "Hi, log in and check application features." + "\n";
                body += "Questions: \n" + "stjepanbaricevic1211@gmail.com" + "\n";

                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                smtp.Send(fromAddress, toAddress, subject, body);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}