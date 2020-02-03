using ConstructionSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace ConstructionSystem.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

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
        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            //IdentityRole Employee = new IdentityRole();
            //Employee.Name = "Employee";

            //RoleManager<IdentityRole> roleManager =
            //    new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            //roleManager.Create(Employee);


            //string email = "abdosalem.dev@gmail.com";
            //string Password = "P@ssw0rd";
            //var user = db.Users.Where(x => x.Email == email).FirstOrDefault();
            //if (user == null)
            //{
            //    ApplicationUser applicationUser = new ApplicationUser()
            //    {
            //        Email = email,
            //        UserName = email
            //    };
            //    UserManager.CreateAsync(applicationUser, Password);
            //    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //    UserManager.AddToRole(applicationUser.Id, "Admin");


            //}
            return View();
        }

        public ActionResult About()
        {


            return View();
        }

        public ActionResult Project()
        {


            return View();
        }

        public ActionResult Service()
        {


            return View();
        }
        public ActionResult Team()
        {


            return View();
        }
        public ActionResult Contact()
        {


            return View();
        }

        public ActionResult Login()
        {


            return View();
        }

        //[HttpPost]
        //public ActionResult Login(LoginViewModel model)
        //{
        //    var mail = model.Email;
        //    var pass = model.Password;

        //    return View();
        //}


        public ActionResult Register()
        {


            return View();
        }
    }
}