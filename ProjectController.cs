using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMSApplication.Models;
using PMSApplication.ViewModels;


namespace PMSApplication.Controllers
{
    public class ProjectController : Controller
    {
        TestDBEntities db = new TestDBEntities();

        // GET: Project
        public ActionResult Index()
        {
            ProjectViewModel pvm = new ProjectViewModel();
            List<ProjectModel> obj = new List<ProjectModel>();
            var list = db.ProjectMasters.ToList();
            foreach(var x in list)
            {
                ProjectModel p = new ProjectModel();
                p.ProjectId = x.ProjectId;
                p.Name = x.Name;
                p.StartDate = x.StartDate;
                p.EndDate = x.EndDate;
                p.EstimatedHours = x.EstimatedHours;
                p.Skills = x.Skills;
                p.Summary = x.Summary;



                obj.Add(p);


            }

            List<SkillModel> j = new List<SkillModel>();
            var sklist = db.SkillMasters.ToList();
            foreach (var y in sklist)
            {
                SkillModel s = new SkillModel();
                s.SkillId = y.SkillId;
                s.SName = y.SName;
                s.Status = y.Status;
                j.Add(s);

               
            }

            pvm.smlist = j;

            //var cat = db.SkillMasters.ToList();
            //ViewBag.Categories = new MultiSelectList(cat, "SkillId", "SName");


            pvm.pmlist = obj;
            

            return View(pvm);
        }

        

        //Add Data

        public ActionResult Update(ProjectViewModel pvm)
        {
            if(pvm.pm.ProjectId == 0)
            {
                ProjectMaster tbl = new ProjectMaster();
                tbl.Name = pvm.pm.Name;
                tbl.StartDate = pvm.pm.StartDate;
                tbl.EndDate = pvm.pm.EndDate;
                tbl.EstimatedHours = pvm.pm.EstimatedHours;
                tbl.Skills = pvm.pm.Skills;
                tbl.Summary = pvm.pm.Summary;

                db.ProjectMasters.Add(tbl);
                db.SaveChanges();

            }

            else
            {
                var tbl = db.ProjectMasters.Where(m=>m.ProjectId == pvm.pm.ProjectId).FirstOrDefault();
                tbl.Name = pvm.pm.Name;
                tbl.StartDate = pvm.pm.StartDate;
                tbl.EndDate = pvm.pm.EndDate;
                tbl.EstimatedHours = pvm.pm.EstimatedHours;
                tbl.Skills = pvm.pm.Skills;
                tbl.Summary = pvm.pm.Summary;
                db.SaveChanges();

            }


            return RedirectToAction("Index");
        }



        public ActionResult Delete(int id)
        {
            var x = db.ProjectMasters.Find(id);
            db.ProjectMasters.Remove(x);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult GetId(int id)
        {
            ProjectViewModel pvm = new ProjectViewModel();
            ProjectModel m = new ProjectModel();
            var tbl = db.ProjectMasters.Where(u=>u.ProjectId == id).FirstOrDefault();
            m.ProjectId = tbl.ProjectId;
            m.Name = tbl.Name;
            m.StartDate = tbl.StartDate;
            m.EndDate = tbl.EndDate;
            m.EstimatedHours = tbl.EstimatedHours;
            m.Skills = tbl.Skills;
            m.Summary = tbl.Summary;

            pvm.pm = m;

            List<ProjectModel> obj = new List<ProjectModel>();
            var list = db.ProjectMasters.ToList();
            foreach (var x in list)
            {
                ProjectModel p = new ProjectModel();
                p.ProjectId = x.ProjectId;
                p.Name = x.Name;
                p.StartDate = x.StartDate;
                p.EndDate = x.EndDate;
                p.EstimatedHours = x.EstimatedHours;
                p.Skills = x.Skills;
                p.Summary = x.Summary;

                obj.Add(p);


            }

            List<SkillModel> j = new List<SkillModel>();
            var sklist = db.SkillMasters.ToList();
            foreach (var y in sklist)
            {
                SkillModel s = new SkillModel();
                s.SkillId = y.SkillId;
                s.SName = y.SName;
                s.Status = y.Status;
                j.Add(s);


            }

            pvm.smlist = j;

            pvm.pmlist = obj;

            return View("Index",pvm);
           
        }



        public JsonResult GetData()
        {
            List<SkillMaster> allProduct = new List<SkillMaster>();
            using (TestDBEntities db = new TestDBEntities())
            {
                allProduct = db.SkillMasters.ToList();

            }
            return new JsonResult { Data = allProduct, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult demo()
        {
            ProjectViewModel pvm = new ProjectViewModel();
            List<ProjectModel> obj = new List<ProjectModel>();
            var list = db.ProjectMasters.ToList();
            foreach (var x in list)
            {
                ProjectModel p = new ProjectModel();
                p.ProjectId = x.ProjectId;
                p.Name = x.Name;
                p.StartDate = x.StartDate;
                p.EndDate = x.EndDate;
                p.EstimatedHours = x.EstimatedHours;
                p.Skills = x.Skills;
                p.Summary = x.Summary;



                obj.Add(p);


            }

            List<SkillModel> j = new List<SkillModel>();
            var sklist = db.SkillMasters.ToList();
            foreach (var y in sklist)
            {
                SkillModel s = new SkillModel();
                s.SkillId = y.SkillId;
                s.SName = y.SName;
                s.Status = y.Status;
                j.Add(s);


            }

            pvm.smlist = j;

            //var cat = db.SkillMasters.ToList();
            //ViewBag.Categories = new MultiSelectList(cat, "SkillId", "SName");


            pvm.pmlist = obj;


            return View(pvm);
           
        }

    }
}