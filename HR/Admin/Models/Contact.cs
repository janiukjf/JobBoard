using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Net.Mail;
using Admin.Models;

namespace Admin {
    partial class Contact {

        public bool isAdmin() {
            if (this.level == AuthLevel.ADMIN.ToString()) {
                return true;
            }
            return false;
        }
    }

}