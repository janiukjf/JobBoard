using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using HR.Models;
using System.Reflection;
using System.IO;

namespace HR.Controllers {
    public class JobController : PublicController {

        public ActionResult Index(Guid id = new Guid()) {

            // Get the job record
            DisplayableJob job = JobModel.Get(id);
            ViewBag.job = job;

            return View();
        }

        public ActionResult Apply(Guid id = new Guid()) {

            Guid app_id = (Request.QueryString["app_id"] != null) ? Guid.Parse(Request.QueryString["app_id"]) : Guid.Empty;
            string error = (Request.QueryString["error"] != null) ? Request.QueryString["error"] : "";

            // Get the job record
            DisplayableJob job = JobModel.Get(id);
            List<Crumb> crumbs = job.GenerateCrumbs(app_id, ApplicationStates.BASIC);
            ViewBag.crumbs = crumbs;
            ViewBag.job = job;


            // Attempt to fetch an existing Application
            Application app = new Application();
            app.Get(app_id);
            ViewBag.application = app;
            if (app != null && app.id != Guid.Empty && app.dateSubmitted != null) {
                return RedirectToAction("Review", new { id = app_id });
            }

            // Get the states
            List<State> states = ContentModel.GetStates();
            ViewBag.states = states;

            ViewBag.error = error;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveBasic(Guid id, Guid app_id) {
            try {

                string fname = ((Request["fname"] != null) ? Request["fname"] : "");
                string mname = ((Request["mname"] != null) ? Request["mname"] : "");
                string lname = ((Request["lname"] != null) ? Request["lname"] : "");
                string address = ((Request["address"] != null) ? Request["address"] : "");
                string city = ((Request["city"] != null) ? Request["city"] : "");
                Guid state_id = ((Request["state_id"] != null) ? new Guid(Request["state_id"]) : Guid.Empty);
                string zip = ((Request["zip"] != null) ? Request["zip"] : "");
                string phone1 = ((Request["phone1"] != null) ? Request["phone1"] : "");
                string phone2 = ((Request["phone2"] != null) ? Request["phone2"] : "");
                string referred_by = ((Request["referred_by"] != null) ? Request["referred_by"] : "");
                int previousEmployee = ((Request["previousEmployee"] != null) ? Convert.ToInt32(Request["previousEmployee"]) : 0);
                string previousExplanation = ((Request["previousExplanation"] != null) ? Request["previousExplanation"] : "");
                int isAdult = ((Request["isAdult"] != null) ? Convert.ToInt32(Request["isAdult"]) : 0);
                int isCitizen = ((Request["isCitizen"] != null) ? Convert.ToInt32(Request["isCitizen"]) : 0);
                int isEmployed = ((Request["isEmployed"] != null) ? Convert.ToInt32(Request["isEmployed"]) : 0);
                string date_available = ((Request["date_available"] != null) ? Request["date_available"] : "");
                decimal startSal = ((Request["desired_start"] != null) ? Convert.ToDecimal(Request["desired_start"]) : 0);
                decimal endSal = ((Request["desired_end"] != null) ? Convert.ToDecimal(Request["desired_end"]) : 0);
                string employment_type = ((Request["employment_type"] != null) ? Request["employment_type"] : "");
                string preferred_shift = ((Request["preferred_shift"] != null) ? Request["preferred_shift"] : "");
                int isFelon = ((Request["isFelon"] != null) ? Convert.ToInt32(Request["isFelon"]) : 0);
                List<string> conviction_dates = ((Request["conviction_date"] != null) ? Request["conviction_date"].Split(',').ToList<string>() : new List<string>());
                List<string> convictions = ((Request["conviction"] != null) ? Request["conviction"].Split(',').ToList<string>() : new List<string>());

                Application resp = JobModel.SaveBasic(app_id, fname, mname, lname, address, city, state_id, zip, phone1, phone2, referred_by, previousEmployee, previousExplanation, isAdult, isCitizen, isEmployed, date_available, startSal, endSal, employment_type, preferred_shift, isFelon, conviction_dates, convictions, id);

                return Redirect("/Job/Employment/"+ id + "?app_id="+resp.id);
                //return RedirectToAction("Employment", new { id = resp.id },);
            } catch (Exception e) {
                return Redirect("/Job/Apply/" + id + "?app_id=" + app_id + "&error=Error processing information: " + e.Message);
            }
        }

        public ActionResult Employment(Guid id = new Guid(), string error = "") {
            Guid app_id = Guid.Empty;
            try {
                app_id = (Request.QueryString["app_id"] != null)?Guid.Parse(Request.QueryString["app_id"]): Guid.Empty;
                // Get the job record
                DisplayableJob job = JobModel.Get(id);
                List<Crumb> crumbs = job.GenerateCrumbs(app_id, ApplicationStates.EMPLOYMENT);
                ViewBag.crumbs = crumbs;
                ViewBag.job = job;

                // Attempt to fetch an existing application
                Application app = new Application();
                app.Get(app_id);
                //app.GetEmployments();
                ViewBag.application = app;
                if (app != null && app.id != Guid.Empty && app.dateSubmitted != null) {
                    return RedirectToAction("Review", new { id = app_id });
                }

                // Get the states
                List<State> states = ContentModel.GetStates();
                ViewBag.states = states;

                ViewBag.error = error;
                return View();
            } catch {
                return Redirect("/Job/Apply/" + id + "?app_id=" + app_id + "&error=Error processing information.");
                //return RedirectToAction("Apply", new { id = id, error = "Error processing information." });
            }
        }

        public ActionResult SaveEmployment(Guid id = new Guid(), Guid emp_id = new Guid(), Guid job_id = new Guid()) {
            try {

                string start_date = (Request.Form["start_date"] != null) ? Request.Form["start_date"] : "";
                string end_date = (Request.Form["end_date"] != null) ? Request.Form["end_date"] : "";
                string employer = (Request.Form["employer"] != null) ? Request.Form["employer"] : "";
                string phone = (Request.Form["phone"] != null) ? Request.Form["phone"] : "";
                string address = (Request.Form["address"] != null) ? Request.Form["address"] : "";
                string city = (Request.Form["city"] != null) ? Request.Form["city"] : "";
                Guid state_id = (Request.Form["state_id"] != null) ? Guid.Parse(Request.Form["state_id"]) : Guid.Empty;
                string starting_title = (Request.Form["starting_title"] != null) ? Request.Form["starting_title"] : "";
                string ending_title = (Request.Form["ending_title"] != null) ? Request.Form["ending_title"] : "";
                string supervisor = (Request.Form["supervisor"] != null) ? Request.Form["supervisor"] : "";
                int contact = (Request.Form["contact"] != null) ? Convert.ToInt16(Request.Form["contact"]) : 0;
                string reason_leaving = (Request.Form["reason_leaving"] != null) ? Request.Form["reason_leaving"] : "";
                string summary = (Request.Form["summary"] != null) ? Request.Form["summary"] : "";
                string starting_pay = (Request.Form["starting_pay"] != null) ? Request.Form["starting_pay"] : "";
                string ending_pay = (Request.Form["ending_pay"] != null) ? Request.Form["ending_pay"] : "";

                JobModel.AddEmployment(emp_id, id, start_date, end_date, employer, phone, address, city, state_id, starting_title, ending_title, supervisor, contact, reason_leaving, summary, starting_pay, ending_pay);

                if (Request.Form["addEmployment"] != null) {
                    return Redirect("/Job/Employment/" + job_id + "?app_id=" + id + "&error=Employment saved.");
                    //return RedirectToAction("Employment", new { id = id, error = "Employment added." });
                } else {
                    return RedirectToAction("Education", new { id = id });
                }
            } catch (Exception e) {
                return Redirect("/Job/Employment/" + job_id + "?app_id=" + id + "&error=Error saving employment: " + e.Message);
                //return RedirectToAction("Employment", new { id = id, error = e.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteEmployment(Guid id = new Guid()) {
            Guid app_id = new Guid(); 
            Guid emp_id = new Guid();
            try {
                emp_id = (Request.QueryString["emp_id"] != null) ? Guid.Parse(Request.QueryString["emp_id"]) : Guid.Empty;
                app_id = (Request.QueryString["app_id"] != null) ? Guid.Parse(Request.QueryString["app_id"]) : Guid.Empty;
                Application app = new Application();
                app.Get(app_id);
                app.DeleteEmployment(emp_id);
                return Redirect("/Job/Employment/"+id + "?app_id=" + app_id);
            } catch (Exception e) { 
                return Redirect("/Job/Employment/"+id + "?app_id=" + app_id + "&error=Error while deleteing: " + e.Message);
            }
            
        }

        public ActionResult Education(Guid id = new Guid(), string error = "") {
            try {
                // Attempt to fetch an existing application
                Application app = new Application();
                app.Get(id);
                ViewBag.application = app;
                if (app != null && app.id != Guid.Empty && app.dateSubmitted != null) {
                    return RedirectToAction("Review", new { id = id });
                }

                // Get the job record
                DisplayableJob job = JobModel.Get((Guid)app.job_id);
                List<Crumb> crumbs = job.GenerateCrumbs(id, ApplicationStates.EDUCATION);
                ViewBag.crumbs = crumbs;
                ViewBag.job = job;
                
                // Get the states
                List<State> states = ContentModel.GetStates();
                ViewBag.states = states;

                ViewBag.error = error;
                return View();
            } catch (Exception) {
                Application app = new Application();
                app.Get(id);
                return RedirectToAction("Employment", new { id = (Guid)app.job_id, app_id = id, error = "Error processing information." });
            }
        }

        /// <summary>
        /// Save military service and redirect to Review page
        /// </summary>
        /// <param name="id">Application ID</param>
        /// <returns>View</returns>
        public ActionResult SaveService(Guid id = new Guid()) {
            Application app = new Application();
            try {
                app.Get(id);

                string branch = (Request.Form["service_branch"] != null) ? Request.Form["service_branch"] : "";
                string service_start = (Request.Form["service_start"] != null && Request.Form["service_start"].Length > 0) ? Request.Form["service_start"] : "";
                string service_end = (Request.Form["service_end"] != null && Request.Form["service_end"].Length > 0) ? Request.Form["service_end"] : "";
                string rank_duties = (Request.Form["rank_duties"] != null) ? Request.Form["rank_duties"] : "";
                string additional_info = (Request.Form["additional_info"] != null) ? Request.Form["additional_info"] : "";

                if (branch.Trim().ToLower() == "enter the branch of service") {
                    branch = "";
                }

                app.SetMilitary(branch, service_start, service_end, rank_duties, additional_info);

                Guid job_id = (Guid)app.job_id;
                DisplayableJob job = JobModel.Get(job_id);
                if (job.isDriving == 1) {
                    return Redirect("/Job/Driving/" + app.id);
                } else {
                    return Redirect("/Job/Resume/" + app.id);
                }
            } catch (Exception e) {
                return Redirect("/Job/Education/" + app.job_id + "?app_id=" + id + "&error=Error saving employment: " + e.Message);
            }
        }

        public ActionResult Driving(Guid id = new Guid(), string error = "") {
            Application app = new Application();
            try {
                // Get the Application
                app.Get(id);
                ViewBag.application = app;
                if (app != null && app.id != Guid.Empty && app.dateSubmitted != null) {
                    return RedirectToAction("Review", new { id = id });
                }
                Guid job_id = (app.job_id != null) ? Guid.Parse(app.job_id.ToString()) : Guid.Empty;

                // Get the job record
                DisplayableJob job = JobModel.Get(job_id);
                List<Crumb> crumbs = job.GenerateCrumbs(id, ApplicationStates.DRIVING);
                ViewBag.crumbs = crumbs;
                ViewBag.job = job;

                // Get the states
                List<State> states = ContentModel.GetStates();
                ViewBag.states = states;

                ViewBag.error = error;

                return View();
            } catch (Exception e) {
                return Redirect("/Job/Education/" + id + "&error=" + e.Message);
            }
        }

        public ActionResult SaveDriving(Guid id = new Guid()) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                Application app = db.Applications.Where(x => x.id.Equals(id)).FirstOrDefault<Application>();

                app.dob = (Request.Form["dob"] != null && Request.Form["dob"].Length > 0) ? Convert.ToDateTime(Request.Form["dob"]) : (DateTime?)null;
                app.valid_license = (Request.Form["valid_license"] != null)? Convert.ToInt32(Request.Form["valid_license"]) : 0;
                app.vehicles_driven = (Request.Form["vehicles_driven"] != null) ? Request.Form["vehicles_driven"] : "";
                app.years_driving = (Request.Form["years_driving"] != null) ? Convert.ToInt32(Request.Form["years_driving"]) : 0;

                app.driver_license_num = (Request.Form["driver_license_num"] != null) ? Request.Form["driver_license_num"] : "";
                app.driver_license_state = (Request.Form["driver_license_state"] != null) ? Guid.Parse(Request.Form["driver_license_state"]) : Guid.Empty;
                app.driver_license_exp = (Request.Form["driver_license_exp"] != null && Request.Form["driver_license_exp"].Length > 0) ? Convert.ToDateTime(Request.Form["driver_license_exp"]) : (DateTime?)null;
                
                app.chauffeur_license_num = (Request.Form["chauffeur_license_num"] != null) ? Request.Form["chauffeur_license_num"] : "";
                app.chauffeur_license_state = (Request.Form["chauffeur_license_state"] != null) ? Guid.Parse(Request.Form["chauffeur_license_state"]) : Guid.Empty;
                app.chauffeur_license_exp = (Request.Form["chauffeur_license_exp"] != null && Request.Form["chauffeur_license_exp"].Length > 0) ? Convert.ToDateTime(Request.Form["chauffeur_license_exp"]) : (DateTime?)null;

                app.inAccidents = (Request.Form["inAccidents"] != null) ? Convert.ToInt32(Request.Form["inAccidents"]) : 0;
                app.accidents_desc = (Request.Form["accidents_desc"] != null) ? Request.Form["accidents_desc"] : "";

                app.license_suspended = (Request.Form["license_suspended"] != null) ? Convert.ToInt32(Request.Form["license_suspended"]) : 0;
                app.suspended_desc = (Request.Form["suspended_desc"] != null) ? Request.Form["suspended_desc"] : "";

                app.traffic_violations = (Request.Form["traffic_violations"] != null) ? Convert.ToInt32(Request.Form["traffic_violations"]) : 0;
                app.violations_desc = (Request.Form["violations_desc"] != null) ? Request.Form["violations_desc"] : "";

                db.SubmitChanges();

                return Redirect("/Job/Resume/" + id);
            } catch (Exception e) {
                return Redirect("/Job/Driving/" + id + "?error=" + e.Message);
            }
        }

