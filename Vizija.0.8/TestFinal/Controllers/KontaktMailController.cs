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
    public class KontaktMailController :Controller
    {
        private Vizija db = new Vizija();
        static int pom = 0;
        // GET: KontaktMail
        public ActionResult Index(int id)
        {

            pom = id;
            var kontaktss = db.KontaktMails.Include(k => k.Kontakt).Where(k => k.IDOsoba == id);

            ViewBag.Id = db.Kontakts.Where(k => k.IDOsoba == id).Select(k => k.IDPreduzeca).FirstOrDefault();


            ViewBag.ImeOsobe = db.Kontakts.Where(k => k.IDOsoba == id).Select(k => k.Ime).FirstOrDefault();


            ViewBag.PrezimeOsobe = db.Kontakts.Where(k => k.IDOsoba == id).Select(k => k.Prezime).FirstOrDefault();



            if (User.IsInRole(RoleName.RE))
                return View("ReadIndex", kontaktss.ToList());




            return View(kontaktss.ToList());
        }

        // GET: KontaktMail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktMail kontaktMail = db.KontaktMails.Find(id);
            if (kontaktMail == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(RoleName.RE))
                return View("ReadDetails", kontaktMail);




            return View(kontaktMail);
        }

        // GET: KontaktMail/Create

        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Create(int id)
        {
            ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime");
            ViewBag.Id = db.Kontakts.Where(k => k.IDPreduzeca == id).Select(k => k.IDOsoba).FirstOrDefault();
            return View();
        }

        // POST: KontaktMail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDMail,IDOsoba,OznakaPosla,Adresa")] KontaktMail kontaktMail)
        {
            if (ModelState.IsValid)
            {
                var rez = db.Kontakts.Where(k => k.IDOsoba == pom).Select(k => k.IDOsoba).First();
                kontaktMail.IDOsoba = rez;
                db.KontaktMails.Add(kontaktMail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = kontaktMail.IDOsoba });
            }

            ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime", kontaktMail.IDOsoba);
            return View(kontaktMail);
        }

        // GET: KontaktMail/Edit/5

        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktMail kontaktMail = db.KontaktMails.Find(id);
            if (kontaktMail == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime", kontaktMail.IDOsoba);
           
            return View(kontaktMail);
        }

        // POST: KontaktMail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDMail,IDOsoba,OznakaPosla,Adresa")] KontaktMail kontaktMail)
        {
            if (ModelState.IsValid)
            {
                var rez = db.Kontakts.Where(k => k.IDOsoba == pom).Select(k => k.IDOsoba).FirstOrDefault();
                kontaktMail.IDOsoba = rez;
                db.Entry(kontaktMail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = kontaktMail.IDOsoba });
            }

            

           ViewBag.IDOsoba = new SelectList(db.Kontakts, "IDOsoba", "Ime", kontaktMail.IDOsoba);
            return View(kontaktMail);
        }

        // GET: KontaktMail/Delete/5

        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KontaktMail kontaktMail = db.KontaktMails.Find(id);
            if (kontaktMail == null)
            {
                return HttpNotFound();
            }
            return View(kontaktMail);
        }

        // POST: KontaktMail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                KontaktMail kontaktMail = db.KontaktMails.Find(id);
                db.KontaktMails.Remove(kontaktMail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = kontaktMail.IDOsoba });

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
