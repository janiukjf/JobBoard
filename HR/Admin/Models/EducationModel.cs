using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models {
    public class EducationModel {

        public static List<Education> GetAll() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Educations.ToList<Education>();
            } catch (Exception) {
                return new List<Education>();
            }
        }

        public static Education Get(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception("Invalid reference."); }

                // Get the Education record
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Educations.Where(x => x.id == id).FirstOrDefault<Education>();
            } catch (Exception) {
                return new Education();
            }
        }

        public static List<Job> GetJobs(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception("Invalid reference."); }

                // Get the jobs for the given experience level
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Jobs.Where(x => x.education == id).ToList<Job>();
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static void Create(string education) {
            if (education.Length == 0) { throw new Exception("Invalid experience."); }

            JobBoardDataContext db = new JobBoardDataContext();

            // Make sure we don't already have a education entry for this
            int existing = db.Educations.Where(x => x.edu_level == education).Count();
            if (existing > 0) { throw new Exception("Existing entry."); }

            // Create new education level
            Education ed = new Education {
                id = Guid.NewGuid(),
                edu_level = education,
                date_added = DateTime.Now
            };

            // Save education
            db.Educations.InsertOnSubmit(ed);
            db.SubmitChanges();
        }

        public static void Update(Guid id, string education) {
            if (id == null || id == Guid.Empty) { throw new Exception("Invalid education reference."); }
            if (education.Length == 0) { throw new Exception("Invalid education."); }

            // Get the record to be updated
            JobBoardDataContext db = new JobBoardDataContext();
            Education ed = db.Educations.Where(x => x.id == id).FirstOrDefault<Education>();

            // Update data
            ed.edu_level = education;

            // Save record
            db.SubmitChanges();
        }

        public static void Delete(Guid id) {
            if (id == null || id == Guid.Empty) {
                throw new Exception("Invalid reference.");
            }

            // Retrieve the record to delete
            JobBoardDataContext db = new JobBoardDataContext();
            Education ed = db.Educations.Where(x => x.id == id).FirstOrDefault<Education>();

            // Get the jobs listings for this education level and set their education to blank
            List<Job> jobs = db.Jobs.Where(x => x.education == id).ToList<Job>();
            foreach (Job j in jobs) {
                j.experience = Guid.Empty;
            }

            // Save changes
            db.Educations.DeleteOnSubmit(ed);
            db.SubmitChanges();
        }
    }
}