using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class EducationController : AuthController {

        public ActionResult Index() {

            ViewBag.msg = TempData["msg"];
            TempData.Clear();

            // Get all the Experience data
            List<Education> eds = EducationModel.GetAll();
            ViewBag.eds = eds;

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
                Education ed = EducationModel.Get(id);
                ViewBag.ed = ed;

                // Get the jobs for this location
                List<Job> jobs = EducationModel.GetJobs(id);
                ViewBag.jobs = jobs;

                return View();
            } catch (Exception) {
                TempData["msg"] = "There was an error while processing your request.";
                return RedirectToAction("Index", "Education");
            }
        }

        public dynamic Save(Guid id = new Guid(), string education = "") {
            try {
                TempData["id"] = id;
                TempData["education"] = education;

                if (id == null || id == Guid.Empty) { // Create new
                    EducationModel.Create(education);
                    TempData["msg"] = "Successfully added education level.";
                } else { // Update existing
                    EducationModel.Update(id, education);
                    TempData["msg"] = "Successfully updated education level.";
                }
                return RedirectToAction("Index", "Education");
            } catch (Exception) {
                TempData["id"] = id;
                TempData["education"] = education;
                TempData["msg"] = "Failed to save education level";
                return RedirectToAction("Index", "Education");
            }
        }

        public string Delete(Guid id = new Guid()) {
            try {
                EducationModel.Delete(id);
                return "";
            } catch (Exception) {
                return "There was an error while removing the education level.";
            }
        }
    }
}
