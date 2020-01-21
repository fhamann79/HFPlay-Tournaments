using Backend.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Backend.Controllers
{
    
    public class GenericController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        [HttpPost]
        public JsonResult GetPlayers(string prefix)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var users = from u in db.Users
                        select u;
            if (!String.IsNullOrEmpty(prefix))
            {
                users = users.Where(u => u.FirstName.ToUpper().Contains(prefix.ToUpper()) 
                || u.LastName.ToUpper().Contains(prefix.ToUpper())
                || u.IdentificationCard.ToUpper().Contains(prefix.ToUpper())
                || u.Email.ToUpper().Contains(prefix.ToUpper()));
            }
            return Json(users.OrderBy(u => u.LastName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeams(int leagueId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var teams = db.Teams.Where(t => t.LeagueId == leagueId).OrderBy(t => t.Name);
            return Json(teams.OrderBy(t => t.Name), JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetTeamsMatch(int tournamentGroupId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var tournamentTeams = db.TournamentTeams
                .Include(t => t.Team)
                .Where(t => t.TournamentGroupId == tournamentGroupId);
            var teams = ExtractTeams(tournamentTeams);

            return Json(teams.OrderBy(t => t.Name), JsonRequestBehavior.AllowGet);
        }


        private List<Team> ExtractTeams(IQueryable<TournamentTeam> tournamentTeams)
        {
            var teams = new List<Team>();

            foreach (var item in tournamentTeams)
            {
                teams.Add(new Team
                {
                    Name = item.Team.Name,
                    TeamId = item.Team.TeamId
                });
            }
            return teams;
        }


        [Authorize]
        public ActionResult Thumbnail(int width, int height, string path)
        {
            var imageFile = Server.MapPath(path);
            using (var srcImage = Image.FromFile(imageFile))
            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height));
                newImage.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }
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