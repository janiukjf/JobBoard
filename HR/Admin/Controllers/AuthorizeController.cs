using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers
{
    public class AuthorizeController : Controller
    {
        public ActionResult Index(string redirectUrl = "", string msg = "", string username = "") {

            // Assign GET data to ViewBag
            ViewBag.redirect = redirectUrl;
            ViewBag.msg = msg;
            ViewBag.username = username;

            // Check if we have anything stored in TempData and set them as the ViewBag variable if available
            if (TempData["msg"] != null && TempData["msg"].ToString().Length > 0) {
                ViewBag.msg = TempData["msg"];
            }
            if (TempData["username"] != null && TempData["username"].ToString().Length > 0) {
                ViewBag.username = TempData["username"];
            }
            if (TempData["redirect"] != null && TempData["redirect"].ToString().Length > 0) {
                ViewBag.redirect = TempData["redirect"];
            }
            ViewBag.hide_logout = true;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public dynamic Login(string username = "", string password = "", string redirect = "") {
            try {

                // Attempt to retrieve the Guid for the contact record
                Guid id = Auth.Login(username, password);

                // Validate the the returned Guid is a valid identification
                if (id == Guid.Empty) { throw new Exception("Your Username or Password is incorrect. Try again."); }
                if (id == null || id.ToString().Length == 0) { throw new Exception("Your Username or Password is incorrect. Try again."); }

                // Add the contactID to the HttpCookie object
                HttpCookie contactID = new HttpCookie("contact_id");
                contactID.Value = id.ToString();
                contactID.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(contactID);

                if (redirect.Length == 0) { // Redirect to homepage
                    return RedirectToAction("Index", "Index");
                } else {// Redirect to where the user was originally intending on going
                    return Redirect(redirect);
                }
            } catch (Exception e) {
                // Store variables into TempData object and redirect to login page
                TempData["msg"] = "Failed to log you into the system. " + e.Message;
                TempData["username"] = username;
                TempData["redirect"] = redirect;
                return RedirectToAction("Index", "Authorize");
            }
        }

        public ActionResult Out() {
            // Clear the authentication cookies
            Response.Cookies["contact_id"].Expires = DateTime.Now.AddYears(-100);

            // Clear the session data
            Session["contact_id"] = 0; // We'll set the contact_id variable to zero, just to be sure
            Session.RemoveAll();
            Session.Abandon();

            // Store success message into TempData and redirect
            TempData["msg"] = "Successfully logged out.";
            return RedirectToAction("Index", "Authorize");
        }

    }
}
