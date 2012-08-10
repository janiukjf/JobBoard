using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class ContactsController : AuthController {

        /// <summary>
        /// Display all contact records
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {

            ViewBag.msg = TempData["msg"];
            TempData.Clear();

            // Get the contacts in the database
            List<DisplayableContact> contacts = ContactModel.GetAll();
            ViewBag.contacts = contacts;

            return View();
        }

        public ActionResult Add() {
            // Get the states
            List<State> states = ContactModel.GetStates();
            ViewBag.states = states;

            return View();
        }

        public ActionResult Edit(Guid id) {
            if (id == null) {
                return RedirectToAction("Index", "Contact");
            }

            // Get the contact record
            DisplayableContact contact = ContactModel.Get(id);
            ViewBag.contact = contact;

            // Get the listings for this contact
            List<Job> listings = ContactModel.GetListings(id);
            ViewBag.listings = listings;

            // Get the states
            List<State> states = ContactModel.GetStates();
            ViewBag.states = states;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public dynamic Save(Guid id = new Guid(), Guid state_id = new Guid(), string name = "", string street = "", string city = "", string phone = "", string fax = "", string email = "", string username = "", string pwd = "", string level = "USER") {
            try {
                string msg = "";
                if (id == null || id == Guid.Empty) {
                    msg = ContactModel.Create(name, street, city, state_id, phone, fax, email, username, pwd, level);
                } else {
                    msg = ContactModel.Update(id, name, street, city, state_id, phone, fax, email, username, pwd, level);
                }
                TempData["msg"] = msg;
                return RedirectToAction("Index", "Contacts");
            } catch (Exception) {
                TempData["msg"] = "Failed to save contact.";
                return RedirectToAction("Index", "Contacts");
            }
        }

        public string Delete(Guid id = new Guid()) {
            try {
                // Get the current users contact_id
                HttpCookie contactID = new HttpCookie("");
                contactID = Request.Cookies.Get("contact_id");

                return ContactModel.Delete(id, contactID.Value);
            } catch (Exception e) {
                return e.Message;
            }
        }
    }
}
