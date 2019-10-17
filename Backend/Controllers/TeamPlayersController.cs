using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Backend.Models;
using Domain;

namespace Backend.Controllers
{
    public class TeamPlayersController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: TeamPlayers
        public async Task<ActionResult> Index()
        {
            var teamPlayers = db.TeamPlayers.Include(t => t.Team).Include(t => t.User);
            return View(await teamPlayers.ToListAsync());
        }

        // GET: TeamPlayers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamPlayer teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return HttpNotFound();
            }
            return View(teamPlayer);
        }

        // GET: TeamPlayers/Create
        public ActionResult Create()
        {
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: TeamPlayers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TeamPlayerId,Number,UserId,TeamId,IsAccepted")] TeamPlayer teamPlayer)
        {
            if (ModelState.IsValid)
            {
                db.TeamPlayers.Add(teamPlayer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", teamPlayer.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", teamPlayer.UserId);
            return View(teamPlayer);
        }

        // GET: TeamPlayers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamPlayer teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", teamPlayer.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", teamPlayer.UserId);
            return View(teamPlayer);
        }

        // POST: TeamPlayers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TeamPlayerId,Number,UserId,TeamId,IsAccepted")] TeamPlayer teamPlayer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teamPlayer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TeamId = new SelectList(db.Teams, "TeamId", "Name", teamPlayer.TeamId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", teamPlayer.UserId);
            return View(teamPlayer);
        }

        // GET: TeamPlayers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeamPlayer teamPlayer = await db.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return HttpNotFound();
            }
            return View(teamPlayer);
        }

        // POST: TeamPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TeamPlayer teamPlayer = await db.TeamPlayers.FindAsync(id);
            db.TeamPlayers.Remove(teamPlayer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
