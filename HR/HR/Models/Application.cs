using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Net.Mail;

namespace HR {
    partial class Application {

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

        public void GetEmployments() {
            JobBoardDataContext db = new JobBoardDataContext();
            List<EmploymentHistory> emp_history = new List<EmploymentHistory>();

            emp_history = db.EmploymentHistories.Where(x => x.app_id.Equals(this.id)).ToList<EmploymentHistory>();
        }

        public void DeleteEmployment(Guid id) {
            JobBoardDataContext db = new JobBoardDataContext();
            EmploymentHistory emp = db.EmploymentHistories.Where(x => x.id.Equals(id)).FirstOrDefault<EmploymentHistory>();

            db.EmploymentHistories.DeleteOnSubmit(emp);
            db.SubmitChanges();
        }

        public void SetMilitary(string branch, string start, string end, string rank_duties, string additional_info) {
            Application app = new Application();
            JobBoardDataContext db = new JobBoardDataContext();
            DateTime? startdate = null;
            DateTime? enddate = null;
            try {
                startdate = Convert.ToDateTime(start);
            } catch {}
            try {
                enddate = Convert.ToDateTime(end);
            } catch { }

            app = db.Applications.Where(x => x.id.Equals(this.id)).FirstOrDefault<Application>();
            app.service_branch = branch;
            app.service_start = startdate;
            app.service_end = enddate;
            app.rank_duties = rank_duties;
            app.additional_info = additional_info;

            db.SubmitChanges();
        }

        public void Submit() {
            JobBoardDataContext db = new JobBoardDataContext();
            Application app = new Application();
            app = db.Applications.Where(x => x.id.Equals(this.id)).FirstOrDefault<Application>();
            app.dateSubmitted = DateTime.Now;
            db.SubmitChanges();

            SendNotifications();
        }

        private void SendNotifications() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                SmtpClient SmtpServer = new SmtpClient();

                Job job = db.Jobs.Where(x => x.id.Equals(this.job_id)).FirstOrDefault<Job>();
                List<Contact> receivers = job.Notifications.Select(x => x.Contact).ToList<Contact>();

                foreach (Contact receiver in receivers) {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(receiver.email);
                    mail.Subject = "New Application Submitted for " + job.title;
                    mail.IsBodyHtml = true;
                    Uri url = HttpContext.Current.Request.Url;
                    string path = "http://" + url.Host + ((url.Port != 80) ? ":" + url.Port.ToString() : "") + "/Admin/Application/" + this.id;
                    string htmlBody = "<p>Hi " + receiver.name + ",</p>";
                    htmlBody += "<p>Someone has submitted a new application for the " + job.title + " position.<br /><br />";
                    htmlBody += "<a href=\"" + path + "\">View Application</a>";
                    mail.Body = htmlBody;
                    SmtpServer.Send(mail);
                }

            } catch { }
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

        public void Save() {
            JobBoardDataContext db = new JobBoardDataContext();

            // Attempt to find a record with this ID
            EducationalBackground existing = db.EducationalBackgrounds.Where(x => x.id.Equals(this.id)).FirstOrDefault<EducationalBackground>();

            if (existing == null) { // Insert
                db.EducationalBackgrounds.InsertOnSubmit(this);
            } else { // Update
                existing.school_type = this.school_type;
                existing.name = this.name;
                existing.address = this.address;
                existing.city = this.city;
                existing.state_id = this.state_id;
                existing.years_completed = this.years_completed;
                existing.gpa = this.gpa;
                existing.degree = this.degree;
                existing.app_id = this.app_id;
                existing.description = this.ParseNameLocation();
            }
            db.SubmitChanges();

        }

        public void Delete() {
            JobBoardDataContext db = new JobBoardDataContext();

            EducationalBackground edu = db.EducationalBackgrounds.Where(x => x.id.Equals(this.id)).FirstOrDefault<EducationalBackground>();

            db.EducationalBackgrounds.DeleteOnSubmit(edu);
            db.SubmitChanges();
        }
    }

    partial class Reference {
        public string json { get; set; }

        public void Save() {

            JobBoardDataContext db = new JobBoardDataContext();

            // Attempt to find a record with this ID
            HR.Reference existing = db.References.Where(x => x.id.Equals(this.id)).FirstOrDefault<HR.Reference>();

            if (existing == null) {
                db.References.InsertOnSubmit(this);
            } else {
                existing.name = this.name;
                existing.job_title = this.job_title;
                existing.phone = this.phone;
                existing.years_known = this.years_known;
            }
            db.SubmitChanges();
        }

        public void Delete() {
            JobBoardDataContext db = new JobBoardDataContext();

            HR.Reference refer = db.References.Where(x => x.id.Equals(this.id)).FirstOrDefault<HR.Reference>();

            db.References.DeleteOnSubmit(refer);
            db.SubmitChanges();
        }
    }
}