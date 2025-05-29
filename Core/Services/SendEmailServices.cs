using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.Settings;
using Microsoft.Extensions.Options;
using Core.Interfaces;
using Core.Model;

namespace Core.Services
{
    public class SendEmailServices
    {
        private readonly EmailConfgSettings email_confg;
        private readonly IUnitOfWork<CodeVerification> codeUnitOfWork;
        private readonly CodeDatabaseServices databaseServices;

        public SendEmailServices(IOptions<EmailConfgSettings> options, IUnitOfWork<CodeVerification> CodeUnitOfWork, CodeDatabaseServices databaseServices)
        {
            email_confg = options.Value;
            codeUnitOfWork = CodeUnitOfWork;
            this.databaseServices = databaseServices;
        }



        public async Task SendEmail(string Email)
        {
            try
            {

                var email = email_confg.Email;
                var password = email_confg.Password;
                var host = email_confg.Host;
                var port = email_confg.Port;


                var smtpClient = new SmtpClient(host, port);
                smtpClient.EnableSsl = true;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Credentials = new NetworkCredential(email, password);


                string verificationCode;


                if (await codeUnitOfWork.Entity.Any(x => x.Email == Email))
                {
                    verificationCode = codeUnitOfWork.Entity.Find(x => x.Email == Email).Code;
                }
                else
                {
                    verificationCode = GenerateCodeVerify.GenerateCode(6);

                    await databaseServices.SaveCode(verificationCode, Email);

                }

                // Generate a verification code


                var message = new MailMessage(email!, Email, "Verification", $"Verification Code: {verificationCode} , Please enter this code to verify your identity. Do not share it with anyone.");

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the exception (to file, console, or database)
                Console.WriteLine(ex.Message);
                throw; // or handle accordingly
            }


        }
    }
}
