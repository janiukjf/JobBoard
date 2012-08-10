using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Net.Mail;
using Admin.Models;

namespace Admin {
    partial class Experience {

        public int applicationCount { get; set; }

        public void getAppCount(Guid userid = new Guid()) {
            JobBoardDataContext db = new JobBoardDataContext();
            Contact user = ContactModel.Get(userid);
            if (user.isAdmin()) {
                this.applicationCount = (from e in db.Experiences
                                         join j in db.Jobs on e.id equals j.experience
                                         join a in db.Applications on j.id equals a.job_id
                                         where e.id.Equals(this.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                         && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         select a).Count();
            } else {
                JobContact contact = new JobContact {
                    contact = user.id
                };
                this.applicationCount = (from e in db.Experiences
                                         join j in db.Jobs on e.id equals j.experience
                                         join a in db.Applications on j.id equals a.job_id
                                         where e.id.Equals(this.id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         && a.status.Equals(ApplicationStatus.ACTIVE.ToString()) && !a.dateSubmitted.Equals(null)
                                         && j.Notifications.Contains(contact, new JobContactEqualityComparer())
                                         && j.jobState.Equals(JobState.PUBLISHED.ToString())
                                         select a).Count();
            }
        }
    }
}