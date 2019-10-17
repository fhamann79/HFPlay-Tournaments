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
    public class SanctionsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Sanctions
        public async Task<ActionResult> Index()
        {
            return View(await db.Sanctions.ToListAsync());
        }

        // GET: Sanctions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanction sanction = await db.Sanctions.FindAsync(id);
            if (sanction == null)
            {
                return HttpNotFound();
            }
            return View(sanction);
        }

        // GET: Sanctions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sanctions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SanctionId,Description,PenaltyFee,NumberOfMatchs,NumberOfMonths,NumberOfYears")] Sanction sanction)
        {
            if (ModelState.IsValid)
            {
                db.Sanctions.Add(sanction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sanction);
        }

        // GET: Sanctions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanction sanction = await db.Sanctions.FindAsync(id);
            if (sanction == null)
            {
                return HttpNotFound();
            }
            return View(sanction);
        }

        // POST: Sanctions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SanctionId,Description,PenaltyFee,NumberOfMatchs,NumberOfMonths,NumberOfYears")] Sanction sanction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sanction);
        }

        // GET: Sanctions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanction sanction = await db.Sanctions.FindAsync(id);
            if (sanction == null)
            {
                return HttpNotFound();
            }
            return View(sanction);
        }

        // POST: Sanctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sanction sanction = await db.Sanctions.FindAsync(id);
            db.Sanctions.Remove(sanction);
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
