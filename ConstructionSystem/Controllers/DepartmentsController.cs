using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionSystem.Models;
using ConstructionSystem.Models.Entities;
using ConstructionSystem.Models.MyViewModels;

namespace ConstructionSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Departments
        public ActionResult Index()
        {
            return View(db.Departments.ToList());
        }



        //=====================================================================     


        //Search
        // POST: Departments
        [HttpPost]
        public ActionResult Index(string SearchTerm)
        {
            List<Department> departments;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                departments = db.Departments.ToList();
            }
            else
            {
                departments = db.Departments.Where(x => x.Name.StartsWith(SearchTerm)).ToList();
            }
            return View(departments);
        }


        //=====================================================================     

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            //var Selects = (from s in db.Departments
            //               join n in db.DepartmentLocations
            //               on s.DepartmentID equals n.DepartmentID
            //               join m in db.DepartmentManagers
            //               on s.DepartmentID equals m.DepartmentID
            //               join z in db.Employees
            //               on m.DepartmentID equals z.DepartmentID
            //               where m.EmployeeId == z.EmployeeId
            //               select new
            //               {
            //                   DepartmentID = s.DepartmentID,
            //                   EmployeeId = z.EmployeeId,
            //                   Name = s.Name,
            //                   NumberOfEmps = db.Employees.GroupBy(x => x.DepartmentID).Count(),
            //                   Location = n.Location,
            //                   StartDate = m.StartDate,
            //                   FirstName = z.FirstName,
            //                   LastName = z.LastName
            //               }).FirstOrDefault();
            //retrieve each item and assign to model

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            else
            {
                DepartmentViewModel model = new DepartmentViewModel()
                {
                    DepartmentID = department.DepartmentID,
                    Name = db.Departments.Where(m => m.DepartmentID == id).Select(x => x.Name).FirstOrDefault(),
                  //  NumberOfEmps = db.Departments.Where(m => m.DepartmentID == id).Select(x => x.NumberOfEmps).FirstOrDefault(),
                    Location = db.DepartmentLocations.Where(m => m.DepartmentID == id).Select(x => x.Location).FirstOrDefault(),
                    StartDate = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.StartDate).FirstOrDefault(),
                    FirstName = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.Employee.FirstName).FirstOrDefault(),
                    LastName = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.Employee.LastName).FirstOrDefault(),
                   EmployeeId = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.EmployeeId).FirstOrDefault()

                };
                return View(model);
            }
        }

        //=====================================================================     


        // GET: Departments/Create
        public ActionResult Create()
        {
            DepartmentViewModel model = new DepartmentViewModel();
            //model.MyList = new SelectList((from s in db.Employees.ToList()
            //                               select new
            //                               {
            //                                   EmplyeeId = s.EmployeeId,
            //                                   FullName = s.FirstName + " " + s.LastName
            //                               }),
            //                      "EmplyeeId",
            //                      "FullName",
            //                          null);
            return View(model);
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentViewModel DeptVM)
        {
            if (ModelState.IsValid)
            {
                Department dept = new Department();
                dept.Name = DeptVM.Name;
                db.Departments.Add(dept);
                db.SaveChanges();

                DepartmentLocation deptLoc = new DepartmentLocation();
                deptLoc.DepartmentID = dept.DepartmentID;
                deptLoc.Location = DeptVM.Location;
                db.DepartmentLocations.Add(deptLoc);
                db.SaveChanges();

                //DepartmentManager deptMan = new DepartmentManager();
                //deptMan.DepartmentID = dept.DepartmentID;
                //deptMan.EmployeeId = DeptVM.EmployeeId;
                //deptMan.StartDate = DeptVM.StartDate;
                //db.DepartmentManagers.Add(deptMan);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(DeptVM);

        }

        //=====================================================================     


        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            Department dept = db.Departments.Find(id);
            DepartmentManager dm = db.DepartmentManagers.Where(x => x.DepartmentID == id).FirstOrDefault();
            DepartmentLocation dl = db.DepartmentLocations.Where(x => x.DepartmentID == id).FirstOrDefault();

            DepartmentViewModel model = new DepartmentViewModel();
            //model.MyList = new SelectList(from s in db.Employees.ToList()
            //                               select new
            //                               {
            //                                   EmplyeeId = s.EmployeeId,
            //                                   FullName = s.FirstName + " " + s.LastName
            //                               },
            //                      "EmplyeeId",
            //                      "FullName",
            //                      new { dm.EmployeeId });


            //var Selects = (from s in db.Departments
            //               join n in db.DepartmentLocations
            //               on s.DepartmentID equals n.DepartmentID
            //               join m in db.DepartmentManagers
            //               on s.DepartmentID equals m.DepartmentID
            //               join z in db.Employees
            //               on m.DepartmentID equals z.DepartmentID
            //               where m.EmployeeId == z.EmployeeId
            //               select new
            //               {
            //                   DepartmentID = s.DepartmentID,
            //                   EmployeeId = z.EmployeeId,
            //                   Name = s.Name,
            //                   NumberOfEmps = s.NumberOfEmps,
            //                   Location = n.Location,
            //                   StartDate = m.StartDate,
            //                   FirstName = z.FirstName,
            //                   LastName = z.LastName
            //               }).FirstOrDefault();


            model.DepartmentID = dept.DepartmentID;
            model.Name = db.Departments.Where(m => m.DepartmentID == id).Select(x => x.Name).FirstOrDefault();
            model.NumberOfEmps = db.Departments.Where(m => m.DepartmentID == id).Select(x => x.NumberOfEmps).FirstOrDefault();
            model.Location = db.DepartmentLocations.Where(m => m.DepartmentID == id).Select(x => x.Location).FirstOrDefault();
            model.StartDate = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.StartDate).FirstOrDefault();
            //model.FirstName = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.Employee.FirstName).FirstOrDefault();
            //model.LastName = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.Employee.LastName).FirstOrDefault();
            //model.EmployeeId = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.EmployeeId).FirstOrDefault();

            //model.DepartmentID = dept.DepartmentID;
            //model.Name = dept.Name;
            //model.Location = dl.Location;
            //model.EmployeeId = dm.EmployeeId;
            //model.StartDate = dm.StartDate;

            TempData["GetLocation"] = model.Location;
            //TempData["EmpID"] = model.EmployeeId;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (dept == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DepartmentViewModel DeptVM)
        {
            //DeptVM.MyList = new SelectList(from s in db.Employees.ToList()
            //                               select new
            //                               {
            //                                   EmplyeeId = s.EmployeeId,
            //                                   FullName = s.FirstName + " " + s.LastName
            //                               },
            //                      "EmplyeeId",
            //                      "FullName",
            //                      new { DeptVM.EmployeeId });
            if (ModelState.IsValid)
            {
                var loc = TempData["GetLocation"].ToString();
               // var emp = int.Parse(TempData["EmpID"].ToString());

                var Dept = db.Departments.Find(id);


                Dept.Name = DeptVM.Name;

                ////var DeptLocation = db.DepartmentLocations.Where(x=>x.DepartmentID == id && x.Location == loc).FirstOrDefault();
                ////DeptLocation.DepartmentID = DeptVM.DepartmentID;
                ////DeptLocation.Location = DeptVM.Location;

                //var DeptManager = db.DepartmentManagers.Where(x=>x.DepartmentID == id && x.EmployeeId == emp).FirstOrDefault();
                //DeptManager.EmployeeId = DeptVM.EmployeeId;
                //DeptManager.StartDate = DeptVM.StartDate;

                //db.Entry(Dept).State = EntityState.Modified;
                //db.Entry(DeptLocation).State = EntityState.Modified;
                //db.Entry(DeptManager).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(DeptVM);
        }


        //=====================================================================     

        // GET: Departments/Delete/5
        //public ActionResult Delete(int? id)
        //{
            //var Selects = (from s in db.Departments
            //               join n in db.DepartmentLocations
            //               on s.DepartmentID equals n.DepartmentID
            //               join m in db.DepartmentManagers
            //               on s.DepartmentID equals m.DepartmentID
            //               join z in db.Employees
            //               on m.DepartmentID equals z.DepartmentID
            //               where m.EmployeeId == z.EmployeeId
            //               select new
            //               {
            //                   DepartmentID = s.DepartmentID,
            //                   EmployeeId = m.EmployeeId,
            //                   Name = s.Name,
            //                   NumberOfEmps = s.NumberOfEmps,
            //                   Location = n.Location,
            //                   StartDate = m.StartDate,
            //                   FirstName = z.FirstName,
            //                   LastName = z.LastName
            //               }).FirstOrDefault();
            //retrieve each item and assign to model

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Department department = db.Departments.Find(id);
        //    if (department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        DepartmentViewModel model = new DepartmentViewModel()
        //        {
        //            DepartmentID = department.DepartmentID,
        //            Name = db.Departments.Where(m => m.DepartmentID == id).Select(x => x.Name).FirstOrDefault(),
        //            NumberOfEmps = db.Departments.Where(m => m.DepartmentID == id).Select(x => x.NumberOfEmps).FirstOrDefault(),
        //            Location = db.DepartmentLocations.Where(m => m.DepartmentID == id).Select(x => x.Location).FirstOrDefault(),
        //            StartDate = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.StartDate).FirstOrDefault(),
        //            FirstName = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.Employee.FirstName).FirstOrDefault(),
        //            LastName = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.Employee.LastName).FirstOrDefault(),
        //            EmployeeId = db.DepartmentManagers.Where(m => m.DepartmentID == id).Select(x => x.EmployeeId).FirstOrDefault()
        //        };
        //        return View(model);


        //    }

        //}

        // GET: Departments/Delete/5
      
        public bool Delete(int? id)
        {

            if (id == null)
            {
                return false;
            }
            else
            {
                Department department = db.Departments.Find(id);
                if (department == null)
                {
                    return false;
                }
                var dm = db.DepartmentManagers.FirstOrDefault(x => x.DepartmentID == id);
                var dl = db.DepartmentLocations.FirstOrDefault(n => n.DepartmentID == id);
                var emp = db.Employees.Where(x => x.DepartmentID == id).ToList();

                foreach (var item in emp)
                {
                    item.DepartmentID = null;
                    db.SaveChanges();
                }

                db.DepartmentManagers.Remove(dm);
                db.DepartmentLocations.Remove(dl);
                db.Departments.Remove(department);


                db.SaveChanges();

                return true;
            }
            
        }


        //=====================================================================     


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

