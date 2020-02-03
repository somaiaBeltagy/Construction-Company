using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    //[Authorize(Roles ="Admin")]
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department).Include(e => e.employee);
            return View(employees.ToList());
        }
        [HttpPost]
        public ActionResult Index(string SearchTerm)
        {
            List<Employee> employees;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                employees = db.Employees.Include(e => e.Department).Include(e => e.employee).ToList();
            }
            else
            {
                employees = db.Employees.Include(e => e.Department).Include(e => e.employee).Where(x => x.FirstName.StartsWith(SearchTerm) || x.LastName.StartsWith(SearchTerm)).ToList();
            }

            //var employees = db.Employees.Include(e => e.Department).Include(e => e.employee);
            return View(employees);
        }

        //Email must be Unique
        public JsonResult IsUserExists(string Email)
        {

            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.Employees.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }

        ////Email must be Unique
        //public JsonResult IsPhoneExists(string Phone)
        //{

        //    //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        //    return Json(!db.Employees.Any(x => x.Phone == Phone), JsonRequestBehavior.AllowGet);
        //}

        // GET: Employees/Details/5
        
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //  Employee employee = db.Employees.Find(id);
            var employee = db.Employees.Include(e => e.Department).Include(e => e.employee).Where(x => x.EmployeeId == id).FirstOrDefault();

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public EmployeesController()
        {
        }

        public EmployeesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model,bool Role)
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
                    if (Role)
                    {                       
                        UserManager.AddToRole(user.Id, "Employee");                        
                    }
                    else
                    {
                        UserManager.AddToRole(user.Id, "Admin");
                    }
                    
                    return RedirectToAction("Create", new { model.Email });
                }
                AddErrors(result);
            }

            return View(model);
        }


        // GET: Employees/Create
        public ActionResult Create(string email)
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName");

            Employee employee = new Employee();
            employee.Email = email;

            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            TempData["Email"] = employee.Email;
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var mail = TempData["Email"].ToString();
                var user = db.Users.Where(x => x.Email == mail).FirstOrDefault();
                user.Email = employee.Email;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = db.Employees.Include(e => e.Department).Include(e => e.employee).Where(x => x.EmployeeId == id).FirstOrDefault();

            if (employee != null)
            {
                var projects = db.EmployeeProjects.Where(x => x.EmployeeId == id);
                foreach (var item in projects)
                {
                    db.EmployeeProjects.Remove(item);
                }

                var allemps = db.Employees.Where(x => x.SuperId == employee.EmployeeId);
                foreach (var item in allemps)
                {
                    item.SuperId = null;
                }
                var user = db.Users.Where(x => x.Email == employee.Email).FirstOrDefault();
                db.Employees.Remove(employee);
                db.Users.Remove(user);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return new HttpNotFoundResult();
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
