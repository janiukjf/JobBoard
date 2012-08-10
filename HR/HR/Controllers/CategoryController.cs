using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Models;

namespace HR.Controllers {
    public class CategoryController : PublicController {

        public ActionResult Index(Guid id = new Guid()) {

            Category cat = new Category(id);
            cat.LoadJobs();
            ViewBag.cat = cat;

            return View();
        }

    }
}
