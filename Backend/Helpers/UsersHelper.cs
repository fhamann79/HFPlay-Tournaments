using Backend.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Backend.Helpers
{
    public class UsersHelper
    {

        private static ApplicationDbContext userContext = new ApplicationDbContext();

        private static DataContextLocal db = new DataContextLocal();

        public static bool DeleteUser(string userName, string rolName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = userManager.FindByEmail(userName);

            if (userASP == null)
            {
                return false;

            }

            var response = userManager.RemoveFromRole(userASP.Id, rolName);

            return response.Succeeded;

        }

        public static bool UpdateUserName(string currentUserName, string newUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(currentUserName);

            if (userASP == null)
            {
                return false;
            }

            userASP.UserName = newUserName;
            userASP.Email = newUserName;
            var response = userManager.Update(userASP);
            return response.Succeeded;
        }

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                CreateUserASP(email, "Admin", password);
                return;

            }

            userManager.AddToRole(userASP.Id, "Admin");
        }
        public static void CreateUserASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userASP, email);
            }
            userManager.AddToRole(userASP.Id, roleName);
        }

        public static void CreateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            
                var userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userASP, password);
                userManager.AddToRole(userASP.Id, roleName);
            
            
            
        }


        public static void AddRoleASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser user = userManager.FindByEmail(email);
            userManager.AddToRole(user.Id, roleName);

        }

        public static string FindId(string email, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            ApplicationUser user = userManager.Find(email, password);

            return user.Id;
           
        }
        public static ApplicationUser FindUserByEmail(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            ApplicationUser user = userManager.FindByEmail(email);

            return user;

        }

        public static ApplicationUser FindUserById(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            ApplicationUser user = userManager.FindById(id);

            return user;
             

        }

        public static IdentityResult DeleteUserAsp(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            ApplicationUser user = userManager.FindById(id);

            var result = userManager.Delete(user);

            return result;

        }

        public static IdentityResult DeleteUserAsp(ApplicationUser user)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var result = userManager.Delete(user);

            return result;

        }

        public static async Task PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return;
            }

            var user = db.Users.Where(tp => tp.Email == email).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            var random = new Random();
            var newPassword = string.Format("{0}{1}{2:04}*",
                user.FirstName.Trim().ToUpper().Substring(0, 1),
                user.LastName.Trim().ToLower(),
                random.Next(10000));

            userManager.RemovePassword(userASP.Id);
            userManager.AddPassword(userASP.Id, newPassword);

            var subject = "Recuperacion de Contraseña - HFPlay";
            var body = string.Format(@"
                <h1>Recuperacion de Contraseña - HFPlay</h1>
                <p>Su nueva contraseña es: <strong>{0}</strong></p>
                <p>Por favor no olvide cambiarla por una de facil recordación.",
                newPassword);

            await MailHelper.SendMail(email, subject, body);
        }

        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}