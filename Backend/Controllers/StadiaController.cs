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
    [Authorize(Roles = "Admin")]
    public class StadiaController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Stadia
        public async Task<ActionResult> Index()
        {
            return View(await db.Stadia.ToListAsync());
        }

        // GET: Stadia/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stadium stadium = await db.Stadia.FindAsync(id);
            if (stadium == null)
            {
                return HttpNotFound();
            }
            return View(stadium);
        }

        // GET: Stadia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stadia/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StadiumId,Name")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                db.Stadia.Add(stadium);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(stadium);
        }

        // GET: Stadia/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stadium stadium = await db.Stadia.FindAsync(id);
            if (stadium == null)
            {
                return HttpNotFound();
            }
            return View(stadium);
        }

        // POST: Stadia/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StadiumId,Name")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stadium).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stadium);
        }

        // GET: Stadia/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stadium stadium = await db.Stadia.FindAsync(id);
            if (stadium == null)
            {
                return HttpNotFound();
            }
            return View(stadium);
        }

        // POST: Stadia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Stadium stadium = await db.Stadia.FindAsync(id);
            db.Stadia.Remove(stadium);
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
