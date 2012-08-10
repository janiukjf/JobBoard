using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Models;

namespace HR {
    partial class Category {
        private Category _parentCategory;
        public Category parentCategory {
            get { return _parentCategory; }
            set { _parentCategory = value; }
        }

        private List<Category> _subs;
        public List<Category> subs {
            get { return _subs; }
            set { _subs = value; }
        }

        private List<DisplayableJob> _jobs;
        public List<DisplayableJob> jobs {
            get {
                if (_jobs != null) {
                    return _jobs;
                } else {
                    return new List<DisplayableJob>();
                }
            }
            set { _jobs = value; }
        }

        public Category(Guid id = new Guid()) {
            if (id != null) {
                JobBoardDataContext db = new JobBoardDataContext();
                Category cat = db.Categories.Where(x => x.id.Equals(id)).FirstOrDefault<Category>();
                this.id = cat.id;
                this.name = cat.name;
                this.short_desc = cat.short_desc;
                this.long_desc = cat.long_desc;
                this.date_added = cat.date_added;
                this.parent = cat.parent;
            }
        }

        public void LoadParent() {
            JobBoardDataContext db = new JobBoardDataContext();
            Category parent = db.Categories.Where(x => x.id.Equals(this.parent)).FirstOrDefault<Category>();
            this.parentCategory = parent;
        }

        public void LoadSubs() {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Category> cats = db.Categories.Where(x => x.parent.Equals(this.id)).ToList<Category>();
            this.subs = cats;
        }

        public void LoadJobs() {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Guid> job_ids = (from jc in db.JobCategories
                                  join j in db.Jobs on jc.job equals j.id
                                  where jc.cat.Equals(this.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                  select jc.job).ToList<Guid>();

            List<DisplayableJob> cat_jobs = new List<DisplayableJob>();
            foreach (Guid j in job_ids) {
                DisplayableJob job = JobModel.Get(j);
                if (job != null) {
                    cat_jobs.Add(job);
                }
            }
            cat_jobs.OrderBy(x => x.date_added);
            this.jobs = cat_jobs;
        }
    }
}