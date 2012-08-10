using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Models {
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
    }
}