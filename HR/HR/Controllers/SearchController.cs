using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Models;

namespace HR.Controllers {
    public class SearchController : PublicController {
        //
        // GET: /Search/

        public ActionResult Index(string exp = "", string edu = "", string loc = "", string shift = "") {

            List<DisplayableJob> jobs = JobModel.Search(exp, edu, loc, shift);
            ViewBag.jobs = jobs;

            return View();
        }

    }
}
