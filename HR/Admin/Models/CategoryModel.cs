using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models {
    public class CategoryModel {

        /// <summary>
        /// Gets all the Category records with references to their parent category
        /// </summary>
        /// <returns>List of CategoryWithParent</returns>
        public static List<CategoryWithParent> GetAll() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<CategoryWithParent> cats = (from c in db.Categories
                                                 join c2 in db.Categories on c.parent equals c2.id into DefaultCats
                                                 from c3 in DefaultCats.DefaultIfEmpty()
                                                 select new CategoryWithParent {
                                                     id = c.id,
                                                     name = c.name,
                                                     short_desc = c.short_desc,
                                                     long_desc = c.long_desc,
                                                     date_added = c.date_added,
                                                     parent = c.parent,
                                                     parent_name = c3.name
                                                 }).ToList<CategoryWithParent>();
                return cats;
            } catch (Exception) {
                return new List<CategoryWithParent>();
            }
        }

        /// <summary>
        /// Gets all the basic category records from the database
        /// </summary>
        /// <returns>List of Category</returns>
        public static List<Category> GetCategories() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Categories.ToList<Category>();
            } catch (Exception) {
                return new List<Category>();
            }
        }

        public static List<Category> GetParentCategories() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Categories.Where(x => x.parent == null || x.parent == Guid.Empty).ToList<Category>();
            } catch (Exception) {
                return new List<Category>();
            }
        }

        public static Category Get(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                return db.Categories.Where(x => x.id == id).FirstOrDefault<Category>();
            } catch (Exception) {
                return new Category();
            }
        }

        public static List<Job> GetListings(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<Job> jobs = (from j in db.Jobs
                                  join jc in db.JobCategories on j.id equals jc.job
                                  where jc.cat.Equals(id)
                                  select j).ToList<Job>();
                return jobs;
            } catch (Exception) {
                return new List<Job>();
            }
        }

        public static string Create(string name, string short_desc, string long_desc, Guid parent) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                if (parent == null || parent == Guid.Empty) {
                    int parent_count = db.Categories.Where(x => x.parent == null || x.parent == Guid.Empty).Count();
                    if (parent_count >= 4) {
                        throw new Exception("There are already (4) parent categories in the system. You may not exceed (4) parent categories. Please add the category a sub-category");
                    }
                }
                Category cat = new Category {
                    id = Guid.NewGuid(),
                    name = name,
                    short_desc = short_desc,
                    long_desc = long_desc,
                    date_added = DateTime.Now,
                    parent = parent
                };
                db.Categories.InsertOnSubmit(cat);
                db.SubmitChanges();

                return "";
            } catch (Exception e) {
                return e.Message;
            }
        }

        public static string Update(Guid id, string name, string short_desc, string long_desc, Guid parent) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                if (parent == null || parent == Guid.Empty) { // Check to make sure we don't already have 4 parent categories
                    int parent_count = db.Categories.Where(x => x.parent == null || x.parent == Guid.Empty).Count();
                    if (parent_count >= 4) {
                        throw new Exception("There are already (4) parent categories in the system. You may not exceed (4) parent categories. Please add the category a sub-category");
                    }
                }

                Category cat = db.Categories.Where(x => x.id == id).FirstOrDefault<Category>();
                cat.name = name;
                cat.short_desc = short_desc;
                cat.long_desc = long_desc;
                cat.parent = parent;

                db.SubmitChanges();

                return "";
            } catch (Exception e) {
                return e.Message;
            }
        }

        public static string Delete(Guid id) {
            try {
                if (id == null || id == Guid.Empty) { throw new Exception("Invalid category"); }

                JobBoardDataContext db = new JobBoardDataContext();

                // Get the category record
                Category cat = db.Categories.Where(x => x.id == id).FirstOrDefault<Category>();

                // We need to remove any job listing references to this category
                List<JobCategory> jobs = db.JobCategories.Where(x => x.cat == id).ToList<JobCategory>();
                foreach (JobCategory jc in jobs) {
                    db.JobCategories.DeleteOnSubmit(jc);
                }

                db.Categories.DeleteOnSubmit(cat);
                db.SubmitChanges();

                return "";
            } catch (Exception e) {
                return e.Message;
            }
        }
    }

    public class CategoryWithParent : Category {
        public string parent_name { get; set; }
        public int jobCount { get; set; }
    }
}