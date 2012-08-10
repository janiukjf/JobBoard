using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Models {
    public class ContentModel {

        public static List<TieredCategories> GetMenu() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<TieredCategories> cats = new List<TieredCategories>();

                // Get the parent categories
                List<Category> parents = db.Categories.Where(x => x.parent.Equals(Guid.Empty)).ToList<Category>();
                foreach (Category parent in parents) {
                    // Get the subs of this category
                    List<Category> subs = (from c in db.Categories
                                           join jc in db.JobCategories on c.id equals jc.cat
                                           join j in db.Jobs on jc.job equals j.id
                                           where c.parent.Equals(parent.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                           select c).ToList<Category>();

                    // Check to see if there are any jobs tied to this category
                    //int job_count = db.JobCategories.Where(x => x.cat == parent.id).Count();
                    int job_count = (from jc in db.JobCategories
                                          join j in db.Jobs on jc.job equals j.id
                                          where jc.cat.Equals(parent.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                          select jc.job).Count();


                    if (subs.Count > 0 || job_count > 0) {
                        TieredCategories cat = new TieredCategories {
                            id = parent.id,
                            name = parent.name,
                            short_desc = parent.short_desc,
                            long_desc = parent.long_desc,
                            date_added = parent.date_added,
                            parent = parent.parent,
                            subs = subs,
                            jobCount = job_count
                        };
                        cats.Add(cat);
                    }
                }

                /*List<TieredCategories> cats = (from c in db.Categories
                                               from parent_jobs in c.JobCategories.ToList()
                                               join c2 in db.Categories on c.id equals c2.parent into SubCats
                                               from sub_jobs in SubCats.ToList()
                                               where (c.parent.Equals(Guid.Empty) || sub_jobs.JobCategories.Count > 0) && (parent_jobs.Jobs.Count > 0 || sub_jobs.JobCategories.Count > 0)
                                               select new TieredCategories {
                                                   id = c.id,
                                                   name = c.name,
                                                   short_desc = c.short_desc,
                                                   long_desc = c.long_desc,
                                                   date_added = c.date_added,
                                                   parent = c.parent,
                                                   subs = SubCats.ToList<Category>()
                                               }).ToList<TieredCategories>();*/
                return cats;
            } catch (Exception) {
                return new List<TieredCategories>();
            }
        }

        public static List<Education> GetEducation() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Educations.ToList<Education>();
            } catch (Exception) {
                return new List<Education>();
            }
        }

        public static List<DisplayableLocation> GetLocations() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<DisplayableLocation> locs = (from l in db.Locations
                                                  join s in db.States on l.state_id equals s.id
                                                  select new DisplayableLocation {
                                                      id = l.id,
                                                      city = l.city,
                                                      state_id = l.state_id,
                                                      state = s.state1,
                                                      abbr = s.abbr
                                                  }).ToList<DisplayableLocation>();
                return locs;
            } catch (Exception) {
                return new List<DisplayableLocation>();
            }
        }

        public static List<Experience> GetExperience() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Experiences.ToList<Experience>();
            } catch (Exception) {
                return new List<Experience>();
            }
        }

        public static List<State> GetStates() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.States.OrderBy(x => x.state1).ToList<State>();
            } catch (Exception) {
                return new List<State>();
            }
        }
    }

    public class DisplayableLocation : Location {
        public string state { get; set; }
        public string abbr { get; set; }
    }
}