using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestFinal.Models;

namespace TestFinal.Controllers
{
    public class KontaktController : Controller
    {
        private Vizija db = new Vizija();

        
        // GET: Kontakt
        public ActionResult Index(int id)
        {
          
            var kontakts = db.Kontakts.Include(k => k.GlavnaTabela).Where(k=> k.IDPreduzeca == id);
            
            ViewBag.Id = id;
            pom = id;
            
            ViewBag.Pom = db.GlavnaTabelas.Where(k => k.IDPreduzeca == id).Select(k => k.NazivPreduzeca).FirstOrDefault();

            if (User.IsInRole(RoleName.RE))
                return View("ReadIndex", kontakts.ToList());




            return View(kontakts.ToList());
        }

        // GET: Kontakt/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kontakt kontakt = db.Kontakts.Find(id);
            if (kontakt == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(RoleName.RE))
                return View("ReadDetails", kontakt);



            return View(kontakt);
        }

        // GET: Kontakt/Create
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Create(int id)
        {
            ViewBag.IDPreduzeca = new SelectList(db.GlavnaTabelas, "IDPreduzeca", "NazivPreduzeca");
            ViewBag.Id = id;
            return View();
        }

        // POST: Kontakt/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOsoba,IDPreduzeca,Ime,Prezime,RadnoMesto")] Kontakt kontakt, int id)
        {
            if (ModelState.IsValid)
            {
                kontakt.IDPreduzeca = id;
                db.Kontakts.Add(kontakt);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id});
            }

            ViewBag.IDPreduzeca = new SelectList(db.GlavnaTabelas, "IDPreduzeca", "NazivPreduzeca", kontakt.IDPreduzeca);
            return View(kontakt);
        }

        // GET: Kontakt/Edit/5
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kontakt kontakt = db.Kontakts.Find(id);
            if (kontakt == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDPreduzeca = new SelectList(db.GlavnaTabelas, "IDPreduzeca", "NazivPreduzeca", kontakt.IDPreduzeca);
            return View(kontakt);
        }
        static int pom=0;
        // POST: Kontakt/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOsoba,IDPreduzeca,Ime,Prezime,RadnoMesto")] Kontakt kontakt)
        {
            if (ModelState.IsValid)
            {
                var rez = db.Kontakts.Where(k => k.IDPreduzeca == pom).Select(k => k.IDPreduzeca).FirstOrDefault();
                kontakt.IDPreduzeca = rez;

                db.Entry(kontakt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = kontakt.IDPreduzeca });
            }
            ViewBag.IDPreduzeca = new SelectList(db.GlavnaTabelas, "IDPreduzeca", "NazivPreduzeca", kontakt.IDPreduzeca);
            return View(kontakt);
        }

        // GET: Kontakt/Delete/5
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Kontakt kontakt = db.Kontakts.Find(id);
            if (kontakt == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = id;
            return View(kontakt);
        }

        // POST: Kontakt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            Kontakt kontakt = db.Kontakts.Find(id);
            try{
                db.Kontakts.Remove(kontakt);
            }
                catch
            {
               return RedirectToAction("NotFound", "Error");
            }
            //if(kontakt==null)
            //    RedirectToAction("NotFound", "Error");
            db.SaveChanges();
            return RedirectToAction("Index", new { id = kontakt.IDPreduzeca });
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
