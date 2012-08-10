using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Models;

namespace HR.Controllers {
    public class IndexController : PublicController {

        public ActionResult Index() {

            // Get the education levels
            List<Education> eds = ContentModel.GetEducation();
            ViewBag.eds = eds;

            // Get the locations
            List<DisplayableLocation> locs = ContentModel.GetLocations();
            ViewBag.locs = locs;

            // Get the experience levels
            List<Experience> exps = ContentModel.GetExperience();
            ViewBag.exps = exps;

            // Get the shifts
            List<Shift> shifts = ShiftModel.GetAll();
            ViewBag.shifts = shifts;

            // Get the last 10 jobs
            List<DisplayableJob> last_ten = JobModel.GetAll(true, 10);
            ViewBag.jobs = last_ten;

            return View();
        }

    }
}
