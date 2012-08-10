using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class ExperienceController : AuthController {

        public ActionResult Index() {

            ViewBag.msg = TempData["msg"];
            TempData.Clear();

            // Get all the Experience data
            List<Experience> exps = ExperienceModel.GetAll();
            ViewBag.exps = exps;

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
                Experience exp = ExperienceModel.Get(id);
                ViewBag.exp = exp;

                // Get the jobs for this location
                List<Job> jobs = ExperienceModel.GetJobs(id);
                ViewBag.jobs = jobs;

                return View();
            } catch (Exception) {
                TempData["msg"] = "There was an error while processing your request.";
                return RedirectToAction("Index", "Experience");
            }
        }

        public dynamic Save(Guid id = new Guid(), string experience = "") {
            try {
                TempData["id"] = id;
                TempData["experience"] = experience;

                if (id == null || id == Guid.Empty) { // Create new
                    ExperienceModel.Create(experience);
                    TempData["msg"] = "Successfully added experience level.";
                } else { // Update existing
                    ExperienceModel.Update(id, experience);
                    TempData["msg"] = "Successfully updated experience level.";
                }
                return RedirectToAction("Index", "Experience");
            } catch (Exception) {
                TempData["id"] = id;
                TempData["experience"] = experience;
                TempData["msg"] = "Failed to save experience level";
                return RedirectToAction("Index", "Experience");
            }
        }

        public string Delete(Guid id = new Guid()) {
            try {
                ExperienceModel.Delete(id);
                return "";
            } catch (Exception) {
                return "There was an error while removing the experience level.";
            }
        }

    }
}
