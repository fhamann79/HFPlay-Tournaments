using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Conecte su servicio de correo electrónico aquí para enviar correo electrónico.
            await ConfigMailasync(message);
        }

        
        private async Task ConfigMailasync(IdentityMessage message)
        {
            //MAIL HELPER METHOD 1
            var to = message.Destination;
            var subject = message.Subject;
            var body = message.Body;
            await MailHelper.SendMail(to, subject, body);

            //MAIL HELPER METHOD 2
            //var mailMessage = new MailMessage();
            //mailMessage.To.Add(new MailAddress(message.Destination));
            //mailMessage.From = new MailAddress(WebConfigurationManager.AppSettings["SMTPUser"]);
            //mailMessage.Subject = message.Subject;
            //mailMessage.Body = message.Body;
            //mailMessage.IsBodyHtml = true;

            //using (var smtp = new SmtpClient())
            //{
            //    var credential = new NetworkCredential
            //    {
            //        UserName = WebConfigurationManager.AppSettings["SMTPUser"],
            //        Password = WebConfigurationManager.AppSettings["SMTPPassword"]
            //    };

            //    smtp.Credentials = credential;
            //    smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
            //    smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
            //    smtp.EnableSsl = true;
            //    await smtp.SendMailAsync(mailMessage);
            //}



            //SENGRID METHOD 1
            //var apiKey = WebConfigurationManager.AppSettings["HFPlaySendGridApyKey"];
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("fhamann79@hotmail.com", "HFPlay");
            //var subject = message.Subject;
            //var to = new EmailAddress(message.Destination);
            //var plainTextContent = message.Body;
            //var htmlContent = message.Body;
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);

            //SENGRID METHOD 2
            //var myMessage = new SendGridMessage();
            //myMessage.AddTo(message.Destination);
            //myMessage.From = new EmailAddress(
            //                    "fhamann79@hotmail.com", "HFPlay");
            //myMessage.Subject = message.Subject;
            //myMessage.PlainTextContent = message.Body;
            //myMessage.HtmlContent = message.Body;

            //var credentials = new NetworkCredential(
            //           ConfigurationManager.AppSettings["mailAccount"],
            //           ConfigurationManager.AppSettings["mailPassword"]
            //           );

            //// Create a Web transport for sending email.
            //var transportWeb = new System.Web(credentials);

            //// Send the email.
            //if (transportWeb != null)
            //{
            //  await transportWeb.DeliverAsync(myMessage);
            //}
            //else
            //{
            //    Trace.TraceError("Failed to create Web transport.");
            //    await Task.FromResult(0);
            //}
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Conecte el servicio SMS aquí para enviar un mensaje de texto.
            return Task.FromResult(0);
        }
    }

    // Configure el administrador de usuarios de aplicación que se usa en esta aplicación. UserManager se define en ASP.NET Identity y se usa en la aplicación.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure la lógica de validación de nombres de usuario
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure la lógica de validación de contraseñas
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configurar valores predeterminados para bloqueo de usuario
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registre proveedores de autenticación en dos fases. Esta aplicación usa los pasos Teléfono y Correo electrónico para recibir un código para comprobar el usuario
            // Puede escribir su propio proveedor y conectarlo aquí.
            manager.RegisterTwoFactorProvider("Código telefónico", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Su código de seguridad es {0}"
            });
            manager.RegisterTwoFactorProvider("Código de correo electrónico", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Código de seguridad",
                BodyFormat = "Su código de seguridad es {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure el administrador de inicios de sesión que se usa en esta aplicación.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