        public ActionResult Resume(Guid id = new Guid(), string error = "") {
            Application app = new Application();
            try {
                // Get the Application
                app.Get(id);
                ViewBag.application = app;
                if (app != null && app.id != Guid.Empty && app.dateSubmitted != null) {
                    return RedirectToAction("Review", new { id = id });
                }
                Guid job_id = (app.job_id != null) ? Guid.Parse(app.job_id.ToString()) : Guid.Empty;

                // Get the job record
                DisplayableJob job = JobModel.Get(job_id);
                List<Crumb> crumbs = job.GenerateCrumbs(id, ApplicationStates.RESUME);
                ViewBag.crumbs = crumbs;
                ViewBag.job = job;

                List<ApplicationFile> files = app.ApplicationFiles.ToList<ApplicationFile>();
                ViewBag.files = files;

                ViewBag.error = (string)TempData["error"] ?? "";

                return View();
            } catch (Exception e) {
                return Redirect("/Job/Education/" + id + "&error=" + e.Message);
            }
        }

        public ActionResult SaveResume(HttpPostedFileBase file, Guid id = new Guid()) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                Application app = db.Applications.Where(x => x.id.Equals(id)).FirstOrDefault<Application>();

                if (file != null && file.ContentLength > 0) {
                    string fileName = Path.GetFileName(file.FileName);
                    string name = (Request.Form["name"] != null && Request.Form["name"].Length > 0) ? Request.Form["name"] : "";

                    Stream uploadedFile = file.InputStream;
                    int slength = Convert.ToInt32(uploadedFile.Length);
                    byte[] fileData = new byte[slength];
                    uploadedFile.Read(fileData, 0, slength);
                    uploadedFile.Close();

                    ApplicationFile afile = new ApplicationFile {
                        id = Guid.NewGuid(),
                        app_id = app.id,
                        fileName = fileName,
                        name = name,
                        size = fileData.Length,
                        fileContent = fileData
                    };

                    if (afile.isTypeAllowed() && afile.isSizeAllowed()) {
                        db.ApplicationFiles.InsertOnSubmit(afile);
                        db.SubmitChanges();
                    } else {
                        if (!afile.isSizeAllowed()) {
                            throw new Exception("File is too large!");
                        }
                        if (!afile.isTypeAllowed()) {
                            throw new Exception("File type not allowed!");
                        }
                    }
                }

