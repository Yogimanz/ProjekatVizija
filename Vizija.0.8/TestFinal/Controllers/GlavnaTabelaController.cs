using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using TestFinal.Models;

namespace TestFinal.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class GlavnaTabelaController :Controller
    {
        static byte[] pom;
        public void InitializeController(RequestContext context)
        {
            base.Initialize(context);
        }
        public GlavnaTabelaController()
        {

            //if (Session["IDUsername"] == null)
            //    RedirectToAction("Login", "Home");

        }
        private Vizija db = new Vizija();

        // GET: GlavnaTabela
        public ActionResult Index()
        {

            if (User.IsInRole(RoleName.Admin) || User.IsInRole(RoleName.RWE))
            {
                return View(db.GlavnaTabelas.ToList());
            }



            return View("ReadIndex",db.GlavnaTabelas.ToList());
        }

        



        // GET: GlavnaTabela/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlavnaTabela glavnaTabela = db.GlavnaTabelas.Find(id);
            if (glavnaTabela == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(RoleName.RE))
                return View("ReadDetails", glavnaTabela);






            return View(glavnaTabela);
        }

        // GET: GlavnaTabela/Create


        [Authorize(Roles =RoleName.RWE+ ","+ RoleName.Admin)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GlavnaTabela/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDPreduzeca,NazivPreduzeca,AdresaRegistracijePreduzeca,Opstina,MaticniBrojPreduzeca,PIB,BrRacuna,WebStranica,Fotografija,Beleska")] GlavnaTabela glavnaTabela,HttpPostedFileBase image1)
        {

            if (ModelState.IsValid)
            {

                if (image1!=null)
                { 
                    
                
                    glavnaTabela.Fotografija = new byte[image1.ContentLength];
                    image1.InputStream.Read(glavnaTabela.Fotografija, 0, image1.ContentLength);

                }


             


                db.GlavnaTabelas.Add(glavnaTabela);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(glavnaTabela);
        }

        // GET: GlavnaTabela/Edit/5
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Edit(int? id)
        {
            


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlavnaTabela glavnaTabela = db.GlavnaTabelas.Find(id);

            pom = glavnaTabela.Fotografija;

           
            if (glavnaTabela == null)
            {
                return HttpNotFound();
            }
            return View(glavnaTabela);
        }

        // POST: GlavnaTabela/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDPreduzeca,NazivPreduzeca,AdresaRegistracijePreduzeca,Opstina,MaticniBrojPreduzeca,PIB,BrRacuna,WebStranica,Fotografija,Beleska")] GlavnaTabela glavnaTabela, HttpPostedFileBase image1)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (image1 != null)
                    {
                        glavnaTabela.Fotografija = new byte[image1.ContentLength];
                        image1.InputStream.Read(glavnaTabela.Fotografija, 0, image1.ContentLength);
                    }
                    else
                    {
                        glavnaTabela.Fotografija = pom;
                    }


                    db.Entry(glavnaTabela).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("NotFound", "Error");
                }
                return RedirectToAction("Index");
            }
            return View(glavnaTabela);
        }

        // GET: GlavnaTabela/Delete/5
        [Authorize(Roles = RoleName.RWE + "," + RoleName.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlavnaTabela glavnaTabela = db.GlavnaTabelas.Find(id);
            if (glavnaTabela == null)
            {
                return HttpNotFound();
            }
            return View(glavnaTabela);
        }

        // POST: GlavnaTabela/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                GlavnaTabela glavnaTabela = db.GlavnaTabelas.Find(id);
                db.GlavnaTabelas.Remove(glavnaTabela);
                db.SaveChanges();
                return RedirectToAction("Index");
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
