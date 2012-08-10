using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Models;

namespace HR.Models {

    public class TieredCategories : Category {
        public List<Category> subs { get; set; }
        public int jobCount { get; set; }
    }
}