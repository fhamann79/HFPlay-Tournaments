using Backend.Helpers;
using Backend.Models;
using Domain;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using Microsoft.AspNet.Identity;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Backend.Controllers
{
    

    public class LeaguesController : Controller
    {
        #region Properties

        private DataContextLocal db = new DataContextLocal();

        private static ApplicationDbContext userContext = new ApplicationDbContext();

        private int widthPhotoLeague = 360;

        private int heigthPhotoLeague = 360;

        private int widthLeagueMainLogo = 295;

        private int heigthLeagueMainLogo = 86;

        private int widthFrontSecondaryLogo = 295;

        private int heigthFrontSecondaryLogo = 86;

        private int widthReverseMainLogo = 295;

        private int heigthReverseMainLogo = 86;

        private int widthReverseSecondaryLogo = 295;

        private int heigthReverseSecondaryLogo = 86;

        // var userASPId = User.Identity.GetUserId();  OBTIENE ID DE USUARIO ACTUAL 
       
        #endregion


        #region Admin

                [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamAPlayerAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return HttpNotFound();
            }
            return View(teamPlayer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamBPlayerAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return HttpNotFound();
            }
            return View(teamPlayer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamCPlayerAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return HttpNotFound();
            }
            return View(teamPlayer);
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamJPlayerAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return HttpNotFound();
            }
            return View(teamPlayer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePlayerAdmin(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }

            var teamPlayer = await db.TeamPlayers.FindAsync(id);

            return View(teamPlayer);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePlayerAdmin(int id)
        {
            var teamPlayer = await db.TeamPlayers.FindAsync(id);
            try
            {
                db.TeamPlayers.Remove(teamPlayer);
                await db.SaveChangesAsync();

                var players = db.TeamPlayers.Where(tm => tm.UserId == teamPlayer.UserId).ToList();

                if (players.Count == 0)
                {
                    var user = db.Users.Where(u => u.UserId == teamPlayer.UserId).FirstOrDefault();
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                    userManager.RemoveFromRole(user.UserASPId, "Player");
                }

                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeletePlayerAdmin", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("DetailsTeam", new { id = teamPlayer.TeamId });
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTeamManagerAdmin(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }

            var teamManagers = await db.TeamManagers.FindAsync(id);

            return View(teamManagers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTeamManagerAdmin(int id)
        {
            var teamManagers = await db.TeamManagers.FindAsync(id);
            try
            {
                db.TeamManagers.Remove(teamManagers);
                await db.SaveChangesAsync();

                var managers = db.TeamManagers.Where(tm => tm.UserId == teamManagers.UserId).ToList();

                if (managers.Count == 0)
                {
                    var user = db.Users.Where(u => u.UserId == teamManagers.UserId).FirstOrDefault();
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                    userManager.RemoveFromRole(user.UserASPId, "TeamManager");
                }
               
                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeleteTeamManagerAdmin", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("DetailsTeam", new { id = teamManagers.TeamId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteLeagueManagerAdmin(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }

            var leagueManagers = await db.LeagueManagers.FindAsync(id);

            return View(leagueManagers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLeagueManagerAdmin(int id)
        {
            var leagueManagers = await db.LeagueManagers.FindAsync(id);
            try
            {
                db.LeagueManagers.Remove(leagueManagers);
                await db.SaveChangesAsync();

                var managers = db.LeagueManagers.Where(tm => tm.UserId == leagueManagers.UserId).ToList();

                if (managers.Count == 0)
                {
                    var user = db.Users.Where(u => u.UserId == leagueManagers.UserId).FirstOrDefault();
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                    userManager.RemoveFromRole(user.UserASPId, "LeagueManager");
                }

                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeleteLeagueManagerAdmin", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Details", new { id = leagueManagers.LeagueId });
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditPlayerAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var teamPlayer = await db.TeamPlayers.FindAsync(id);

            if (teamPlayer == null)
            {
                return HttpNotFound();
            }

            return View(teamPlayer);


        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPlayerAdmin(TeamPlayer teamPlayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teamPlayer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsTeam", new { id = teamPlayer.TeamId });
            }


            return View(teamPlayer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamC(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamJ(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }



        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddManager(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = await db.Teams.FindAsync(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var teamManager = new TeamManager { TeamId = team.TeamId };

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription");

            return View(teamManager);


        }




        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddManager(TeamManager teamManager)
        {
            if (ModelState.IsValid)
            {
                teamManager.IsAccepted = true;
                teamManager.IsActive = true;
                db.TeamManagers.Add(teamManager);
                await db.SaveChangesAsync();
                var user = db.Users.Where(u => u.UserId == teamManager.UserId).FirstOrDefault();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                userManager.AddToRole(user.UserASPId, "TeamManager");
                return RedirectToAction("DetailsTeam", new { id = teamManager.TeamId });
            }
            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription", teamManager.UserId);
            return View(teamManager);
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddLeagueManager(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var league = await db.Leagues.FindAsync(id);

            if (league == null)
            {
                return HttpNotFound();
            }

            var leagueManager = new LeagueManager { LeagueId = league.LeagueId };

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription");

            return View(leagueManager);


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddLeagueManager(LeagueManager leagueManager)
        {
            if (ModelState.IsValid)
            {
                leagueManager.IsAccepted = true;
                leagueManager.IsActive = true;
                db.LeagueManagers.Add(leagueManager);
                await db.SaveChangesAsync();
                var user = db.Users.Where(u => u.UserId == leagueManager.UserId).FirstOrDefault();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                userManager.AddToRole(user.UserASPId, "LeagueManager");
                return RedirectToAction("Details", new { id = leagueManager.LeagueId });
            }
            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription", leagueManager.UserId);
            return View(leagueManager);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddPlayer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = await db.Teams.FindAsync(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var teamPlayer = new TeamPlayer { TeamId = team.TeamId };

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription");

            return View(teamPlayer);


        }


        public async Task<ActionResult> AddPlayerFindAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = await db.Teams.FindAsync(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var teamPlayer = new TeamPlayer { TeamId = team.TeamId };

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription");

            return View(teamPlayer);


        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPlayer(TeamPlayer teamPlayer)
        {
            if (ModelState.IsValid)
            {
                teamPlayer.IsAccepted = true;
                teamPlayer.IsActive = true;
                db.TeamPlayers.Add(teamPlayer);
                await db.SaveChangesAsync();
                var user = db.Users.Where(u => u.UserId == teamPlayer.UserId).FirstOrDefault();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                userManager.AddToRole(user.UserASPId, "Player");
                return RedirectToAction("DetailsTeam", new { id = teamPlayer.TeamId });
            }

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription", teamPlayer.UserId);
            return View(teamPlayer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DetailsTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Report(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteTeam")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedTeam(int id)
        {
            var team = await db.Teams.FindAsync(id);
            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = team.LeagueId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = await db.Teams.FindAsync(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var view = ToView(team);

            return View(view);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTeam(TeamView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Logo;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FileHelpers.UploadPhoto(view.LogoFile, folder, widthPhotoLeague, heigthPhotoLeague);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var team = ToTeam(view);
                team.Logo = pic;

                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = team.LeagueId });
            }

            return View(view);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTeam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var league = await db.Leagues.FindAsync(id);

            if (league == null)
            {
                return HttpNotFound();
            }

            var view = new TeamView { LeagueId = league.LeagueId, };

            return View(view);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(TeamView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FileHelpers.UploadPhoto(view.LogoFile, folder, widthPhotoLeague, heigthPhotoLeague);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var team = ToTeam(view);
                team.Logo = pic;

                db.Teams.Add(team);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = team.LeagueId });
            }


            return View(view);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Leagues.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeagueView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FileHelpers.UploadPhoto(view.LogoFile, folder, widthPhotoLeague, heigthPhotoLeague);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var league = ToLeague(view);
                league.Logo = pic;
                db.Leagues.Add(league);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var league = await db.Leagues.FindAsync(id);

            if (league == null)
            {
                return HttpNotFound();
            }

            var view = ToView(league);

            return View(view);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeagueView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Logo;
                var folder = "~/Content/Logos";

                if (view.LogoFile != null)
                {
                    pic = FileHelpers.UploadPhoto(view.LogoFile, folder, widthPhotoLeague, heigthPhotoLeague);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var league = ToLeague(view);
                league.Logo = pic;

                db.Entry(league).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(view);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            League league = await db.Leagues.FindAsync(id);
            db.Leagues.Remove(league);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        #endregion


        #region LeagueManager

        [Authorize(Roles = "LeagueManager")]
        public async Task<ActionResult> DetailsTeamLeagueManager(int? id)
        {
            var userASPId = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var teamView = await db.Teams.FindAsync(id);

            if (teamView == null)
            {
                return HttpNotFound();
            }

            var isLeagueManager = CheckManagerHelper.CheckLeagueManager(userASPId, teamView.LeagueId);

            if (isLeagueManager.Response == false)
            {
                return HttpNotFound();
            }

            return View(teamView);

        }

        [Authorize(Roles = "LeagueManager")]
        public ActionResult SetupCardLeagueLeagueManager(int? id)
        {
            var userASPId = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var isLeagueManager = CheckManagerHelper.CheckLeagueManager(userASPId, id.Value);

            if (isLeagueManager.Response == false)
            {
                return HttpNotFound();
            }

            var leagueView = db.Leagues.Where(l => l.LeagueId == id).FirstOrDefault();

            return View(leagueView);
        }

        [Authorize(Roles = "LeagueManager")]
        public async Task<ActionResult> CreateLeagueCredentialLogo(int? id)
        {
            //TODO: Control access LeagueManager

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var league = await db.Leagues.FindAsync(id);

            if (league == null)
            {
                return HttpNotFound();
            }

            var view = new LeagueCredentialLogoView { LeagueId = league.LeagueId, IsDefault = true };

            return View(view);
        }

        [Authorize(Roles = "LeagueManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLeagueCredentialLogo(LeagueCredentialLogoView view)
        {
            if (ModelState.IsValid)
            {
                var picLeagueMainLogoView = string.Empty;
                var picFrontSecondaryLogoView = string.Empty;
                var picReverseMainLogoView = string.Empty;
                var picReverseSecondaryLogoView = string.Empty;
                var folder = "~/Content/Credentials";

                if (view.LeagueMainLogoView != null)
                {
                    picLeagueMainLogoView = FileHelpers.UploadPhoto(view.LeagueMainLogoView, folder, widthLeagueMainLogo, heigthLeagueMainLogo);
                    picLeagueMainLogoView = string.Format("{0}/{1}", folder, picLeagueMainLogoView);
                }

                if (view.FrontSecondaryLogoView != null)
                {
                    picFrontSecondaryLogoView = FileHelpers.UploadPhoto(view.FrontSecondaryLogoView, folder, widthFrontSecondaryLogo, heigthFrontSecondaryLogo);
                    picFrontSecondaryLogoView = string.Format("{0}/{1}", folder, picFrontSecondaryLogoView);
                }

                if (view.ReverseMainLogoView != null)
                {
                    picReverseMainLogoView = FileHelpers.UploadPhoto(view.ReverseMainLogoView, folder, widthReverseMainLogo, heigthReverseMainLogo);
                    picReverseMainLogoView = string.Format("{0}/{1}", folder, picReverseMainLogoView);
                }

                if (view.ReverseSecondaryLogoView != null)
                {
                    picReverseSecondaryLogoView = FileHelpers.UploadPhoto(view.ReverseSecondaryLogoView, folder, widthReverseSecondaryLogo, heigthReverseSecondaryLogo);
                    picReverseSecondaryLogoView = string.Format("{0}/{1}", folder, picReverseSecondaryLogoView);
                }

                var leagueCredencialLogo = ToLeagueCredencialLogo(view);
                leagueCredencialLogo.LeagueMainLogo = picLeagueMainLogoView;
                leagueCredencialLogo.FrontSecondaryLogo = picFrontSecondaryLogoView;
                leagueCredencialLogo.ReverseMainLogo = picReverseMainLogoView;
                leagueCredencialLogo.ReverseSecondaryLogo = picReverseSecondaryLogoView;

                db.LeagueCredentialLogoes.Add(leagueCredencialLogo);
                await db.SaveChangesAsync();
                return RedirectToAction("SetupCardLeagueLeagueManager", new { id = leagueCredencialLogo.LeagueId });
            }


            return View(view);
        }



        [Authorize(Roles = "LeagueManager")]
        public ActionResult IndexLeaguesLeagueManager()
        {
            var userASPId = User.Identity.GetUserId();
            var qry = (from l in db.Leagues
                       join lm in db.LeagueManagers on l.LeagueId equals lm.LeagueId
                       join u in db.Users on lm.UserId equals u.UserId
                       where u.UserASPId == userASPId
                       select new { l }).ToList();

            var leagues = new List<League>();

            foreach (var item in qry)
            {
                leagues.Add(item.l);
            }

            return View(leagues);


        }

        [Authorize(Roles = "LeagueManager")]
        public ActionResult DetailsLeaguesLeagueManager(int? id)
        {
            var userASPId = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var isLeagueManager = CheckManagerHelper.CheckLeagueManager(userASPId, id.Value);

            if (isLeagueManager.Response == false)
            {
                return HttpNotFound();
            }

            var leagueView = isLeagueManager.League;

            return View(leagueView);

         }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CardsTeamLeagueManager(int? id)
        {
            //TODO: LeagueManager ACCESS

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }

            var league = await db.Leagues.FindAsync(team.LeagueId);

            var leagueCredentialLogo = db.LeagueCredentialLogoes
                .Where(c => c.LeagueId == league.LeagueId || c.IsDefault == true)
                .FirstOrDefault();

            var credentialView = new CredentialView();
            credentialView.Team = team;
            credentialView.LeagueCredentialLogo = leagueCredentialLogo;

            return View(credentialView);
        }


        #endregion


        #region TeamManager

        [Authorize(Roles = "TeamManager")]
        public async Task<ActionResult> DeletePlayerTeamManager(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = MessageHelper.DeleteError();
            }

            var teamPlayer = await db.TeamPlayers.FindAsync(id);

            var userASPId = User.Identity.GetUserId();

            var user = db.Users.Where(u => u.UserASPId == userASPId).FirstOrDefault();

            var qry = (from tm in db.TeamManagers
                       where tm.UserId == user.UserId
                       join te in db.Teams on tm.TeamId equals te.TeamId
                       where te.TeamId == teamPlayer.TeamId
                       select new { tm }).FirstOrDefault();

            if (qry == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(teamPlayer);
        }

        [Authorize(Roles = "TeamManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePlayerTeamManager(int id)
        {
            var teamPlayer = await db.TeamPlayers.FindAsync(id);
            try
            {
                db.TeamPlayers.Remove(teamPlayer);
                await db.SaveChangesAsync();

                var players = db.TeamPlayers.Where(tm => tm.UserId == teamPlayer.UserId).ToList();

                if (players.Count == 0)
                {
                    var user = db.Users.Where(u => u.UserId == teamPlayer.UserId).FirstOrDefault();
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                    userManager.RemoveFromRole(user.UserASPId, "Player");
                }

                TempData["SuccessMessage"] = MessageHelper.DeleteOk();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("DeletePlayerTeamManager", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("DetailsTeamTeamManager", new { id = teamPlayer.TeamId });
        }

        [Authorize(Roles = "TeamManager")]
        public async Task<ActionResult> EditPlayerTeamManager(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var teamPlayer = await db.TeamPlayers.FindAsync(id);

            if (teamPlayer == null)
            {
                return HttpNotFound();
            }


            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription", teamPlayer.UserId);

            return View(teamPlayer);


        }

        [Authorize(Roles = "TeamManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPlayerTeamManager(TeamPlayer teamPlayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teamPlayer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsTeamTeamManager", new { id = teamPlayer.TeamId });
            }

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription", teamPlayer.UserId);
            return View(teamPlayer);
        }

        [Authorize(Roles = "TeamManager")]
        public async Task<ActionResult> AddPlayerTeamManager(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var team = await db.Teams.FindAsync(id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var teamPlayer = new TeamPlayer { TeamId = team.TeamId };

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription");

            return View(teamPlayer);


        }

        [Authorize(Roles = "TeamManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPlayerTeamManager(TeamPlayer teamPlayer)
        {
            if (ModelState.IsValid)
            {
                teamPlayer.IsAccepted = true;
                teamPlayer.IsActive = true;
                db.TeamPlayers.Add(teamPlayer);
                await db.SaveChangesAsync();
                var user = db.Users.Where(u => u.UserId == teamPlayer.UserId).FirstOrDefault();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                userManager.AddToRole(user.UserASPId, "Player");
                TempData["SuccessMessage"] = MessageHelper.AddOk();
                return RedirectToAction("DetailsTeamTeamManager", new { id = teamPlayer.TeamId });
            }

            ViewBag.UserId = new SelectList(db.Users.OrderBy(u => u.LastName).ToList(), "UserId", "FullDescription", teamPlayer.UserId);
            return View(teamPlayer);
        }

        [Authorize(Roles = "TeamManager")]
        public async Task<ActionResult> DetailsTeamTeamManager(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var userASPId = User.Identity.GetUserId();

                var qry = (from te in db.Teams
                           where te.TeamId == id
                           join tm in db.TeamManagers on te.TeamId equals tm.TeamId
                           join um in db.Users on tm.UserId equals um.UserId
                           where um.UserASPId == userASPId
                           select new { te }).FirstOrDefault();

                var team = await db.Teams.FindAsync(qry.te.TeamId);

                if (team == null)
                {
                    return HttpNotFound();
                }

                return View(team);
            }
            catch (NullReferenceException)
            {
                TempData["FailMessage"] = MessageHelper.TeamFail();
                //TODO: Make Send Email to User
                return RedirectToAction("Index", "Home", new { });
            }


        }

         [Authorize(Roles = "TeamManager")]
        public async Task<ActionResult> DetailsLeaguesTeamManager(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = await db.Leagues.FindAsync(id);

            if (league == null)
            {
                return HttpNotFound();
            }

            League leagueView = new League
            {
                LeagueId = league.LeagueId,
                Logo = league.Logo,
                Name = league.Name,
                Teams = new List<Team>(),
            };

            var userASPId = User.Identity.GetUserId();

            var qry = (from t in db.Teams
                       where t.LeagueId == id
                       join tm in db.TeamManagers on t.TeamId equals tm.TeamId
                       join u in db.Users on tm.UserId equals u.UserId
                       where u.UserASPId == userASPId
                       select new { t }).ToList();

            foreach (var item in qry)
            {
                leagueView.Teams.Add(item.t);
            }

            return View(leagueView);

        }

        [Authorize(Roles = "TeamManager")]
        public ActionResult IndexLeaguesTeamManager()
        {
            var userASPId = User.Identity.GetUserId();
            var qry = (from l in db.Leagues
                       join t in db.Teams on l.LeagueId equals t.LeagueId
                       join tm in db.TeamManagers on t.TeamId equals tm.TeamId
                       join u in db.Users on tm.UserId equals u.UserId
                       where u.UserASPId == userASPId
                       select new { l }).ToList();

            var leagues = new List<League>();

            foreach (var item in qry)
            {
                var league = leagues.Find(lg => lg.LeagueId == item.l.LeagueId);

                if (league == null)
                {
                    leagues.Add(item.l);
                }
              
            }

            return View(leagues);


        }





        #endregion


        #region Player
        //TODO: Give player role privileges to all and change permissions to the method
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DetailsTeamPlayer(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var userASPId = User.Identity.GetUserId();

                var qry = (from te in db.Teams
                           where te.TeamId == id
                           join tp in db.TeamPlayers on te.TeamId equals tp.TeamId
                           join um in db.Users on tp.UserId equals um.UserId
                           where um.UserASPId == userASPId
                           select new { te }).FirstOrDefault();

                var team = await db.Teams.FindAsync(qry.te.TeamId);

                if (team == null)
                {
                    return HttpNotFound();
                }

                return View(team);
            }
            catch (NullReferenceException)
            {
                TempData["FailMessage"] = MessageHelper.TeamFail();
                //TODO: Make Send Email to User
                return RedirectToAction("Index", "Home", new { });
            }

        }
        #endregion


        #region Methods

        private LeagueCredentialLogo ToLeagueCredencialLogo(LeagueCredentialLogoView view)
        {
            return new LeagueCredentialLogo
            {
                AlternateReverseText = view.AlternateReverseText,
                Description = view.Description,
                FrontSecondaryLogo = view.FrontSecondaryLogo,
                IsDefault = view.IsDefault,
                League = view.League,
                LeagueCredentialLogoId = view.LeagueCredentialLogoId,
                LeagueId = view.LeagueId,
                LeagueMainLogo = view.LeagueMainLogo,
                MainReverseText = view.MainReverseText,
                ReverseMainLogo = view.ReverseMainLogo,
                ReverseSecondaryLogo = view.ReverseSecondaryLogo,
                SecondaryReverseText = view.SecondaryReverseText,

            };
        }
        private Team ToTeam(TeamView view)
        {
            return new Team
            {
                Initials = view.Initials,
                League = view.League,
                LeagueId = view.LeagueId,
                Logo = view.Logo,
                Name = view.Name,
                TeamId = view.TeamId,
            };
        }

        private TeamView ToView(Team team)
        {
            return new TeamView
            {
                TeamId = team.TeamId,
                Name = team.Name,
                Initials = team.Initials,
                League = team.League,
                LeagueId = team.LeagueId,
                Logo = team.Logo,

            };
        }

        private League ToLeague(LeagueView view)
        {
            return new League
            {
                LeagueId = view.LeagueId,
                Logo = view.Logo,
                Name = view.Name,
                Teams = view.Teams,
            };
        }

        private LeagueView ToView(League league)
        {
            return new LeagueView
            {
                LeagueId = league.LeagueId,
                Logo = league.Logo,
                Name = league.Name,
                Teams = league.Teams,

            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        

    }
}
