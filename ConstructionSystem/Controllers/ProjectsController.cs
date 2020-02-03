using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionSystem.Models;
using ConstructionSystem.Models.Entities;

namespace ConstructionSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // View All project
        //GET: Projects
        public ActionResult Project_Details()
        {
            return View(db.Projects.ToList());
        }

        // view projects by search 
        [HttpPost]
        public ActionResult Project_Details(string Search)
        {
            List<Project> projects;
            if (string.IsNullOrEmpty(Search))
            {
                projects = db.Projects.ToList();
            }
            else
            {
                projects= db.Projects.Where(a => a.Name.StartsWith(Search)).ToList();
            }
            return View(projects);
        }


        // View details of one project 
        // GET: Projects/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }


        // Create Project
        // GET: Projects/Create_Ptoject
        public ActionResult Create_Ptoject()
        {
            return View();
        }

        // POST: Projects/Create_Ptoject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Ptoject(Project project)
        {

            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Project_Details");
            }
            return View(project);
        }



        // Edit project 
        // GET: Projects/Edit/5
        public ActionResult Edit_Project(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Project(Project project)
        {
            if (ModelState.IsValid)
            {
                var p = db.Projects.Where(a => a.ProjectId == project.ProjectId).FirstOrDefault();
                p.Name = project.Name;
                p.Location = project.Location;
                p.Description = project.Description;
                p.StartDate = project.StartDate;
                p.ExpectedPeriod = project.ExpectedPeriod;
                db.SaveChanges();
                return RedirectToAction("Project_Details");
            }
            return View(project);
        }


        // Show detail of project before delete it 
        // GET: Projects/Delete/5
        public ActionResult Delete_Project(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete_Project")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var ps = db.ProjectServices.Where(a => a.ProjectID == id);
            foreach (var item in ps)
            {
                db.ProjectServices.Remove(item);
            }
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Project_Details");
        }


        // delete by using ajax
        public bool Delete(int id)
        {
            var ps = db.ProjectServices.Where(a => a.ProjectID == id);
            foreach (var item in ps)
            {
                db.ProjectServices.Remove(item);
            }
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return true;
        }


        // Add servie to project
        [HttpGet]
        public ActionResult AssignServicToProject()
        {
            ViewBag.proj_list = new SelectList(db.Projects, "ProjectID", "Name");
            ViewBag.serv_list = new SelectList(db.Services, "ServiceID", "ServiceName");
            return View();
        }
        [HttpPost]
        public ActionResult AssignServicToProject(ProjectService model)
        {
            //ProjectService ps = new ProjectService();
            if (ModelState.IsValid)
            {
                //ps.ProjectID = model.ProjectID;
                //ps.ServiceID = model.ServiceID;
                db.ProjectServices.Add(model);
                db.SaveChanges();
                return RedirectToAction("Project_Details");
            }
            return View(model);
        }


        // search auto comblete 
        public JsonResult getSearch(string term)
        {
            List<string> projects;
            projects = db.Projects.Where(a => a.Name.StartsWith(term)).Select(s => s.Name).ToList();
            return Json(projects, JsonRequestBehavior.AllowGet);
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
