using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace HR.Models {
    public class JobModel {

        public static List<DisplayableJob> GetAll(bool recent_first = false, int count = 0) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                List<DisplayableJob> jobs = new List<DisplayableJob>();
                #region
                if (recent_first) {
                    if (count > 0) {
                        jobs = (from j in db.Jobs
                                join l in db.Locations on j.location equals l.id into DefaultLocs
                                from dl in DefaultLocs.DefaultIfEmpty()
                                join s in db.States on dl.state_id equals s.id
                                join ed in db.Educations on j.education equals ed.id into DefaultEds
                                from ded in DefaultEds.DefaultIfEmpty()
                                join ex in db.Experiences on j.experience equals ex.id into DefaultExps
                                from dex in DefaultExps.DefaultIfEmpty()
                                join con in db.Contacts on j.contact equals con.id into DefaultCon
                                from dcon in DefaultCon.DefaultIfEmpty()
                                where j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select new DisplayableJob{
                                    id = j.id,
                                    title = j.title,
                                    short_desc = j.short_desc,
                                    long_desc = j.long_desc,
                                    date_added = j.date_added,
                                    experience = j.experience,
                                    exp = dex,
                                    education = j.education,
                                    ed = ded,
                                    location = j.location,
                                    loc = new DisplayableLocation { city = dl.city, state = s.state1, abbr = s.abbr },
                                    JobCategories = j.JobCategories,
                                    JobShifts = j.JobShifts,
                                    contact = j.contact,
                                    contact_info = (from c in db.Contacts
                                                    join cs in db.States on c.state_id equals cs.id
                                                    where c.id.Equals(j.contact)
                                                    select new DisplayableContact {
                                                        id = c.id,
                                                        name = c.name,
                                                        street = c.street,
                                                        city = c.city,
                                                        state_id = c.state_id,
                                                        phone = c.phone,
                                                        fax = c.fax,
                                                        email = c.email,
                                                        abbr = cs.abbr
                                                    }).FirstOrDefault<DisplayableContact>()
                                }).OrderByDescending(x => x.date_added).Take(count).ToList<DisplayableJob>();
                    } else {
                        jobs = (from j in db.Jobs
                                join l in db.Locations on j.location equals l.id into DefaultLocs
                                from dl in DefaultLocs.DefaultIfEmpty()
                                join s in db.States on dl.state_id equals s.id
                                join ed in db.Educations on j.education equals ed.id into DefaultEds
                                from ded in DefaultEds.DefaultIfEmpty()
                                join ex in db.Experiences on j.experience equals ex.id into DefaultExps
                                from dex in DefaultExps.DefaultIfEmpty()
                                where j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select new DisplayableJob {
                                    id = j.id,
                                    title = j.title,
                                    short_desc = j.short_desc,
                                    long_desc = j.long_desc,
                                    date_added = j.date_added,
                                    experience = j.experience,
                                    exp = dex,
                                    education = j.education,
                                    ed = ded,
                                    location = j.location,
                                    loc = new DisplayableLocation { city = dl.city, state = s.state1, abbr = s.abbr },
                                    JobCategories = j.JobCategories,
                                    JobShifts = j.JobShifts,
                                    contact = j.contact,
                                    contact_info = (from c in db.Contacts
                                                    join cs in db.States on c.state_id equals cs.id
                                                    where c.id.Equals(j.contact)
                                                    select new DisplayableContact{
                                                        id = c.id,
                                                        name = c.name,
                                                        street = c.street,
                                                        city = c.city,
                                                        state_id = c.state_id,
                                                        phone = c.phone,
                                                        fax = c.fax,
                                                        email = c.email,
                                                        abbr = cs.abbr
                                                    }).FirstOrDefault<DisplayableContact>()
                                }).OrderByDescending(x => x.date_added).ToList<DisplayableJob>();
                    }
                } else {
                    if (count > 0) {
                        jobs = (from j in db.Jobs
                                join l in db.Locations on j.location equals l.id into DefaultLocs
                                from dl in DefaultLocs.DefaultIfEmpty()
                                join s in db.States on dl.state_id equals s.id
                                join ed in db.Educations on j.education equals ed.id into DefaultEds
                                from ded in DefaultEds.DefaultIfEmpty()
                                join ex in db.Experiences on j.experience equals ex.id into DefaultExps
                                from dex in DefaultExps.DefaultIfEmpty()
                                join con in db.Contacts on j.contact equals con.id into DefaultCon
                                from dcon in DefaultCon.DefaultIfEmpty()
                                where j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select new DisplayableJob {
                                    id = j.id,
                                    title = j.title,
                                    short_desc = j.short_desc,
                                    long_desc = j.long_desc,
                                    date_added = j.date_added,
                                    experience = j.experience,
                                    exp = dex,
                                    education = j.education,
                                    ed = ded,
                                    location = j.location,
                                    loc = new DisplayableLocation { city = dl.city, state = s.state1, abbr = s.abbr },
                                    JobCategories = j.JobCategories,
                                    JobShifts = j.JobShifts,
                                    contact = j.contact,
                                    contact_info = (from c in db.Contacts
                                                    join cs in db.States on c.state_id equals cs.id
                                                    where c.id.Equals(j.contact)
                                                    select new DisplayableContact {
                                                        id = c.id,
                                                        name = c.name,
                                                        street = c.street,
                                                        city = c.city,
                                                        state_id = c.state_id,
                                                        phone = c.phone,
                                                        fax = c.fax,
                                                        email = c.email,
                                                        abbr = cs.abbr
                                                    }).FirstOrDefault<DisplayableContact>()
                                }).Take(count).ToList<DisplayableJob>();
                    } else {
                        jobs = (from j in db.Jobs
                                join l in db.Locations on j.location equals l.id into DefaultLocs
                                from dl in DefaultLocs.DefaultIfEmpty()
                                join s in db.States on dl.state_id equals s.id
                                join ed in db.Educations on j.education equals ed.id into DefaultEds
                                from ded in DefaultEds.DefaultIfEmpty()
                                join ex in db.Experiences on j.experience equals ex.id into DefaultExps
                                from dex in DefaultExps.DefaultIfEmpty()
                                join con in db.Contacts on j.contact equals con.id into DefaultCon
                                from dcon in DefaultCon.DefaultIfEmpty()
                                where j.jobState.Equals(JobState.PUBLISHED.ToString())
                                select new DisplayableJob {
                                    id = j.id,
                                    title = j.title,
                                    short_desc = j.short_desc,
                                    long_desc = j.long_desc,
                                    date_added = j.date_added,
                                    experience = j.experience,
                                    exp = dex,
                                    education = j.education,
                                    ed = ded,
                                    location = j.location,
                                    loc = new DisplayableLocation { city = dl.city, state = s.state1, abbr = s.abbr },
                                    JobCategories = j.JobCategories,
                                    JobShifts = j.JobShifts,
                                    contact = j.contact,
                                    contact_info = (from c in db.Contacts
                                                    join cs in db.States on c.state_id equals cs.id
                                                    where c.id.Equals(j.contact)
                                                    select new DisplayableContact {
                                                        id = c.id,
                                                        name = c.name,
                                                        street = c.street,
                                                        city = c.city,
                                                        state_id = c.state_id,
                                                        phone = c.phone,
                                                        fax = c.fax,
                                                        email = c.email,
                                                        abbr = cs.abbr
                                                    }).FirstOrDefault<DisplayableContact>()
                                }).ToList<DisplayableJob>();
                    }
                }
                #endregion
                return jobs;
            } catch (Exception) {
                return new List<DisplayableJob>();
            }
        }

        public static DisplayableJob Get(Guid id) {
            try {
                if (id == null) { throw new Exception(); }

                JobBoardDataContext db = new JobBoardDataContext();
                return (from j in db.Jobs
                        join l in db.Locations on j.location equals l.id into DefaultLocs
                        from dl in DefaultLocs.DefaultIfEmpty()
                        join s in db.States on dl.state_id equals s.id
                        join ed in db.Educations on j.education equals ed.id into DefaultEds
                        from ded in DefaultEds.DefaultIfEmpty()
                        join ex in db.Experiences on j.experience equals ex.id into DefaultExps
                        from dex in DefaultExps.DefaultIfEmpty()
                        join con in db.Contacts on j.contact equals con.id into DefaultCon
                        from dcon in DefaultCon.DefaultIfEmpty()
                        where j.id.Equals(id) && j.jobState.Equals(JobState.PUBLISHED.ToString())
                        select new DisplayableJob {
                            id = j.id,
                            title = j.title,
                            short_desc = j.short_desc,
                            long_desc = j.long_desc,
                            status = j.status,
                            salary_type = j.salary_type,
                            date_added = j.date_added,
                            experience = j.experience,
                            isDriving = j.isDriving,
                            exp = dex,
                            education = j.education,
                            ed = ded,
                            location = j.location,
                            loc = new DisplayableLocation { city = dl.city, state = s.state1, abbr = s.abbr },
                            JobCategories = j.JobCategories,
                            JobShifts = j.JobShifts,
                            contact = j.contact,
                            contact_info = (from c in db.Contacts
                                            join cs in db.States on c.state_id equals cs.id
                                            where c.id.Equals(j.contact)
                                            select new DisplayableContact {
                                                id = c.id,
                                                name = c.name,
                                                street = c.street,
                                                city = c.city,
                                                state_id = c.state_id,
                                                phone = c.phone,
                                                fax = c.fax,
                                                email = c.email,
                                                abbr = cs.abbr
                                            }).FirstOrDefault<DisplayableContact>()
                        }).FirstOrDefault<DisplayableJob>();
            } catch (Exception) {
                return new DisplayableJob();
            }
        }

        internal static Application SaveBasic(Guid app_id, string fname, string mname, string lname, string address, string city, Guid state_id, string zip, string phone1, string phone2, string referred_by, int previousEmployee, string previousExplanation, int isAdult, int isCitizen, int isEmployed, string date_available, decimal startSal, decimal endSal, string employment_type, string preferred_shift, int isFelon, List<string> conviction_dates, List<string> convictions, Guid id) {
            JobBoardDataContext db = new JobBoardDataContext();
            Application app = new Application();
            if (app_id != null && app_id != Guid.Empty) {
                app = db.Applications.Where(x => x.id.Equals(app_id)).FirstOrDefault<Application>();
            }
            //id = Guid.NewGuid();
            app.job_id = id;
            app.date_added = DateTime.Now;
            app.fname = fname;
            app.lname = lname;
            app.mname = mname;
            app.address = address;
            app.city = city;
            app.state_id = state_id;
            app.zip = zip;
            app.phone1 = phone1;
            app.phone2 = phone2;
            app.referred_by = referred_by;
            app.previousEmployee = previousEmployee;
            app.previousExplanation = previousExplanation;
            app.isAdult = isAdult;
            app.isCitizen = isCitizen;
            app.isEmployed = isEmployed;
            try {
                app.date_available = (date_available != "") ? Convert.ToDateTime(date_available) : (DateTime?)null;
            } catch (Exception) {
                throw new Exception("Invalid date available");
            }
            app.desired_start = startSal;
            app.desired_end = endSal;
            app.employment_type = employment_type;
            app.preferred_shift = preferred_shift;
            app.status = ApplicationStatus.ACTIVE.ToString();

            if (app_id == null || app_id == Guid.Empty) {
                app.job_id = id;
                db.Applications.InsertOnSubmit(app);
            }
            db.SubmitChanges();

            for (int i = 0; i <= conviction_dates.Count; i++) {
                try {
                    // First empty out any conviction records that are tied to this app
                    List<Conviction> cons = db.Convictions.Where(x => x.app_id.Equals(app.id)).ToList<Conviction>();
                    db.Convictions.DeleteAllOnSubmit<Conviction>(cons);
                    db.SubmitChanges();

                    Conviction con = new Conviction {
                        id = Guid.NewGuid(),
                        conviction1 = convictions[i],
                        date_convicted = Convert.ToDateTime(conviction_dates[i]),
                        app_id = app.id
                    };
                    db.Convictions.InsertOnSubmit(con);
                } catch (Exception) { }
            }
            db.SubmitChanges();
            return app;
        }

        internal static Application GetApplication(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                Application app = new Application();
                app = db.Applications.Where(x => x.id.Equals(id)).FirstOrDefault<Application>();
                return app;
            } catch (Exception) {
                throw new Exception();
            }
        }

        internal static List<DisplayableJob> Search(string exp, string edu, string loc, string shift) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();

                List<Job> jobs = db.Jobs.Where(x => x.jobState.Equals(JobState.PUBLISHED.ToString())).ToList<Job>();

                if (!String.IsNullOrEmpty(exp)) {
                    jobs = jobs.Where(x => x.experience.Equals(new Guid(exp))).ToList<Job>();
                }

                if (!String.IsNullOrEmpty(edu)) {
                    jobs = jobs.Where(x => x.education.Equals(new Guid(edu))).ToList<Job>();
                }

                if (!String.IsNullOrEmpty(loc)) {
                    jobs = jobs.Where(x => x.location.Equals(new Guid(loc))).ToList<Job>();
                }

                if (!String.IsNullOrEmpty(shift)) {
                    Guid shift_id = db.Shifts.Where(x => x.id.Equals(shift)).Select(x => x.id).FirstOrDefault<Guid>();
                    JobShift j = new JobShift {
                        shift = shift_id
                    };
                    jobs = jobs.Where(x => x.JobShifts.Contains(j, new JobShiftEqualityComparer())).ToList<Job>();
                }

                List<DisplayableJob> dJobs = new List<DisplayableJob>();
                foreach (Job j in jobs) {
                    DisplayableJob dj = Get(j.id);
                    dJobs.Add(dj);
                }
                return dJobs;
            } catch (Exception) {
                return new List<DisplayableJob>();
            }
        }

        internal static void AddEmployment(Guid emp_id, Guid id, string start_date, string end_date, string employer, string phone, string address, string city, Guid state_id, string starting_title, string ending_title, string supervisor, int contact, string reason_leaving, string summary, string starting_pay, string ending_pay) {
            EmploymentHistory emp = new EmploymentHistory();
            JobBoardDataContext db = new JobBoardDataContext();
            emp.id = Guid.NewGuid();
            emp.app_id = id;
            if (emp_id != null && emp_id != Guid.Empty) {
                emp = db.EmploymentHistories.Where(x => x.id.Equals(emp_id)).FirstOrDefault<EmploymentHistory>();
            }
            try{
                emp.start_date = Convert.ToDateTime(start_date);
            }catch(Exception){
                throw new Exception("Invalid start date.");
            }
            try {
                emp.end_date = (end_date != "") ? Convert.ToDateTime(end_date) : (DateTime?)null;
            } catch (Exception) {
                throw new Exception("Invalid end date.");
            }
            emp.employer = employer;
            emp.phone = phone;
            emp.address = address;
            emp.city = city;
            emp.state_id = state_id;
            emp.starting_title = starting_title;
            emp.end_title = ending_title;
            emp.supervisor = supervisor;
            emp.canContact = contact;
            emp.reason_leaving = reason_leaving;
            emp.summary = summary;
            emp.start_pay = starting_pay;
            emp.end_pay = ending_pay;

            if (emp_id == null || emp_id == Guid.Empty) {
                db.EmploymentHistories.InsertOnSubmit(emp);
            }
            db.SubmitChanges();

        }
    }

    public enum ApplicationStates {
        BASIC, 
        EMPLOYMENT,
        EDUCATION,
        DRIVING,
        RESUME,
        REVIEW
    }

    public enum ApplicationStatus {
        ACTIVE,
        ARCHIVED,
        INACTIVE
    }

    public enum JobState {
        CREATED,
        PUBLISHED,
        ARCHIVED
    }

    public class DisplayableJob : Job {
        public DisplayableLocation loc { get; set; }
        public Education ed { get; set; }
        public Experience exp { get; set; }
        public DisplayableContact contact_info { get; set; }
        


        internal List<Crumb> GenerateCrumbs(Guid id = new Guid(), ApplicationStates state = ApplicationStates.BASIC) {
            List<Crumb> crumbs = new List<Crumb>();
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                Application app = db.Applications.Where(x => x.id.Equals(id)).FirstOrDefault<Application>();
                if (app.dateSubmitted != null) {
                    state = ApplicationStates.REVIEW;
                }

                // Build out Basic crumb
                Crumb basic = new Crumb() {
                    title = "Basic Information",
                    path = "/Job/Apply/"+ this.id + "?app_id=" + id,
                    active = false,
                    order = 0,
                    disabled = false
                };
                if (app.dateSubmitted == null) 
                    crumbs.Add(basic);

                // Build out Employment crumb
                Crumb emp = new Crumb() {
                    title = "Employment History",
                    path = "/Job/Employment/" + this.id + "?app_id=" + id,
                    active = false,
                    order = 1,
                    disabled = true
                };
                if(app.job_id != null && app.job_id != Guid.Empty){
                    emp.disabled = false;
                }
                if (app.dateSubmitted == null)
                    crumbs.Add(emp);

                // Build out Education crumb
                Crumb edu = new Crumb() {
                    title = "Educational Background",
                    path = "/Job/Education/" + id,
                    active = false,
                    order = 2,
                    disabled = true
                };
                int emp_history = db.EmploymentHistories.Where(x => x.app_id.Equals(id)).Count();
                if (emp_history > 0) {
                    edu.disabled = false;
                }
                if (app.dateSubmitted == null)
                    crumbs.Add(edu);

                int order = 3;
                int edu_count = db.EducationalBackgrounds.Where(x => x.app_id.Equals(id)).Count();
                if (this.isDriving != 0) {
                    // Build out Driving crumb
                    Crumb driving = new Crumb() {
                        title = "Driving History",
                        path = "/Job/Driving/" + id,
                        active = false,
                        order = order,
                        disabled = true
                    };
                    
                    if (edu_count > 0) {
                        driving.disabled = false;
                    }
                    if (app.dateSubmitted == null)
                        crumbs.Add(driving);
                    order++;
                }

                // Build out Resume crumb
                Crumb resume = new Crumb() {
                    title = "Resumé",
                    path = "/Job/Resume/" + id,
                    active = false,
                    order = order,
                    disabled = true
                };
                if (this.isDriving == 0) {
                    if (emp_history > 0 && edu_count > 0) {
                        resume.disabled = false;
                    }
                } else {
                    if (emp_history > 0 && edu_count > 0 && app.dob != null) {
                        resume.disabled = false;
                    }
                }
                if (app.dateSubmitted == null)
                    crumbs.Add(resume);
                
                // Build out Review crumb
                Crumb review = new Crumb() {
                    title = "Review",
                    path = "/Job/Review/" + id,
                    active = false,
                    order = order,
                    disabled = true
                };
                if (this.isDriving == 0) {
                    if (emp_history > 0 && edu_count > 0) {
                        review.disabled = false;
                    }
                } else {
                    if (emp_history > 0 && edu_count > 0 && app.dob != null) {
                        review.disabled = false;
                    }
                }
                crumbs.Add(review);

                switch (state) {
                    case ApplicationStates.BASIC:
                        crumbs.Where(x => x.title.Equals("Basic Information")).FirstOrDefault<Crumb>().active = true;
                        break;
                    case ApplicationStates.EMPLOYMENT:
                        crumbs.Where(x => x.title.Equals("Employment History")).FirstOrDefault<Crumb>().active = true;
                        break;
                    case ApplicationStates.EDUCATION:
                        crumbs.Where(x => x.title.Equals("Educational Background")).FirstOrDefault<Crumb>().active = true;
                        break;
                    case ApplicationStates.DRIVING:
                        crumbs.Where(x => x.title.Equals("Driving History")).FirstOrDefault<Crumb>().active = true;
                        break;
                    case ApplicationStates.RESUME:
                        crumbs.Where(x => x.title.Equals("Resumé")).FirstOrDefault<Crumb>().active = true;
                        break;
                    case ApplicationStates.REVIEW:
                        crumbs.Where(x => x.title.Equals("Review")).FirstOrDefault<Crumb>().active = true;
                        break;
                    default:
                        crumbs.Where(x => x.title.Equals("Basic Information")).FirstOrDefault<Crumb>().active = true;
                        break;
                }
            } catch (Exception) {
                crumbs = new List<Crumb>();
            }
            return crumbs;
        }
    }

    public class DisplayableContact : Contact {
        public string abbr { get; set; }
    }

    public class Crumb {
        public string title { get; set; }
        public string path { get; set; }
        public bool active { get; set; }
        public bool disabled { get; set; }
        public int order { get; set; }
    }

    class JobShiftEqualityComparer : IEqualityComparer<JobShift> {

        public bool Equals(JobShift j1, JobShift j2) {
            if (j1.shift == j2.shift) {
                return true;
            } else {
                return false;
            }
        }


        public int GetHashCode(JobShift js) {
            return js.GetHashCode();
        }

    }
}