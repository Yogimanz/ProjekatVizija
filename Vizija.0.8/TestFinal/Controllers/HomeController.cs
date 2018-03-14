using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestFinal.Models;

namespace TestFinal.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (User.IsInRole(RoleName.Admin))
            {





                return View("HomeAdminIndex");
            }

            else if (User.IsInRole(RoleName.RE) || User.IsInRole(RoleName.RWE))
            {
                return View("Index");
            }



            return View("HomeIndex");
        }
        

        
    }
}