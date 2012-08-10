using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class JobsController : AuthController {

        /// <summary>
        /// Display all job listings
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {

            // Get all the jobs
            List<Job> jobs = JobModel.GetAll();
            ViewBag.jobs = jobs;

            return View();
        }

        /// <summary>
        /// Display all archived job listings
        /// </summary>
        /// <returns></returns>
        public ActionResult Archive() {

            // Get all the jobs
            List<Job> jobs = JobModel.GetArchived();
            ViewBag.jobs = jobs;

            return View();
        }

        /// <summary>
        /// Display job categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Categories() {
            JobBoardDataContext db = new JobBoardDataContext();

            // Get all the jobs
            List<CategoryWithParent> categories = CategoryModel.GetAll();
            foreach (CategoryWithParent category in categories) {
                category.jobCount = db.JobCategories.Where(x => x.cat.Equals(category.id)).Count();
            }
            ViewBag.categories = categories;

            return View();
        }

        /// <summary>
        /// Display jobs in specified category
        /// </summary>
        /// <returns></returns>
        public ActionResult Category(Guid id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();

            // Get all the jobs
            Category category = CategoryModel.Get(id);
            ViewBag.category = category;

            // Get all the jobs
            List<Job> jobs = JobModel.GetByCategory(id);
            ViewBag.jobs = jobs;

            return View();
        }

        /// <summary>
        /// Display job categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Locations() {
            JobBoardDataContext db = new JobBoardDataContext();

            // Get all the jobs
            List<DisplayableLocation> locations = LocationModel.GetAllDisplayable();
            foreach (DisplayableLocation location in locations) {
                location.jobCount = db.Jobs.Where(x => x.location.Equals(location.id)).Count();
            }
            ViewBag.locations = locations;

            return View();
        }

        /// <summary>
        /// Display jobs in specified category
        /// </summary>
        /// <returns></returns>
        public ActionResult Location(Guid id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();

            // Get all the jobs
            Location location = LocationModel.Get(id);
            ViewBag.location = location;

            // Get all the jobs
            List<Job> jobs = JobModel.GetByLocation(id);
            ViewBag.jobs = jobs;

            return View();
        }
        
        /// <summary>
        /// Add a new listing
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Add() {
            
            // Get the education levels
            List<Education> eds = EducationModel.GetAll();
            ViewBag.eds = eds;

            // Get the experience levels
            List<Experience> exps = ExperienceModel.GetAll();
            ViewBag.exps = exps;

            // Get the contacts
            List<DisplayableContact> cons = ContactModel.GetAll();
            ViewBag.cons = cons;

            // Get the locations
            List<DisplayableLocation> locs = LocationModel.GetAllDisplayable();
            ViewBag.locs = locs;

            // Get the categories
            List<Category> cats = CategoryModel.GetCategories();
            ViewBag.cats = cats;

            // Get the shifts
            List<Shift> shifts = ShiftModel.GetAll();
            ViewBag.shifts = shifts;

            // Get the contacts
            List<DisplayableContact> contacts = ContactModel.GetAll();
            ViewBag.contacts = contacts;

            ViewBag.job_title = TempData["title"];
            ViewBag.short_desc = TempData["short_desc"];
            ViewBag.long_desc = TempData["long_desc"];
            ViewBag.experience = TempData["experience"];
            ViewBag.education = TempData["education"];
            ViewBag.location = TempData["location"];
            ViewBag.contact = TempData["contact"];
            ViewBag.isDriving = TempData["isDriving"];
            ViewBag.salary_type = TempData["salary_type"];
            ViewBag.status = TempData["status"];

            return View();
        }

        /// <summary>
        /// Edit a job listing
        /// </summary>
        /// <param name="id">Identifier for job</param>
        /// <returns>ActionResult</returns>
        public ActionResult Edit(Guid id = new Guid()) {

            // Get the job
            Job job = JobModel.Get(id);
            ViewBag.job = job;

            // Get the education levels
            List<Education> eds = EducationModel.GetAll();
            ViewBag.eds = eds;

            // Get the experience levels
            List<Experience> exps = ExperienceModel.GetAll();
            ViewBag.exps = exps;

            // Get the contacts
            List<DisplayableContact> cons = ContactModel.GetAll();
            ViewBag.cons = cons;

            // Get the locations
            List<DisplayableLocation> locs = LocationModel.GetAllDisplayable();
            ViewBag.locs = locs;

            // Get the categories
            List<Category> cats = CategoryModel.GetCategories();
            ViewBag.cats = cats;

            // Get the categories
            List<Shift> shifts = ShiftModel.GetAll();
            ViewBag.shifts = shifts;

            // Get the contacts
            List<DisplayableContact> contacts = ContactModel.GetAll();
            ViewBag.contacts = contacts;

            ViewBag.job_title = TempData["title"];
            ViewBag.short_desc = TempData["short_desc"];
            ViewBag.long_desc = TempData["long_desc"];
            ViewBag.experience = TempData["experience"];
            ViewBag.education = TempData["education"];
            ViewBag.location = TempData["location"];
            ViewBag.contact = TempData["contact"];
            ViewBag.isDriving = TempData["isDriving"];
            ViewBag.salary_type = TempData["salary_type"];
            ViewBag.status = TempData["status"];
            ViewBag.jobState = TempData["jobState"];

            return View();
        }

        /// <summary>
        /// Save a new job listing
        /// </summary>
        /// <returns>Redirect to appropriate page</returns>
        public dynamic Save(Guid id = new Guid(), string title = "", string short_desc = "", string salary_type = "", string status = "", string long_desc = "", Guid experience = new Guid(), Guid education = new Guid(), Guid location = new Guid(), Guid contact = new Guid(), int isDriving = 0, string[] cats = null, string[] shifts = null, string jobState = "") {
            try {
                List<Guid> categories = new List<Guid>();
                if (cats != null) {
                    foreach (string cat in cats) {
                        categories.Add(new Guid(cat));
                    }
                }

                List<Guid> shfts = new List<Guid>();
                if (shifts != null) {
                    foreach (string sh in shifts) {
                        shfts.Add(new Guid(sh));
                    }
                }

                if (id == null || id == Guid.Empty) {
                    JobModel.Create(title, short_desc, salary_type, status, long_desc, experience, education, location, contact, isDriving, categories, shfts);
                } else {
                    JobModel.Update(id, title, short_desc, salary_type, status, long_desc, experience, education, location, contact, isDriving, jobState);
                }

                return RedirectToAction("Index", "Jobs");
            } catch (Exception) {
                try {
                    TempData["title"] = title;
                    TempData["short_desc"] = short_desc;
                    TempData["long_desc"] = long_desc;
                    TempData["experience"] = experience;
                    TempData["education"] = education;
                    TempData["location"] = location;
                    TempData["contact"] = contact;
                    TempData["isDriving"] = isDriving;
                    TempData["salary_type"] = salary_type;
                    TempData["status"] = status;
                    if (id == null || id == Guid.Empty) {
                        return RedirectToAction("Add", "Jobs");
                    } else {
                        return RedirectToAction("Edit", "Jobs", new { id = id });
                    }
                } catch (Exception) {
                    return RedirectToAction("Index", "Jobs");
                }
            }
        }

        public dynamic Delete(Guid id = new Guid(), bool ajax = false) {
            if (!ajax) {
                string resp = JobModel.Delete(id);
                return RedirectToAction("Index", "Jobs");
            } else {
                return JobModel.Delete(id);
            }
        }

        public ActionResult Duplicate(Guid id = new Guid()) {
            Guid new_id = JobModel.Duplicate(id);
            return RedirectToAction("Edit", new { id = new_id });
        }

        /******** AJAX Functions *******/

        public string AddCategory(Guid job_id = new Guid(), Guid cat_id = new Guid()) {
            try {
                return JobModel.AddJobCategory(job_id, cat_id);
            } catch (Exception) {
                return "[]";
            }
        }

        public string DeleteCategory(Guid job_id = new Guid(), Guid cat_id = new Guid()) {
            try {
                JobModel.DeleteJobCategory(job_id, cat_id);
                return "";
            } catch (Exception) {
                return "There was an error while processing your request.";
            }
        }

        public string AddShift(Guid job_id = new Guid(), Guid shift_id = new Guid()) {
            try {
                return JobModel.AddJobShift(job_id, shift_id);
            } catch (Exception) {
                return "[]";
            }
        }

        public string DeleteShift(Guid job_id = new Guid(), Guid shift_id = new Guid()) {
            try {
                JobModel.DeleteJobShift(job_id, shift_id);
                return "";
            } catch (Exception) {
                return "There was an error while processing your request.";
            }
        }

        public string AddContact(Guid job_id = new Guid(), Guid contact_id = new Guid()) {
            try {
                return JobModel.AddJobContact(job_id, contact_id);
            } catch (Exception) {
                return "[]";
            }
        }

        public string DeleteContact(Guid job_id = new Guid(), Guid contact_id = new Guid()) {
            try {
                JobModel.DeleteJobContact(job_id, contact_id);
                return "";
            } catch (Exception) {
                return "There was an error while processing your request.";
            }
        }
    }
}
