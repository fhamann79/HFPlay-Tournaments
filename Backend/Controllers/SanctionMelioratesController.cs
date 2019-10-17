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
    public class SanctionMelioratesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: SanctionMeliorates
        public async Task<ActionResult> Index()
        {
            var sanctionMeliorates = db.SanctionMeliorates.Include(s => s.TournamentTeam);
            return View(await sanctionMeliorates.ToListAsync());
        }

        // GET: SanctionMeliorates/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanctionMeliorate sanctionMeliorate = await db.SanctionMeliorates.FindAsync(id);
            if (sanctionMeliorate == null)
            {
                return HttpNotFound();
            }
            return View(sanctionMeliorate);
        }

        // GET: SanctionMeliorates/Create
        public ActionResult Create()
        {
            ViewBag.TournamentTeamId = new SelectList(db.TournamentTeams, "TournamentTeamId", "TournamentTeamId");
            return View();
        }

        // POST: SanctionMeliorates/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SanctionMeliorateId,Value,Type,TournamentTeamId")] SanctionMeliorate sanctionMeliorate)
        {
            if (ModelState.IsValid)
            {
                db.SanctionMeliorates.Add(sanctionMeliorate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TournamentTeamId = new SelectList(db.TournamentTeams, "TournamentTeamId", "TournamentTeamId", sanctionMeliorate.TournamentTeamId);
            return View(sanctionMeliorate);
        }

        // GET: SanctionMeliorates/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanctionMeliorate sanctionMeliorate = await db.SanctionMeliorates.FindAsync(id);
            if (sanctionMeliorate == null)
            {
                return HttpNotFound();
            }
            ViewBag.TournamentTeamId = new SelectList(db.TournamentTeams, "TournamentTeamId", "TournamentTeamId", sanctionMeliorate.TournamentTeamId);
            return View(sanctionMeliorate);
        }

        // POST: SanctionMeliorates/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SanctionMeliorateId,Value,Type,TournamentTeamId")] SanctionMeliorate sanctionMeliorate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanctionMeliorate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TournamentTeamId = new SelectList(db.TournamentTeams, "TournamentTeamId", "TournamentTeamId", sanctionMeliorate.TournamentTeamId);
            return View(sanctionMeliorate);
        }

        // GET: SanctionMeliorates/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanctionMeliorate sanctionMeliorate = await db.SanctionMeliorates.FindAsync(id);
            if (sanctionMeliorate == null)
            {
                return HttpNotFound();
            }
            return View(sanctionMeliorate);
        }

        // POST: SanctionMeliorates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SanctionMeliorate sanctionMeliorate = await db.SanctionMeliorates.FindAsync(id);
            db.SanctionMeliorates.Remove(sanctionMeliorate);
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
