using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models {
    public class LocationModel {

        public static List<DisplayableLocation> GetAllDisplayable() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();

                List<DisplayableLocation> locs = (from l in db.Locations
                                                  join s in db.States on l.state_id equals s.id into DefaultStates
                                                  from s1 in DefaultStates.DefaultIfEmpty()
                                                  select new DisplayableLocation {
                                                      id = l.id,
                                                      city = l.city,
                                                      state_id = l.state_id,
                                                      state = s1.state1,
                                                      abbr = s1.abbr,
                                                      listing_count = db.Jobs.Where(x => x.location == l.id).Count()
                                                  }).ToList<DisplayableLocation>();
                return locs;
            } catch (Exception) {
                return new List<DisplayableLocation>();
            }
        }

        public static Location Get(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception(); }

                JobBoardDataContext db = new JobBoardDataContext();
                return db.Locations.Where(x => x.id == id).FirstOrDefault<Location>();
            } catch (Exception) {
                throw new Exception();
            }
        }

        public static List<Job> GetJobs(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception(); }

                JobBoardDataContext db = new JobBoardDataContext();
                return db.Jobs.Where(x => x.location == id).ToList<Job>();
            } catch (Exception) {
                throw new Exception();
            }
        }

        public static void Create(string city, Guid state_id) {
            if (city.Length == 0) { throw new Exception("Invalid city."); }
            if (state_id == null || state_id == Guid.Empty) { throw new Exception("Invalid state."); }

            JobBoardDataContext db = new JobBoardDataContext();

            // Make sure we don't already have a location entry for this
            int existing = db.Locations.Where(x => x.city == city && x.state_id == state_id).Count();
            if (existing > 0) { throw new Exception("Existing entry."); }

            // Create new location
            Location loc = new Location {
                id = Guid.NewGuid(),
                city = city,
                state_id = state_id
            };

            // Save location
            db.Locations.InsertOnSubmit(loc);
            db.SubmitChanges();
        }

        public static void Update(Guid id, string city, Guid state_id) {
            if (id == null || id == Guid.Empty) { throw new Exception("Invalid location reference."); }
            if (city.Length == 0) { throw new Exception("Invalid city."); }
            if (state_id == null || state_id == Guid.Empty) { throw new Exception("Invalid state."); }

            // Get the record to be updated
            JobBoardDataContext db = new JobBoardDataContext();
            Location loc = db.Locations.Where(x => x.id == id).FirstOrDefault<Location>();

            // Update data
            loc.city = city;
            loc.state_id = state_id;

            // Save record
            db.SubmitChanges();
        }

        public static void Delete(Guid id) {
            if (id == null || id == Guid.Empty) {
                throw new Exception("Invalid reference.");
            }

            // Retrieve the record to delete
            JobBoardDataContext db = new JobBoardDataContext();
            Location loc = db.Locations.Where(x => x.id == id).FirstOrDefault<Location>();

            // Get the jobs listings for this location and set their location to blank
            List<Job> jobs = db.Jobs.Where(x => x.location == id).ToList<Job>();
            foreach (Job j in jobs) {
                j.location = Guid.Empty;
            }

            // Save changes
            db.Locations.DeleteOnSubmit(loc);
            db.SubmitChanges();
        }

        public static List<State> GetAllStates() {
            JobBoardDataContext db = new JobBoardDataContext();
            List<State> states = db.States.OrderBy(x => x.state1).ToList<State>();
            return states;
        }
    }

    public class DisplayableLocation : Location {
        public string state { get; set; }
        public string abbr { get; set; }
        public int listing_count { get; set; }
        public int jobCount { get; set; }
    }
}