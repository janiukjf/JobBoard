using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Controllers;

namespace HR.Controllers {
    public class IndexController : AuthController {
        //
        // GET: /Index/

        public ActionResult Index() {
            return View();
        }

    }
}
