using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain;

namespace Backend.Controllers
{
    public class MatchTeamPlayerCardsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: MatchTeamPlayerCards
        public async Task<ActionResult> Index()
        {
            var matchTeamPlayerCards = db.MatchTeamPlayerCards.Include(m => m.CardType).Include(m => m.MatchTeamPlayer);
            return View(await matchTeamPlayerCards.ToListAsync());
        }

        // GET: MatchTeamPlayerCards/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchTeamPlayerCard matchTeamPlayerCard = await db.MatchTeamPlayerCards.FindAsync(id);
            if (matchTeamPlayerCard == null)
            {
                return HttpNotFound();
            }
            return View(matchTeamPlayerCard);
        }

        // GET: MatchTeamPlayerCards/Create
        public ActionResult Create()
        {
            ViewBag.CardTypeId = new SelectList(db.CardTypes, "CardTypeId", "Name");
            ViewBag.MatchTeamPlayerId = new SelectList(db.MatchTeamPlayers, "MatchTeamPlayerId", "MatchTeamPlayerId");
            return View();
        }

        // POST: MatchTeamPlayerCards/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MatchTeamPlayerCardId,Minute,MatchTeamPlayerId,CardTypeId")] MatchTeamPlayerCard matchTeamPlayerCard)
        {
            if (ModelState.IsValid)
            {
                db.MatchTeamPlayerCards.Add(matchTeamPlayerCard);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CardTypeId = new SelectList(db.CardTypes, "CardTypeId", "Name", matchTeamPlayerCard.CardTypeId);
            ViewBag.MatchTeamPlayerId = new SelectList(db.MatchTeamPlayers, "MatchTeamPlayerId", "MatchTeamPlayerId", matchTeamPlayerCard.MatchTeamPlayerId);
            return View(matchTeamPlayerCard);
        }

        // GET: MatchTeamPlayerCards/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchTeamPlayerCard matchTeamPlayerCard = await db.MatchTeamPlayerCards.FindAsync(id);
            if (matchTeamPlayerCard == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardTypeId = new SelectList(db.CardTypes, "CardTypeId", "Name", matchTeamPlayerCard.CardTypeId);
            ViewBag.MatchTeamPlayerId = new SelectList(db.MatchTeamPlayers, "MatchTeamPlayerId", "MatchTeamPlayerId", matchTeamPlayerCard.MatchTeamPlayerId);
            return View(matchTeamPlayerCard);
        }

        // POST: MatchTeamPlayerCards/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MatchTeamPlayerCardId,Minute,MatchTeamPlayerId,CardTypeId")] MatchTeamPlayerCard matchTeamPlayerCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchTeamPlayerCard).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CardTypeId = new SelectList(db.CardTypes, "CardTypeId", "Name", matchTeamPlayerCard.CardTypeId);
            ViewBag.MatchTeamPlayerId = new SelectList(db.MatchTeamPlayers, "MatchTeamPlayerId", "MatchTeamPlayerId", matchTeamPlayerCard.MatchTeamPlayerId);
            return View(matchTeamPlayerCard);
        }

        // GET: MatchTeamPlayerCards/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchTeamPlayerCard matchTeamPlayerCard = await db.MatchTeamPlayerCards.FindAsync(id);
            if (matchTeamPlayerCard == null)
            {
                return HttpNotFound();
            }
            return View(matchTeamPlayerCard);
        }

        // POST: MatchTeamPlayerCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MatchTeamPlayerCard matchTeamPlayerCard = await db.MatchTeamPlayerCards.FindAsync(id);
            db.MatchTeamPlayerCards.Remove(matchTeamPlayerCard);
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
