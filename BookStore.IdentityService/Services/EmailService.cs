using BookStore.Core.Helpers;
using System.Net;
using System.Net.Mail;

namespace BookStore.IdentityService.Services;

public class EmailService
{
    SmtpClient _smtpClient;

    public EmailService()
    {
        _smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Port = 587,
            Credentials = new NetworkCredential("Your email", "Your key"),
            EnableSsl = true,
            UseDefaultCredentials = false
        };
    }

    public void SendEmail(string email, string fullname)
    {
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress("avazbekaolimov@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Email Confirmation";
            mail.Body = $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Book Store - Email verification</title>
            </head>
            <body margin-top: 30px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;">
                <div style="display: flex; justify-content: center; width: 100%;">
                    <div style="width: 50%; display: flex; justify-content: center; background-color: white;">
                        <div>
                            <h1>Hello {fullname}!</h1>
                            <h2>Confirm your email address</h2>
                            <div style="display: flex; justify-content: center;">
                                <a href="https://localhost:7281/api/auth/confirm-email/{email}"
                                   style="font-size: 22px; padding: 10px 15px; border-radius: 5px;
                                          background-color: rgb(9, 146, 59); color: white;
                                          margin: 10px; text-decoration: none;
                                ">Confirm email</a>
                            </div>

                            <div style="margin-top: 30px; margin-bottom: 20px; display: flex; justify-content: center;">
                                <span style="color: rgb(134, 134, 134) " >Book Store - 2024</span>
                            </div>
                        </div>
                    </div>
                </div>
            </body>
            </html>            
            """;
            mail.IsBodyHtml = true;

            try
            {
                using (_smtpClient)
                {
                    _smtpClient.Send(mail);
                }
            }
            catch (Exception ex)
            {
                LoggerBot.Log(ex.Message, LogType.Error);
            }
        }
    }
}