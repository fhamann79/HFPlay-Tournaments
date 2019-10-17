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
    public class GoalTypesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: GoalTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.GoalTypes.ToListAsync());
        }

        // GET: GoalTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoalType goalType = await db.GoalTypes.FindAsync(id);
            if (goalType == null)
            {
                return HttpNotFound();
            }
            return View(goalType);
        }

        // GET: GoalTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoalTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GoalTypeId,Name")] GoalType goalType)
        {
            if (ModelState.IsValid)
            {
                db.GoalTypes.Add(goalType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(goalType);
        }

        // GET: GoalTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoalType goalType = await db.GoalTypes.FindAsync(id);
            if (goalType == null)
            {
                return HttpNotFound();
            }
            return View(goalType);
        }

        // POST: GoalTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GoalTypeId,Name")] GoalType goalType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goalType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(goalType);
        }

        // GET: GoalTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoalType goalType = await db.GoalTypes.FindAsync(id);
            if (goalType == null)
            {
                return HttpNotFound();
            }
            return View(goalType);
        }

        // POST: GoalTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GoalType goalType = await db.GoalTypes.FindAsync(id);
            db.GoalTypes.Remove(goalType);
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
