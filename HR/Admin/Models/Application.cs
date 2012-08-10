using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Net.Mail;
using Admin.Models;

namespace Admin {
    partial class Application {

        public Job job { get; set; }

        public List<Application> GetAll(Guid userid) {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Application> applications = new List<Application>();
            Contact user = ContactModel.Get(userid);
            if(user.isAdmin()) {
                applications = db.Applications.Where(x => x.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !x.dateSubmitted.Equals(null)).ToList<Application>();
            } else {
                JobContact c = new JobContact {
                    contact = user.id
                };
                List<Guid> jobs = db.Jobs.Where(x => x.Notifications.Contains(c, new JobContactEqualityComparer())).Select(x => x.id).ToList<Guid>();
                applications = db.Applications.Where(x => x.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !x.dateSubmitted.Equals(null) && jobs.Contains((Guid)x.job_id)).ToList<Application>();
            }
            return applications;
        }

        public List<Application> GetArchive(Guid userid) {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Application> applications = new List<Application>();
            Contact user = ContactModel.Get(userid);
            if (user.isAdmin()) {
                applications = (from a in db.Applications
                                join j in db.Jobs on a.job_id equals j.id
                                where (a.status.Equals(ApplicationStatus.ARCHIVED.ToString()) || j.jobState.Equals(JobState.ARCHIVED.ToString()))
                                && !a.dateSubmitted.Equals(null)
                                select a).ToList<Application>();
            } else {
                JobContact c = new JobContact {
                    contact = user.id
                };
                applications = (from a in db.Applications
                                join j in db.Jobs on a.job_id equals j.id
                                where (a.status.Equals(ApplicationStatus.ARCHIVED.ToString()) || j.jobState.Equals(JobState.ARCHIVED.ToString()))
                                && !a.dateSubmitted.Equals(null) && (j.Notifications.Contains(c,new JobContactEqualityComparer()))
                                select a).ToList<Application>();
            }
            return applications;
        }

        public void Get(Guid id = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();
            Application app = db.Applications.Where(x => x.id.Equals(id)).FirstOrDefault<Application>();
            var props = this.GetType().GetProperties();

            foreach (PropertyInfo prop in props) {
                if (app != null) {
                    prop.SetValue(this, app.GetType().GetProperty(prop.Name).GetValue(app, null), null);
                }
            }
        }

        public bool isArchived() {
            if (this.status.Equals(ApplicationStatus.ARCHIVED.ToString())) {
                return true;
            }
            return false;
        }

        public void Archive() {
            JobBoardDataContext db = new JobBoardDataContext();
            Application app = db.Applications.Where(x => x.id.Equals(this.id)).FirstOrDefault<Application>();
            app.status = ApplicationStatus.ARCHIVED.ToString();
            db.SubmitChanges();
        }

        public List<Application> GetByCategory(Guid id, Guid userid) {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Application> applications = new List<Application>();
            Contact user = ContactModel.Get(userid);
            if (user.isAdmin()) {
                applications = (from c in db.Categories
                                join jc in db.JobCategories on c.id equals jc.cat
                                join j in db.Jobs on jc.job equals j.id
                                join a in db.Applications on j.id equals a.job_id
                                where c.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            } else {
                JobContact contact = new JobContact {
                    contact = user.id
                };
                applications = (from c in db.Categories
                                join jc in db.JobCategories on c.id equals jc.cat
                                join j in db.Jobs on jc.job equals j.id
                                join a in db.Applications on j.id equals a.job_id
                                where c.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.Notifications.Contains(contact, new JobContactEqualityComparer())
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            }
            return applications;
        }

        public List<Application> GetByLocation(Guid id, Guid userid) {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Application> applications = new List<Application>();
            Contact user = ContactModel.Get(userid);
            if (user.isAdmin()) {
                applications = (from l in db.Locations
                                join j in db.Jobs on l.id equals j.location
                                join a in db.Applications on j.id equals a.job_id
                                where l.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            } else {
                JobContact contact = new JobContact {
                    contact = user.id
                };
                applications = (from l in db.Locations
                                join j in db.Jobs on l.id equals j.location
                                join a in db.Applications on j.id equals a.job_id
                                where l.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.Notifications.Contains(contact, new JobContactEqualityComparer())
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            }
            return applications;
        }

        public List<Application> GetByEducation(Guid id, Guid userid) {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Application> applications = new List<Application>();
            Contact user = ContactModel.Get(userid);
            if (user.isAdmin()) {
                applications = (from e in db.Educations
                                join j in db.Jobs on e.id equals j.education
                                join a in db.Applications on j.id equals a.job_id
                                where e.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            } else {
                JobContact contact = new JobContact {
                    contact = user.id
                };
                applications = (from e in db.Educations
                                join j in db.Jobs on e.id equals j.education
                                join a in db.Applications on j.id equals a.job_id
                                where e.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.Notifications.Contains(contact, new JobContactEqualityComparer())
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            }
            return applications;
        }

        public List<Application> GetByExperience(Guid id, Guid userid) {
            JobBoardDataContext db = new JobBoardDataContext();
            List<Application> applications = new List<Application>();
            Contact user = ContactModel.Get(userid);
            if (user.isAdmin()) {
                applications = (from e in db.Experiences
                                join j in db.Jobs on e.id equals j.experience
                                join a in db.Applications on j.id equals a.job_id
                                where e.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            } else {
                JobContact contact = new JobContact {
                    contact = user.id
                };
                applications = (from e in db.Experiences
                                join j in db.Jobs on e.id equals j.experience
                                join a in db.Applications on j.id equals a.job_id
                                where e.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                && j.Notifications.Contains(contact, new JobContactEqualityComparer())
                                && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select a).ToList<Application>();
            }
            return applications;
        }

        public void GetEmployments() {
            JobBoardDataContext db = new JobBoardDataContext();
            List<EmploymentHistory> emp_history = new List<EmploymentHistory>();

            emp_history = db.EmploymentHistories.Where(x => x.app_id.Equals(this.id)).ToList<EmploymentHistory>();
        }



    }

    partial class EducationalBackground {

        public string description { get; set; }
        public string json { get; set; }

        public string ParseNameLocation(int max_length = 500) {
            JobBoardDataContext db = new JobBoardDataContext();
            string state_abbr = db.States.Where(x => x.id.Equals(this.state_id)).Select(x => x.abbr).FirstOrDefault<string>();
            string full_desc = String.Format("{0}: {1} {2}, {3}", this.name, this.address, this.city, state_abbr);
            if (full_desc.Length > max_length) {
                return full_desc.Substring(0, 500) + "...";
            } else {
                return full_desc;
            }
        }

    }

    partial class Reference {
        public string json { get; set; }
    }

    class JobContactEqualityComparer : IEqualityComparer<JobContact> {

        public bool Equals(JobContact c1, JobContact c2) {
            if (c1.contact == c2.contact) {
                return true;
            } else {
                return false;
            }
        }

        public int GetHashCode(JobContact c) {
            return c.GetHashCode();
        }

    }
}