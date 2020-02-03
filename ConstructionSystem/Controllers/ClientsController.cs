using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ConstructionSystem.Models;
using ConstructionSystem.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ConstructionSystem.Controllers
{

    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ClientsController()
        {
        }

        public ClientsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    var _context = new ApplicationDbContext();
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                    UserManager.AddToRole(user.Id, "Client");
                    return RedirectToAction("Create",new { model.Email });
                }
                AddErrors(result);
            }
            
            return View(model);
        }

        [HttpPost]
        public JsonResult IsEmailExist(string mail)
        {
            var mailinvalid = !db.Users.Any(x => x.Email == mail);
            return Json(mailinvalid);
        } 

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create(string Email)
        {
            Client client = new Client();
            client.Email = Email;
            return View(client);
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Client client)
        {
            if (ModelState.IsValid)
            {
                db.Cliens.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(client);
        }


        public ActionResult Index()
        {
            return View(db.Cliens.ToList());
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            List<Client> clients;
            if (string.IsNullOrEmpty(Search))
            {
                clients = db.Cliens.ToList();
            }
            else
            {
                clients = db.Cliens.Where(a => a.FirstName.StartsWith(Search)).ToList();
            }
            return View(clients);
        }

        public JsonResult getSearch(string term)
        {
            List<string> Clients;
            Clients = db.Cliens.Where(a => a.FirstName.StartsWith(term)).Select(s => s.FirstName).ToList();
            return Json(Clients, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Cliens.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }


        [HttpGet]
        [Authorize(Roles ="Client")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Cliens.Find(id);
            TempData["Email"] = client.Email;
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public ActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                var mail = TempData["Email"].ToString();
                var user = db.Users.Where(x => x.Email == mail).FirstOrDefault();
                user.Email = client.Email;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        
        public bool Delete(int id)
        {
            Client client = db.Cliens.Find(id);
            if (client == null)
            {
                return false;
            }
            else
            {
                var user = db.Users.Where(x => x.Email == client.Email).FirstOrDefault();
                db.Cliens.Remove(client);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
        }

    }
}
