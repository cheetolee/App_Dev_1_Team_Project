using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Inventory_Management.Service
{
    public static class EmailConfirmation
    {
        private static string FROM_EMAIL = "appdevoneassignment2022@gmail.com";
        private static string FROM_PASS = "y2KwoJjV";

        private static String REGISTRATION_MSG = "Thanks for going all through the registration process";
        private static String REGISTRATION_SUBJECT = "Registration complete";

        private static String ORDER_MSG = "Thanks for your order";
        private static String ORDER_SUBJECT = "Order Confirmation";

        public static void sendOrderConfirmation(String to)
        {
            sendEmail(to, ORDER_MSG, ORDER_SUBJECT);
        }

        public static void sendRegistrationConfirmation(String to)
        {
            sendEmail(to, REGISTRATION_MSG, REGISTRATION_SUBJECT);
        }

        private static void sendEmail(String to, String message, String subject)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(FROM_EMAIL, FROM_PASS);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(FROM_EMAIL),
                    Subject = subject,
                    Body = message
                };

                mail.To.Add(new MailAddress(to));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sending email: " + ex.Message);
                return;
            }

            Console.WriteLine("Email sccessfully sent");
        }
    }

}