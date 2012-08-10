using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models {
    public class ShiftModel {

        public static List<Shift> GetAll() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Shifts.ToList<Shift>();
            } catch (Exception) {
                return new List<Shift>();
            }
        }

        public static Shift Get(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception("Invalid reference."); }

                // Get the Shift record
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Shifts.Where(x => x.id == id).FirstOrDefault<Shift>();
            } catch (Exception) {
                return new Shift();
            }
        }

        public static List<Job> GetJobs(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception("Invalid reference."); }

                // Get the jobs for the given experience level
                JobBoardDataContext db = new JobBoardDataContext();
                List<Job> jobs = (from j in db.Jobs
                                  join js in db.JobShifts on j.id equals js.job
                                  where js.shift.Equals(id)
                                  select j).ToList<Job>();
                return jobs;
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static void Create(string shift) {
            if (shift.Length == 0) { throw new Exception("Invalid shift."); }

            JobBoardDataContext db = new JobBoardDataContext();

            // Make sure we don't already have a education entry for this
            int existing = db.Shifts.Where(x => x.shift1 == shift).Count();
            if (existing > 0) { throw new Exception("Existing entry."); }

            // Create new education level
            Shift sh = new Shift {
                id = Guid.NewGuid(),
                shift1 = shift,
                date_added = DateTime.Now
            };

            // Save education
            db.Shifts.InsertOnSubmit(sh);
            db.SubmitChanges();
        }

        public static void Update(Guid id, string shift) {
            if (id == null || id == Guid.Empty) { throw new Exception("Invalid education reference."); }
            if (shift.Length == 0) { throw new Exception("Invalid shift."); }

            // Get the record to be updated
            JobBoardDataContext db = new JobBoardDataContext();
            Shift sh = db.Shifts.Where(x => x.id == id).FirstOrDefault<Shift>();

            // Update data
            sh.shift1 = shift;

            // Save record
            db.SubmitChanges();
        }

        public static void Delete(Guid id) {
            if (id == null || id == Guid.Empty) {
                throw new Exception("Invalid reference.");
            }

            // Retrieve the record to delete
            JobBoardDataContext db = new JobBoardDataContext();
            Shift shift = db.Shifts.Where(x => x.id == id).FirstOrDefault<Shift>();

            // Get the jobs listings for this education level and set their education to blank
            List<JobShift> jobs = (from js in db.JobShifts
                                  where js.shift.Equals(shift.id)
                                  select js).ToList<JobShift>();
            db.JobShifts.DeleteAllOnSubmit(jobs);
            db.SubmitChanges();

            // Save changes
            db.Shifts.DeleteOnSubmit(shift);
            db.SubmitChanges();
        }
    }
}