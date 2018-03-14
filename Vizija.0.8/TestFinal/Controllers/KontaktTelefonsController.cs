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
    public class KontaktTelefonsController : Controller
    {
        private Vizija db = new Vizija();

        // promenljiva koja nam je pomogla da prosledimo pravi ID u Create akciju
        static int pom = 0;
        // GET: KontaktTelefons
        public ActionResult Index(int id)
        {
            var kontaktTelefons = db.KontaktTelefons.Include(k => k.Kontakt).Where(k=> k.IDOsoba == id);
            pom = id;
            ViewBag.Idiot = db.Kontakts.Where(k => k.IDOsoba == id).Select(k => k.IDPreduzeca).FirstOrDefault();


            ViewBag.ImeOsobe = db.Kontakts.Where(k => k.IDOsoba == id).Select(k => k.Ime).FirstOrDefault();
            ViewBag.PrezimeOsobe = db.Kontakts.Where(k => k.IDOsoba == id).Select(k => k.Prezime).FirstOrDefault();

            if (User.IsInRole(RoleName.RE))
                return View("ReadIndex", kontaktTelefons.ToList());



            return View(kontaktTelefons.ToList());
        }

        
        // GET: KontaktTelefons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktTelefon kontaktTelefon = db.KontaktTelefons.Find(id);
            if (kontaktTelefon == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(RoleName.RE))
                return View("ReadDetails", kontaktTelefon);




            return View(kontaktTelefon);
        }

        // GET: KontaktTelefons/Create
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Create(int id)
        {
            ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime");
            ViewBag.Id = db.Kontakts.Where(k => k.IDPreduzeca == id).Select(k => k.IDOsoba).FirstOrDefault();
            return View();
        }

        // POST: KontaktTelefons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTel,IDOsoba,OznakaTelefona,BrojTelefona,Lokal")] KontaktTelefon kontaktTelefon)
        {
            if (ModelState.IsValid)
            {
                var rez = db.Kontakts.Where(k => k.IDOsoba == pom).Select(k => k.IDOsoba).FirstOrDefault();
              
                kontaktTelefon.IDOsoba = rez;
                db.KontaktTelefons.Add(kontaktTelefon);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = kontaktTelefon.IDOsoba});
            }

            ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime", kontaktTelefon.IDOsoba);
            return View(kontaktTelefon);
        }

        // GET: KontaktTelefons/Edit/5
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktTelefon kontaktTelefon = db.KontaktTelefons.Find(id);
            if (kontaktTelefon == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime", kontaktTelefon.IDOsoba);
            return View(kontaktTelefon);
        }

        // POST: KontaktTelefons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDTel,IDOsoba,OznakaTelefona,BrojTelefona,Lokal")] KontaktTelefon kontaktTelefon)
        {
            if (ModelState.IsValid)
            {

                var rez = db.Kontakts.Where(k => k.IDOsoba == pom).Select(k => k.IDOsoba).FirstOrDefault();
                kontaktTelefon.IDOsoba = rez;


                db.Entry(kontaktTelefon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = kontaktTelefon.IDOsoba });
            }
            ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime", kontaktTelefon.IDOsoba);
            return View(kontaktTelefon);
        }

        // GET: KontaktTelefons/Delete/5
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktTelefon kontaktTelefon = db.KontaktTelefons.Find(id);
            if (kontaktTelefon == null)
            {
                return HttpNotFound();
            }
            return View(kontaktTelefon);
        }

        // POST: KontaktTelefons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try { 
            KontaktTelefon kontaktTelefon = db.KontaktTelefons.Find(id);
            db.KontaktTelefons.Remove(kontaktTelefon);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = kontaktTelefon.IDOsoba });
                }
            catch
                {
                return RedirectToAction("NotFound", "Error");
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
