using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TestFinal.Models;

namespace TestFinal.Controllers
{
  
    public class AccountController :Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        ApplicationDbContext context;
        public AccountController()
        {

            context = new ApplicationDbContext();
        }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:

                        
                        return RedirectToLocal(returnUrl);
                    
            
                     
                   
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

   
        [AllowAnonymous]
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Register()
        {

            ViewBag.Name = new SelectList(context.Roles
                                           .ToList(), "Name", "Name");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                   // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                     
                    return RedirectToAction("Index", "Korisnici");
                }
                ViewBag.Name =  new SelectList(context.Roles.ToList(), "Name", "Name");

                
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /////////////////////////////////////

        static string pomocna = "";
          [AllowAnonymous]
        public ActionResult EditRegister(string id)
        {


            ApplicationUser applicationUser = context.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            ViewBag.Name = new SelectList(context.Roles
                                           .ToList(), "Name", "Name");


            var oldUser = UserManager.FindById(applicationUser.Id);
            var oldRoleId = oldUser.Roles.SingleOrDefault().RoleId;
            var oldRoleName = context.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

            //  var rezultat5 = context.Roles.Where(n => n.Id == id).SingleOrDefault(n => n.Name);

            var rezultat2 = context.Users.Where(n => n.Id == id).Select(n => n.UserName).FirstOrDefault();
            var rezultat3 = context.Users.Where(n => n.Id == id).Select(n => n.Email).FirstOrDefault();
            var rezultat4 = context.Users.Where(n => n.Id == id).Select(n => n.PasswordHash).FirstOrDefault();
            pomocna = id;

            ViewBag.Rola = oldRoleName;
            ViewBag.UserName = rezultat2;
            ViewBag.Email = rezultat3;
            ViewBag.Password = rezultat4;

            return View(applicationUser);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult  EditRegister([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser,FormCollection frm)
        {
           


            if (ModelState.IsValid)
            {

                applicationUser.SecurityStamp = Guid.NewGuid().ToString();

                var oldUser = UserManager.FindById(applicationUser.Id);
                var oldRoleId = oldUser.Roles.SingleOrDefault().RoleId;
                var oldRoleName = context.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

                try
                {
                    UserManager.RemoveFromRole(applicationUser.Id, oldRoleName);
                    UserManager.AddToRole(applicationUser.Id, frm[5]);

                }
                catch
                {
                    UserManager.AddToRole(applicationUser.Id, oldRoleName);
                }
                context.Entry(applicationUser).State = EntityState.Modified;

                context.SaveChanges();
                
                return RedirectToAction("Index", "Korisnici");


            }


           return RedirectToAction("Home");
        
        }

         public ActionResult ResetUserPassword(string userId, string UserName)
        {
            ViewBag.Username = UserName.ToString();
            ViewBag.UserId = userId.Trim().ToString();
            return View();
        }


        [HttpPost]
        public ActionResult ResetUserPassword(ResetUserPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                if (userManager.HasPassword(model.UserId))
                {
                    userManager.RemovePassword(model.UserId);
                    userManager.AddPassword(model.UserId, model.ConfirmPassword);

                }

                TempData["Message"] = "Password successfully reset to " + model.ConfirmPassword;
                TempData["MessageValue"] = "1";

                return RedirectToAction("Index","Korisnici");
            }

            // If we got this far, something failed, redisplay form
            TempData["Message"] = "Invalid User Details. Please try again in some minutes ";
            TempData["MessageValue"] = "0";
            return RedirectToAction("Index", "Korisnici");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}