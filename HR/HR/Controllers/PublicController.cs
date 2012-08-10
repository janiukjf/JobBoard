using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Models;

namespace HR.Controllers {
    public class PublicController : Controller {

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);

            // Get all the categories
            List<TieredCategories> cats = ContentModel.GetMenu();
            ViewBag.cats = cats;
        }

    }
}
