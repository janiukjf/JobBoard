using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class LocationsController : AuthController {

        public ActionResult Index() {

            ViewBag.msg = TempData["msg"];
            TempData.Clear();

            // Get all the Locations with the associated state data
            List<DisplayableLocation> locs = LocationModel.GetAllDisplayable();
            ViewBag.locs = locs;

            // Get all the States
            List<State> states = ContactModel.GetStates();
            ViewBag.states = states;

            return View();
        }

        public ActionResult Add() {

            // Get TempData 
            ViewBag.msg = TempData["msg"];

            // Get all the states
            List<State> states = ContactModel.GetStates();
            ViewBag.states = states;

            return View();
        }

        public ActionResult Edit(Guid id) {
            try {
                // Validate the id
                if (id == null || id == Guid.Empty) { throw new Exception("There was an error while processing your request."); };

                // Get TempData 
                ViewBag.msg = TempData["msg"];

                // Get all the states
                List<State> states = ContactModel.GetStates();
                ViewBag.states = states;

                // Get the Location
                Location loc = LocationModel.Get(id);
                ViewBag.loc = loc;

                // Get the jobs for this location
                List<Job> jobs = LocationModel.GetJobs(id);
                ViewBag.jobs = jobs;

                return View();
            } catch (Exception) {
                TempData["msg"] = "There was an error while processing your request.";
                return RedirectToAction("Index", "Locations");
            }
        }

        public dynamic Save(Guid id = new Guid(), string city = "", Guid state_id = new Guid()) {
            try {
                TempData["id"] = id;
                TempData["city"] = city;
                TempData["state_id"] = state_id;

                if (id == null || id == Guid.Empty) { // Create new
                    LocationModel.Create(city, state_id);
                    TempData["msg"] = "Successfully added location.";
                } else { // Update existing
                    LocationModel.Update(id, city, state_id);
                    TempData["msg"] = "Successfully updated location.";
                }
                return RedirectToAction("Index", "Locations");
            } catch (Exception) {
                TempData["id"] = id;
                TempData["city"] = city;
                TempData["state_id"] = state_id;
                TempData["msg"] = "Failed to save location";
                return RedirectToAction("Index", "Locations");
            }
        }

        public string Delete(Guid id = new Guid()) {
            try {
                LocationModel.Delete(id);
                return "";
            } catch (Exception) {
                return "There was an error while removing the location.";
            }
        }

    }
}