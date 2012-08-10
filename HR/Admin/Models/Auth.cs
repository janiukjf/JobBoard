using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models {
    public class Auth {

        public static Guid Login(string username, string password) {
            if (username.Length == 0 || password.Length == 0) { throw new Exception(); }

            JobBoardDataContext db = new JobBoardDataContext();

            Guid contact = db.Contacts.Where(x => x.username == username && x.password == password && x.level != "DISABLED").Select(x => x.id).FirstOrDefault<Guid>();
            return contact;
        }

    }

    public enum AuthLevel {
        USER,
        ADMIN,
        DISABLED
    }
}