                return Redirect("/Job/Resume/" + id);
            } catch (Exception e) {
                TempData["error"] = e.Message;
                return Redirect("/Job/Resume/" + id);
            }
        }

        public ActionResult DeleteFile(Guid id = new Guid(), Guid file_id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();
            Application app = db.Applications.Where(x => x.id.Equals(id)).FirstOrDefault<Application>();

            ApplicationFile file = app.ApplicationFiles.Where(x => x.id.Equals(file_id)).FirstOrDefault<ApplicationFile>();
            db.ApplicationFiles.DeleteOnSubmit(file);
            db.SubmitChanges();
            return RedirectToAction("Resume", new { id = id });
        }
        
        public void ViewFile(Guid id = new Guid(), Guid file_id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();
            Application app = db.Applications.Where(x => x.id.Equals(id)).FirstOrDefault<Application>();

            ApplicationFile file = app.ApplicationFiles.Where(x => x.id.Equals(file_id)).FirstOrDefault<ApplicationFile>();
            string mimetype = file.GetMimeType();

            string attachment = "attachment; filename=" + file.fileName;
            HttpContext.Response.Clear();
            HttpContext.Response.ClearHeaders();
            HttpContext.Response.ClearContent();
            HttpContext.Response.AddHeader("content-disposition", attachment);
            HttpContext.Response.ContentType = mimetype;
            HttpContext.Response.AddHeader("Pragma", "public");
            HttpContext.Response.BinaryWrite(file.fileContent.ToArray());
            HttpContext.Response.End();
        }
        
        public string SaveEducation(string edu = "") {
            try {
                Response.ContentType = "application/json";
                // Instantiate EducationalBackground object
                EducationalBackground education = new EducationalBackground();
                education = JsonConvert.DeserializeObject<EducationalBackground>(edu);
                if (education.id == Guid.Empty) {
                    education.id = Guid.NewGuid();
                }

                // Save object
                education.Save();
                
                string desc = education.ParseNameLocation();
                education.description = desc;

                string json = JsonConvert.SerializeObject(education, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                education.json = json;

                // Serialize saved object and return
                return JsonConvert.SerializeObject(education, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            } catch (Exception e) {
                return e.Message;
            }
        }

        public string DeleteEducation(Guid id = new Guid()) {
            try {
                Response.ContentType = "application/json";

                EducationalBackground edu = new EducationalBackground();
                edu.id = id;
                edu.Delete();

                return "";
                
            } catch (Exception e) {
                return e.Message;
            }
        }

        public string SaveReference(string reference = "") {
            try {
                Response.ContentType = "application/json";

                // Instantiate Reference object
                //HR.Reference refer = new HR.Reference();
                HR.Reference refer = JsonConvert.DeserializeObject<HR.Reference>(reference);
                if (refer.id == Guid.Empty) {
                    refer.id = Guid.NewGuid();
                }

                // Save object
                refer.Save();

                string json = JsonConvert.SerializeObject(refer, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                refer.json = json;

                // Serialize save object and return
                return JsonConvert.SerializeObject(refer, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            } catch (Exception e) {
                return e.StackTrace;
            }
        }

        public string DeleteReference(Guid id = new Guid()) {
            try {
                Response.ContentType = "application/json";

                HR.Reference refer = new HR.Reference();
                refer.id = id;
                refer.Delete();

                return "";

            } catch (Exception e) {
                return e.Message;
            }
        }

        public ActionResult Review(Guid id = new Guid()) {
            Application app = new Application();
            // Get the Application
            app.Get(id);
            ViewBag.application = app;
            Guid job_id = (app.job_id != null) ? Guid.Parse(app.job_id.ToString()) : Guid.Empty;

            // Get the job record
            DisplayableJob job = JobModel.Get(job_id);
            List<Crumb> crumbs = job.GenerateCrumbs(id, ApplicationStates.REVIEW);
            ViewBag.crumbs = crumbs;
            ViewBag.job = job;

            // Get the states
            List<State> states = ContentModel.GetStates();
            ViewBag.states = states;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Submit(Guid id = new Guid()) {
            Application app = new Application();
            // Get the Application
            app.Get(id);
            app.Submit();

            return RedirectToAction("Thanks", new { id = id });
        }

        public ActionResult Thanks(Guid id = new Guid()) {
            Application app = new Application();
            // Get the Application
            app.Get(id);
            Guid job_id = (app.job_id != null) ? Guid.Parse(app.job_id.ToString()) : Guid.Empty;
            DisplayableJob job = JobModel.Get(job_id);
            ViewBag.job = job;

            return View();
        }
    }
}
