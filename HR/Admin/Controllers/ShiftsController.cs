using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class ShiftsController : AuthController {

        public ActionResult Index() {

            ViewBag.msg = TempData["msg"];
            TempData.Clear();

            // Get all the Experience data
            List<Shift> shifts = ShiftModel.GetAll();
            ViewBag.shifts = shifts;

            return View();
        }

        public ActionResult Add() {

            // Get TempData 
            ViewBag.msg = TempData["msg"];

            return View();
        }

        public ActionResult Edit(Guid id) {
            try {
                // Validate the id
                if (id == null || id == Guid.Empty) { throw new Exception("There was an error while processing your request."); };

                // Get TempData 
                ViewBag.msg = TempData["msg"];

                // Get the Location
                Shift shift = ShiftModel.Get(id);
                ViewBag.shiftobj = shift;

                // Get the jobs for this location
                List<Job> jobs = ShiftModel.GetJobs(id);
                ViewBag.jobs = jobs;

                return View();
            } catch (Exception) {
                TempData["msg"] = "There was an error while processing your request.";
                return RedirectToAction("Index", "Shifts");
            }
        }

        public dynamic Save(Guid id = new Guid(), string shift = "") {
            try {
                TempData["id"] = id;
                TempData["shift"] = shift;

                if (id == null || id == Guid.Empty) { // Create new
                    ShiftModel.Create(shift);
                    TempData["msg"] = "Successfully added shift.";
                } else { // Update existing
                    ShiftModel.Update(id, shift);
                    TempData["msg"] = "Successfully updated shift.";
                }
                return RedirectToAction("Index", "Shifts");
            } catch (Exception) {
                TempData["id"] = id;
                TempData["shift"] = shift;
                TempData["msg"] = "Failed to save shift";
                return RedirectToAction("Index", "Shifts");
            }
        }

        public string Delete(Guid id = new Guid()) {
            try {
                ShiftModel.Delete(id);
                return "";
            } catch (Exception) {
                return "There was an error while removing the shift.";
            }
        }
    }
}
