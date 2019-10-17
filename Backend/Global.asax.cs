using Backend.Helpers;
using Backend.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Backend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.DataContextLocal, Migrations.Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbInterception.Add(new HFPlayBackInterceptorTransientErrors());
            DbInterception.Add(new HFPlayBackInterceptorLogging());
            CheckRolAndSuperUser();
        }

        private void CheckRolAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");
            UsersHelper.CheckRole("TeamManager");
            UsersHelper.CheckRole("LeagueManager");
            UsersHelper.CheckRole("TournamentManager");
            UsersHelper.CheckRole("Player");
            UsersHelper.CheckRole("Anonimous");
            UsersHelper.CheckRole("Referee");
            UsersHelper.CheckSuperUser();
        }


        


    }
}
