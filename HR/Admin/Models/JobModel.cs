using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Admin.Models {
    public class JobModel {

        public static List<Job> GetAll() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<Job> jobs = (from j in db.Jobs
                                    where j.jobState != JobState.ARCHIVED.ToString()
                                    select j).ToList<Job>();
                return jobs;
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static List<Job> GetArchived() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<Job> jobs = (from j in db.Jobs
                                                  where j.jobState == JobState.ARCHIVED.ToString()
                                                  select j).ToList<Job>();
                return jobs;
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static List<Job> GetByCategory(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<Job> jobs = (from j in db.Jobs
                                    join jc in db.JobCategories on j.id equals jc.job
                                    where j.jobState != JobState.ARCHIVED.ToString() && jc.cat.Equals(id)
                                    select j).ToList<Job>();
                return jobs;
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static List<Job> GetByLocation(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<Job> jobs = (from j in db.Jobs
                                    where j.jobState != JobState.ARCHIVED.ToString() && j.location.Equals(id)
                                    select j).ToList<Job>();
                return jobs;
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static Job Get(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();

                return db.Jobs.Where(x => x.id == id).FirstOrDefault<Job>();
            } catch (Exception) {
                return new Job();
            }
        }

        public static Guid Create(string title, string short_desc, string salary_type, string status, string long_desc, Guid experience, Guid education, Guid location, Guid contact, int isDriving, List<Guid> categories, List<Guid> shifts) {
            JobBoardDataContext db = new JobBoardDataContext();

            // Create the new job record
            Job new_job = new Job {
                id = Guid.NewGuid(),
                title = title,
                short_desc = short_desc,
                long_desc = long_desc.Replace("\n","<br />"),
                experience = experience,
                education = education,
                location = location,
                contact = contact,
                date_added = DateTime.Now,
                isDriving = isDriving,
                salary_type = salary_type,
                status = status,
                jobState = JobState.CREATED.ToString()
            };

            // Save the job record
            db.Jobs.InsertOnSubmit(new_job);
            db.SubmitChanges();

            // Create the job categories
            foreach (Guid cat_id in categories) {
                JobCategory new_jobcat = new JobCategory {
                    job = new_job.id,
                    cat = cat_id
                };
                db.JobCategories.InsertOnSubmit(new_jobcat);
            }

            // Create the job categories
            foreach (Guid shift_id in shifts) {
                JobShift new_jobshift = new JobShift {
                    id = Guid.NewGuid(),
                    job = new_job.id,
                    shift = shift_id
                };
                db.JobShifts.InsertOnSubmit(new_jobshift);
            }
            
            db.SubmitChanges();
            return new_job.id;
        }

        public static void Update(Guid id, string title, string short_desc, string salary_type, string status, string long_desc, Guid experience, Guid education, Guid location, Guid contact, int isDriving, string jobState) {
            if (id == null || id == Guid.Empty) { throw new Exception("Invalid reference"); }

            JobBoardDataContext db = new JobBoardDataContext();
            Job job = db.Jobs.Where(x => x.id == id).FirstOrDefault<Job>();
            job.title = title;
            job.short_desc = short_desc;
            job.long_desc = long_desc.Replace("\n", "<br />");
            job.experience = experience;
            job.education = education;
            job.location = location;
            job.contact = contact;
            job.isDriving = isDriving;
            job.salary_type = salary_type;
            job.status = status;
            job.jobState = jobState.Trim().ToUpper();

            db.SubmitChanges();
        }

        public static Guid Duplicate(Guid id) {
            Guid new_id = new Guid();
            Job job = Get(id);
            List<Guid> shifts = job.JobShifts.Select(x => x.Shift.id).ToList<Guid>();
            List<Guid> cats = job.JobCategories.Select(x => x.cat).ToList<Guid>();
            List<Guid> contacts = job.Notifications.Select(x => x.contact).ToList<Guid>();
            new_id = Create(job.title, job.short_desc, job.salary_type, job.status, job.long_desc, job.experience, job.education, job.location, job.contact, job.isDriving, cats, shifts);
            
            List<JobContact> jobcontacts = new List<JobContact>();
            foreach (Guid c in contacts) {
                JobContact jc = new JobContact {
                    id = Guid.NewGuid(),
                    job = new_id,
                    contact = c
                };
                jobcontacts.Add(jc);
            }
            JobBoardDataContext db = new JobBoardDataContext();
            db.JobContacts.InsertAllOnSubmit(jobcontacts);
            db.SubmitChanges();

            return new_id;
        }

        public static string Delete(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                Job j = db.Jobs.Where(x => x.id.Equals(id)).FirstOrDefault<Job>();
                db.Jobs.DeleteOnSubmit(j);

                List<JobCategory> cats = db.JobCategories.Where(x => x.job.Equals(id)).ToList<JobCategory>();
                db.JobCategories.DeleteAllOnSubmit<JobCategory>(cats);

                List<JobShift> shifts = db.JobShifts.Where(x => x.job.Equals(id)).ToList<JobShift>();
                db.JobShifts.DeleteAllOnSubmit<JobShift>(shifts);

                List<JobContact> contacts = db.JobContacts.Where(x => x.job.Equals(id)).ToList<JobContact>();
                db.JobContacts.DeleteAllOnSubmit<JobContact>(contacts);

                List<Application> apps = db.Applications.Where(x => x.job_id.Equals(id)).ToList<Application>();
                foreach (Application app in apps) {
                    app.job_id = new Guid();
                }
                db.SubmitChanges();
                return "";
            } catch (Exception) {
                return "Error";
            }
        }

        public static string AddJobCategory(Guid job_id, Guid cat_id) {
            JobBoardDataContext db = new JobBoardDataContext();

            // Lets make sure we don't already have an association for this category
            int exists = db.JobCategories.Where(x => x.cat == cat_id && x.job == job_id).Count();
            if (exists > 0) { return "You may not add this category more than once."; }

            JobCategory jc = new JobCategory {
                job = job_id,
                cat = cat_id
            };
            db.JobCategories.InsertOnSubmit(jc);
            db.SubmitChanges();

            JobCategory json = new JobCategory {
                id = jc.id,
                job = jc.job,
                cat = jc.cat
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(json);
        }

        public static void DeleteJobCategory(Guid job_id, Guid cat_id) {
            JobBoardDataContext db = new JobBoardDataContext();

            JobCategory jc = db.JobCategories.Where(x => x.cat == cat_id && x.job == job_id).FirstOrDefault<JobCategory>();
            db.JobCategories.DeleteOnSubmit(jc);
            db.SubmitChanges();
        }

        public static string AddJobShift(Guid job_id, Guid shift_id) {
            JobBoardDataContext db = new JobBoardDataContext();

            // Lets make sure we don't already have an association for this category
            int exists = db.JobShifts.Where(x => x.shift == shift_id && x.job == job_id).Count();
            if (exists > 0) { return "You may not add this shift more than once."; }

            JobShift js = new JobShift {
                id = Guid.NewGuid(),
                job = job_id,
                shift = shift_id
            };
            db.JobShifts.InsertOnSubmit(js);
            db.SubmitChanges();

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(js);
        }

        public static void DeleteJobShift(Guid job_id, Guid shift_id) {
            JobBoardDataContext db = new JobBoardDataContext();

            JobShift js = db.JobShifts.Where(x => x.shift == shift_id && x.job == job_id).FirstOrDefault<JobShift>();
            db.JobShifts.DeleteOnSubmit(js);
            db.SubmitChanges();
        }
        public static string AddJobContact(Guid job_id, Guid contact_id) {
            JobBoardDataContext db = new JobBoardDataContext();

            // Lets make sure we don't already have an association for this category
            int exists = db.JobContacts.Where(x => x.contact == contact_id && x.job == job_id).Count();
            if (exists > 0) { return "You may not add this contact more than once."; }

            JobContact jc = new JobContact {
                id = Guid.NewGuid(),
                job = job_id,
                contact = contact_id
            };
            db.JobContacts.InsertOnSubmit(jc);
            db.SubmitChanges();

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(jc);
        }

        public static void DeleteJobContact(Guid job_id, Guid contact_id) {
            JobBoardDataContext db = new JobBoardDataContext();

            JobContact jc = db.JobContacts.Where(x => x.contact == contact_id && x.job == job_id).FirstOrDefault<JobContact>();
            db.JobContacts.DeleteOnSubmit(jc);
            db.SubmitChanges();
        }
    }

    public enum ApplicationStatus {
        ACTIVE,
        ARCHIVED,
        INACTIVE
    }

    public enum JobState {
        CREATED,
        PUBLISHED,
        ARCHIVED
    }

}