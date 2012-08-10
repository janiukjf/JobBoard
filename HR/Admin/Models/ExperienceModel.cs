using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models {
    public class ExperienceModel {

        public static List<Experience> GetAll() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Experiences.ToList<Experience>();
            } catch (Exception) {
                return new List<Experience>();
            }
        }

        public static Experience Get(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception("Invalid reference."); }

                // Get the Experience record
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Experiences.Where(x => x.id == id).FirstOrDefault<Experience>();
            } catch (Exception) {
                return new Experience();
            }
        }

        public static List<Job> GetJobs(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception("Invalid reference."); }

                // Get the jobs for the given experience level
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Jobs.Where(x => x.experience == id).ToList<Job>();
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static void Create(string experience) {
            if (experience.Length == 0) { throw new Exception("Invalid experience."); }

            JobBoardDataContext db = new JobBoardDataContext();

            // Make sure we don't already have a experience entry for this
            int existing = db.Experiences.Where(x => x.experience1 == experience).Count();
            if (existing > 0) { throw new Exception("Existing entry."); }

            // Create new experience level
            Experience exp = new Experience {
                id = Guid.NewGuid(),
                experience1 = experience,
                date_added = DateTime.Now
            };

            // Save experience
            db.Experiences.InsertOnSubmit(exp);
            db.SubmitChanges();
        }

        public static void Update(Guid id, string experience) {
            if (id == null || id == Guid.Empty) { throw new Exception("Invalid experience reference."); }
            if (experience.Length == 0) { throw new Exception("Invalid experience."); }

            // Get the record to be updated
            JobBoardDataContext db = new JobBoardDataContext();
            Experience exp = db.Experiences.Where(x => x.id == id).FirstOrDefault<Experience>();

            // Update data
            exp.experience1 = experience;

            // Save record
            db.SubmitChanges();
        }

        public static void Delete(Guid id) {
            if (id == null || id == Guid.Empty) {
                throw new Exception("Invalid reference.");
            }

            // Retrieve the record to delete
            JobBoardDataContext db = new JobBoardDataContext();
            Experience exp = db.Experiences.Where(x => x.id == id).FirstOrDefault<Experience>();

            // Get the jobs listings for this expeience level and set their experience to blank
            List<Job> jobs = db.Jobs.Where(x => x.experience == id).ToList<Job>();
            foreach (Job j in jobs) {
                j.experience = Guid.Empty;
            }

            // Save changes
            db.Experiences.DeleteOnSubmit(exp);
            db.SubmitChanges();
        }
    }
}