using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models {
    public class ContactModel {

        /// <summary>
        /// Retrieve all Contact records from the database
        /// </summary>
        /// <returns>List of DisplayableContact</returns>
        public static List<DisplayableContact> GetAll() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();

                List<DisplayableContact> contacts = (from c in db.Contacts
                                                     join s in db.States on c.state_id equals s.id into DefaultState
                                                     from ds in DefaultState.DefaultIfEmpty()
                                                     where !c.level.Equals("DISABLED")
                                                     select new DisplayableContact {
                                                         id = c.id,
                                                         name = c.name,
                                                         street = c.street,
                                                         city = c.city,
                                                         state_id = c.state_id,
                                                         state = ds.state1,
                                                         abbr = ds.abbr,
                                                         phone = c.phone,
                                                         fax = c.fax,
                                                         email = c.email,
                                                         username = c.username,
                                                         password = c.password,
                                                         listing_count = db.Jobs.Where(x => x.contact == c.id).Count()
                                                     }).ToList<DisplayableContact>();
                return contacts;
            } catch (Exception) {
                return new List<DisplayableContact>();
            }
        }

        /// <summary>
        /// Retrieve a specific Contact record from the database
        /// </summary>
        /// <param name="id">Identification of the contact record</param>
        /// <returns>DisposableContact</returns>
        public static DisplayableContact Get(Guid id) {
            try {
                JobBoardDataContext db = new JobBoardDataContext();
                DisplayableContact contact = (from c in db.Contacts
                                              join s in db.States on c.state_id equals s.id into DefaultStates
                                              from ds in DefaultStates.DefaultIfEmpty()
                                              where c.id.Equals(id)
                                              select new DisplayableContact {
                                                  id = c.id,
                                                  name = c.name,
                                                  street = c.street,
                                                  city = c.city,
                                                  state_id = c.state_id,
                                                  state = ds.state1,
                                                  abbr = ds.abbr,
                                                  phone = c.phone,
                                                  fax = c.fax,
                                                  email = c.email,
                                                  username = c.username,
                                                  password = c.password,
                                                  listing_count = db.Jobs.Where(x => x.contact == c.id).Count(),
                                                  level = c.level
                                              }).FirstOrDefault<DisplayableContact>();
                return contact;
            } catch (Exception) {
                return new DisplayableContact();
            }
        }


        /// <summary>
        /// Retrieve a list of all States in the database
        /// </summary>
        /// <returns>List of State</returns>
        public static List<State> GetStates() {
            try {
                JobBoardDataContext db = new JobBoardDataContext();

                return db.States.OrderBy(x => x.abbr).ToList<State>();
            } catch (Exception) {
                return new List<State>();
            }
        }

        public static List<Job> GetListings(Guid id) {
            try {
                if (id == null) { throw new Exception(); }

                JobBoardDataContext db = new JobBoardDataContext();
                return db.Jobs.Where(x => x.contact == id).ToList<Job>();
            } catch (Exception) {
                return new List<Job>();
            }
        }

        /// <summary>
        /// Creates a new Contact record
        /// </summary>
        /// <param name="name"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="state_id"></param>
        /// <param name="phone"></param>
        /// <param name="fax"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Create(string name, string street, string city, Guid state_id, string phone, string fax, string email, string username, string password, string level) {
            JobBoardDataContext db = new JobBoardDataContext();

            if (password.Length == 0) { username = ""; }
            Contact con = new Contact {
                id = Guid.NewGuid(),
                name = name,
                street = street,
                city = city,
                state_id = state_id,
                phone = phone,
                fax = fax,
                email = email,
                username = username,
                password = password,
                level = level.Trim().ToUpper()
            };
            db.Contacts.InsertOnSubmit(con);
            db.SubmitChanges();
            return "Contact saved.";
        }

        /// <summary>
        /// Updates and existing contact record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="street"></param>
        /// <param name="city"></param>
        /// <param name="state_id"></param>
        /// <param name="phone"></param>
        /// <param name="fax"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Update(Guid id, string name, string street, string city, Guid state_id, string phone, string fax, string email, string username, string password, string level) {
            JobBoardDataContext db = new JobBoardDataContext();
            Contact con = db.Contacts.Where(x => x.id == id).FirstOrDefault<Contact>();

            con.name = name;
            con.street = street;
            con.city = city;
            con.state_id = state_id;
            con.phone = phone;
            con.fax = fax;
            con.email = email;
            con.username = username;
            con.level = level.Trim().ToUpper();
            if (password.Length > 0) {
                con.password = password;
            }

            db.SubmitChanges();
            return "Contact saved.";
        }

        public static string Delete(Guid id, string contact_id) {
            if (id == null) { throw new Exception(); }

            JobBoardDataContext db = new JobBoardDataContext();
            Guid con_id = new Guid(contact_id);

            // Get the job listings that are tied to the contact we're removing
            List<Job> jobs = db.Jobs.Where(x => x.contact == id).ToList<Job>();
            List<JobContact> jobcontacts = db.JobContacts.Where(x => x.contact.Equals(id)).ToList<JobContact>();
            db.JobContacts.DeleteAllOnSubmit(jobcontacts);

            // Loop through the jobs and reassign them to the logged in users ID
            foreach (Job j in jobs) {
                j.contact = con_id;
            }

            Contact con = db.Contacts.Where(x => x.id == id).FirstOrDefault<Contact>();
            db.Contacts.DeleteOnSubmit(con);

            db.SubmitChanges();
            return "";
        }
    }

    public class DisplayableContact : Contact {
        public string state { get; set; }
        public string abbr { get; set; }
        public int listing_count { get; set; }
    }
}