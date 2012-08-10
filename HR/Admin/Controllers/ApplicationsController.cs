using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class ApplicationsController : AuthController {

        /// <summary>
        /// Display all applications
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {

            Contact user = ViewBag.user;
            // Get all the jobs
            List<Application> applications = new Application().GetAll(user.id);
            foreach(Application a in applications) {
                a.job = JobModel.Get((Guid)a.job_id);
            }
            ViewBag.applications = applications;

            return View();
        }


        /// <summary>
        /// View Application Details
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewApplication(Guid id = new Guid()) {

            Application app = new Application();
            app.Get(id);

            Job job = JobModel.Get((Guid)app.job_id);
            List<State> states = LocationModel.GetAllStates();

            ViewBag.application = app;
            ViewBag.states = states;
            ViewBag.job = job;

            return View();
        }

        /// <summary>
        /// Send Application to the Archive
        /// </summary>
        /// <returns></returns>
        public ActionResult ArchiveApplication(Guid id = new Guid()) {

            Application app = new Application();
            app.Get(id);
            app.Archive();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Display all archived job listings
        /// </summary>
        /// <returns></returns>
        public ActionResult Archive() {

            Contact user = ViewBag.user;
            // Get all the jobs
            List<Application> applications = new Application().GetArchive(user.id);
            foreach (Application a in applications) {
                a.job = JobModel.Get((Guid)a.job_id);
            }
            ViewBag.applications = applications;

            return View();
        }

        /// <summary>
        /// Display job categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Jobs() {
            JobBoardDataContext db = new JobBoardDataContext();

            // Get all the jobs
            List<Job> jobs = JobModel.GetAll();
            ViewBag.jobs = jobs;

            return View();
        }

        /// <summary>
        /// Display job categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Job(Guid id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();

            Contact user = ViewBag.user;
            // Get the job
            Job job = JobModel.Get(id);
            ViewBag.job = job;

            List<Application> applications = job.Applications.Where(x => x.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !x.dateSubmitted.Equals(null)).ToList<Application>();
            ViewBag.applications = applications;

            return View();
        }

        /// <summary>
        /// Display job categories
        /// </summary>
        /// <returns></returns>
        public ActionResult Categories() {
            JobBoardDataContext db = new JobBoardDataContext();

            Contact user = ViewBag.user;

            // Get all the jobs
            List<CategoryWithParent> categories = CategoryModel.GetAll();
            foreach (CategoryWithParent category in categories) {
                // instead of extending category again, we'll just reuse the job count field for application count
                if(user.isAdmin()) {
                    category.jobCount = (from c in db.Categories
                                         join jc in db.JobCategories on c.id equals jc.cat
                                         join j in db.Jobs on jc.job equals j.id
                                         join a in db.Applications on j.id equals a.job_id
                                         where c.id.Equals(category.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                         && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         select a).Count();
                } else {
                    JobContact contact = new JobContact {
                        contact = user.id
                    };
                    category.jobCount = (from c in db.Categories
                                         join jc in db.JobCategories on c.id equals jc.cat
                                         join j in db.Jobs on jc.job equals j.id
                                         join a in db.Applications on j.id equals a.job_id
                                         where c.id.Equals(category.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                         && j.Notifications.Contains(contact, new JobContactEqualityComparer())
                                         && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         select a).Count();
                }
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

            Contact user = ViewBag.user;
            // Get all the jobs
            Category category = CategoryModel.Get(id);
            ViewBag.category = category;

            // Get all the jobs
            List<Application> applications = new Application().GetByCategory(id,user.id);
            foreach (Application a in applications) {
                a.job = JobModel.Get((Guid)a.job_id);
            }
            ViewBag.applications = applications;

            return View();
        }

        /// <summary>
        /// Display job locations
        /// </summary>
        /// <returns></returns>
        public ActionResult Locations() {
            JobBoardDataContext db = new JobBoardDataContext();

            Contact user = ViewBag.user;
            // Get all the jobs
            List<DisplayableLocation> locations = LocationModel.GetAllDisplayable();
            foreach (DisplayableLocation location in locations) {
                if (user.isAdmin()) {
                    location.jobCount = (from l in db.Locations
                                         join j in db.Jobs on l.id equals j.location
                                         join a in db.Applications on j.id equals a.job_id
                                         where l.id.Equals(location.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                         && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         select a).Count();
                } else {
                    JobContact contact = new JobContact {
                        contact = user.id
                    };
                    location.jobCount = (from l in db.Locations
                                         join j in db.Jobs on l.id equals j.location
                                         join a in db.Applications on j.id equals a.job_id
                                         where l.id.Equals(location.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                         && j.Notifications.Contains(contact, new JobContactEqualityComparer())
                                         && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         select a).Count();
                }
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

            Contact user = ViewBag.user;
            // Get the location
            Location location = LocationModel.Get(id);
            ViewBag.location = location;

            // Get all the jobs
            List<Application> applications = new Application().GetByLocation(id, user.id);
            foreach (Application a in applications) {
                a.job = JobModel.Get((Guid)a.job_id);
            }
            ViewBag.applications = applications;

            return View();
        }

        /// <summary>
        /// Display job education levels
        /// </summary>
        /// <returns></returns>
        public ActionResult Educations() {
            JobBoardDataContext db = new JobBoardDataContext();

            Contact user = ViewBag.user;
            // Get all the jobs
            List<Education> educations = EducationModel.GetAll();
            foreach (Education ed in educations) {
                ed.getAppCount(user.id);
            }
            ViewBag.educations = educations;

            return View();
        }

        /// <summary>
        /// Display jobs in specified education level
        /// </summary>
        /// <returns></returns>
        public ActionResult Education(Guid id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();

            Contact user = ViewBag.user;
            // Get the location
            Education education = EducationModel.Get(id);
            ViewBag.education = education;

            // Get all the jobs
            List<Application> applications = new Application().GetByEducation(id, user.id);
            foreach (Application a in applications) {
                a.job = JobModel.Get((Guid)a.job_id);
            }
            ViewBag.applications = applications;

            return View();
        }

        /// <summary>
        /// Display job experience levels
        /// </summary>
        /// <returns></returns>
        public ActionResult Experiences() {
            JobBoardDataContext db = new JobBoardDataContext();

            Contact user = ViewBag.user;
            // Get all the jobs
            List<Experience> experiences = ExperienceModel.GetAll();
            foreach (Experience ex in experiences) {
                ex.getAppCount(user.id);
            }
            ViewBag.experiences = experiences;

            return View();
        }

        /// <summary>
        /// Display jobs in specified experience level
        /// </summary>
        /// <returns></returns>
        public ActionResult Experience(Guid id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();

            Contact user = ViewBag.user;
            // Get the location
            Experience experience = ExperienceModel.Get(id);
            ViewBag.experience = experience;

            // Get all the jobs
            List<Application> applications = new Application().GetByExperience(id, user.id);
            foreach (Application a in applications) {
                a.job = JobModel.Get((Guid)a.job_id);
            }
            ViewBag.applications = applications;

            return View();
        }
    }
}
