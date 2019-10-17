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
    public class CardTypesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: CardTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.CardTypes.ToListAsync());
        }

        // GET: CardTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardType cardType = await db.CardTypes.FindAsync(id);
            if (cardType == null)
            {
                return HttpNotFound();
            }
            return View(cardType);
        }

        // GET: CardTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CardTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CardTypeId,Name")] CardType cardType)
        {
            if (ModelState.IsValid)
            {
                db.CardTypes.Add(cardType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cardType);
        }

        // GET: CardTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardType cardType = await db.CardTypes.FindAsync(id);
            if (cardType == null)
            {
                return HttpNotFound();
            }
            return View(cardType);
        }

        // POST: CardTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CardTypeId,Name")] CardType cardType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cardType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cardType);
        }

        // GET: CardTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardType cardType = await db.CardTypes.FindAsync(id);
            if (cardType == null)
            {
                return HttpNotFound();
            }
            return View(cardType);
        }

        // POST: CardTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CardType cardType = await db.CardTypes.FindAsync(id);
            db.CardTypes.Remove(cardType);
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
