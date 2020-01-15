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
    public class LeagueCredentialLogoesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: LeagueCredentialLogoes
        public async Task<ActionResult> Index()
        {
            var leagueCredentialLogoes = db.LeagueCredentialLogoes.Include(l => l.League);
            return View(await leagueCredentialLogoes.ToListAsync());
        }

        // GET: LeagueCredentialLogoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeagueCredentialLogo leagueCredentialLogo = await db.LeagueCredentialLogoes.FindAsync(id);
            if (leagueCredentialLogo == null)
            {
                return HttpNotFound();
            }
            return View(leagueCredentialLogo);
        }

        // GET: LeagueCredentialLogoes/Create
        public ActionResult Create()
        {
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name");
            return View();
        }

        // POST: LeagueCredentialLogoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LeagueCredentialLogoId,Description,LeagueMainLogo,FrontSecondaryLogo,ReverseMainLogo,ReverseSecondaryLogo,MainReverseText,SecondaryReverseText,AlternateReverseText,IsDefault,LeagueId")] LeagueCredentialLogo leagueCredentialLogo)
        {
            if (ModelState.IsValid)
            {
                db.LeagueCredentialLogoes.Add(leagueCredentialLogo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", leagueCredentialLogo.LeagueId);
            return View(leagueCredentialLogo);
        }

        // GET: LeagueCredentialLogoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeagueCredentialLogo leagueCredentialLogo = await db.LeagueCredentialLogoes.FindAsync(id);
            if (leagueCredentialLogo == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", leagueCredentialLogo.LeagueId);
            return View(leagueCredentialLogo);
        }

        // POST: LeagueCredentialLogoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LeagueCredentialLogoId,Description,LeagueMainLogo,FrontSecondaryLogo,ReverseMainLogo,ReverseSecondaryLogo,MainReverseText,SecondaryReverseText,AlternateReverseText,IsDefault,LeagueId")] LeagueCredentialLogo leagueCredentialLogo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leagueCredentialLogo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "LeagueId", "Name", leagueCredentialLogo.LeagueId);
            return View(leagueCredentialLogo);
        }

        // GET: LeagueCredentialLogoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeagueCredentialLogo leagueCredentialLogo = await db.LeagueCredentialLogoes.FindAsync(id);
            if (leagueCredentialLogo == null)
            {
                return HttpNotFound();
            }
            return View(leagueCredentialLogo);
        }

        // POST: LeagueCredentialLogoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LeagueCredentialLogo leagueCredentialLogo = await db.LeagueCredentialLogoes.FindAsync(id);
            db.LeagueCredentialLogoes.Remove(leagueCredentialLogo);
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
