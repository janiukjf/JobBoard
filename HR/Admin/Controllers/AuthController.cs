using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers {
    public class AuthController : Controller {
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);

            HttpCookie contactID = new HttpCookie("");
            contactID = Request.Cookies.Get("contact_id");

            // Check the cookies to make sure the user is authenticated to the proper level
            if (contactID == null || contactID.Value == null || !checkAuth(contactID.Value)) {
                // Check the session to make sure the user is authenticated to the proper level
                if (Session["contact_id"] == null || !checkAuth(Session["contact_id"].ToString())) {
                    TempData["redirect"] = HttpContext.Request.Url.PathAndQuery;
                    HttpContext.Response.Redirect("/Admin/Authorize");
                }
            } else {
                Session["contact_id"] = contactID.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact_id"></param>
        /// <returns></returns>
        private bool checkAuth(string contact_id) {
            try {
                Guid id = new Guid(contact_id);

                if (id.ToString().Length > 0) { // Make sure the id is a valid contact id

                    JobBoardDataContext db = new JobBoardDataContext();

                    Contact contact = db.Contacts.Where(x => x.id == id && x.level != "DISABLED").FirstOrDefault<Contact>();
                    if (contact == null) { throw new Exception(); }
                    ViewBag.user = contact;

                    Session["contact_id"] = id;

                    HttpCookie contactID = new HttpCookie("contact_id");
                    contactID.Value = id.ToString();
                    contactID.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(contactID);

                    return true;
                } else {
                    return false;
                }

            } catch (Exception) {
                return false;
            }
        }
    }
}
