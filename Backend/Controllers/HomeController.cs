using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Domain;
using PagedList;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    
    public class HomeController : Controller
    {
        private DataContextLocal db = new DataContextLocal();
        public void Logo(LoginViewModel model)
        {
            var user = db.Users.Where(u => u.Email == model.Email).FirstOrDefault();
            if (user != null)
            {
                Session["Logo"] = user.Picture;
            }
        }

        public async Task<ActionResult> DetailsDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var date = await db.Dates.FindAsync(id);

            if (date == null)
            {
                return HttpNotFound();
            }

            return View(date);
        }

        public async Task<ActionResult> DetailsGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tournamentGroups = await db.TournamentGroups.FindAsync(id);

            if (tournamentGroups == null)
            {
                return HttpNotFound();
            }

            return View(tournamentGroups);
        }

        public async Task<ActionResult> DetailsTournament(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = await db.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var model = new LoginViewModel { Email = User.Identity.GetUserName() };
            Logo(model);

            var indexHomeViewModel = new IndexHomeViewModel();
            
            var news = from n in db.NewsItems
                       where n.IsApproved == true
                       select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                news = news.Where(n => n.Content.ToUpper().Contains(searchString.ToUpper())
                       || n.ModificationDate.ToString().ToUpper().Contains(searchString.ToUpper())
                       || n.ModificationDate.ToString().ToUpper().Contains(searchString.ToUpper())
                       || n.Title.ToUpper().Contains(searchString.ToUpper())
                       || n.User.FirstName.ToUpper().Contains(searchString.ToUpper())
                       || n.User.LastName.ToUpper().Contains(searchString.ToUpper())
                       || n.User.Email.ToUpper().Contains(searchString.ToUpper())
                       );
                                      
            }

            news = news.OrderByDescending(n => n.PublicationDate);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            indexHomeViewModel.NewItems = news.ToPagedList(pageNumber, pageSize);
            
            var userASPId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.UserASPId == userASPId).FirstOrDefault();
            indexHomeViewModel.User = user;
            var tournamentTeams = db.TournamentTeams.ToList();
            indexHomeViewModel.TournamentTeams = tournamentTeams;
            var tounaments = db.Tournaments.ToList();
            indexHomeViewModel.Tournaments = tounaments;
            return View(indexHomeViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}