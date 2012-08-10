using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Admin {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{file}.txt");
            routes.IgnoreRoute("{file}.htm");
            routes.IgnoreRoute("{file}.html");
            routes.IgnoreRoute("{file}.xml");

            routes.MapRoute(
                "Application View",
                "Application/{id}",
                new { controller = "Applications", action = "ViewApplication", id = new Guid() }
            );

            routes.MapRoute(
                "Duplicate Job",
                "Jobs/Duplicate/{id}",
                new { controller = "Jobs", action = "Duplicate", id = new Guid() }
            );

            routes.MapRoute(
                "Job Categories",
                "Jobs/Categories",
                new { controller = "Jobs", action = "Categories" }
            );

            routes.MapRoute(
                "Jobs by Category",
                "Jobs/Category/{id}",
                new { controller = "Jobs", action = "Category", id = new Guid() }
            );

            routes.MapRoute(
                "Job Locations",
                "Jobs/Locations",
                new { controller = "Jobs", action = "Locations" }
            );

            routes.MapRoute(
                "Jobs by Location",
                "Jobs/Location/{id}",
                new { controller = "Jobs", action = "Location", id = new Guid() }
            );

            routes.MapRoute(
                "Job Archive",
                "Jobs/Archive",
                new { controller = "Jobs", action = "Archive" }
            );

            routes.MapRoute(
                "Job Add Category",
                "Jobs/AddCategory",
                new { controller = "Jobs", action = "AddCategory" }
            );

            routes.MapRoute(
                "Job Delete Category",
                "Jobs/DeleteCategory",
                new { controller = "Jobs", action = "DeleteCategory" }
            );

            routes.MapRoute(
                "Job Add Shift",
                "Jobs/AddShift",
                new { controller = "Jobs", action = "AddShift" }
            );

            routes.MapRoute(
                "Job Delete Shift",
                "Jobs/DeleteShift",
                new { controller = "Jobs", action = "DeleteShift" }
            );

            routes.MapRoute(
                "Job Add Contact",
                "Jobs/AddContact",
                new { controller = "Jobs", action = "AddContact" }
            );

            routes.MapRoute(
                "Job Delete Contact",
                "Jobs/DeleteContact",
                new { controller = "Jobs", action = "DeleteContact" }
            );

            routes.MapRoute(
                "Job Add",
                "Jobs/Add",
                new { controller = "Jobs", action = "Add" }
            );

            routes.MapRoute(
                "Job Save New",
                "Jobs/Save/{id}",
                new { controller = "Jobs", action = "Save", id = new Guid() }
            );

            routes.MapRoute(
                "Jobs Edit",
                "Jobs/{id}",
                new { controller = "Jobs", action = "Edit" }
            );

            routes.MapRoute(
                "Education Add",
                "Education/Add",
                new { controller = "Education", action = "Add" }
            );

            routes.MapRoute(
                "Education Save New",
                "Education/Save/{id}",
                new { controller = "Education", action = "Save", id = new Guid() }
            );

            routes.MapRoute(
                "Education Edit",
                "Education/{id}",
                new { controller = "Education", action = "Edit" }
            );

            routes.MapRoute(
                "Shift Add",
                "Shifts/Add",
                new { controller = "Shifts", action = "Add" }
            );

            routes.MapRoute(
                "Shift Save New",
                "Shifts/Save/{id}",
                new { controller = "Shifts", action = "Save", id = new Guid() }
            );

            routes.MapRoute(
                "Shift Edit",
                "Shifts/{id}",
                new { controller = "Shifts", action = "Edit" }
            );
            routes.MapRoute(
                "Experience Add",
                "Experience/Add",
                new { controller = "Experience", action = "Add" }
            );

            routes.MapRoute(
                "Experience Save New",
                "Experience/Save/{id}",
                new { controller = "Experience", action = "Save", id = new Guid() }
            );

            routes.MapRoute(
                "Experience Edit",
                "Experience/{id}",
                new { controller = "Experience", action = "Edit" }
            );

            routes.MapRoute(
                "Locations Add",
                "Locations/Add",
                new { controller = "Locations", action = "Add" }
            );

            routes.MapRoute(
                "Locations Save New",
                "Locations/Save/{id}",
                new { controller = "Locations", action = "Save", id = new Guid() }
            );

            routes.MapRoute(
                "Locations Edit",
                "Locations/{id}",
                new { controller = "Locations", action = "Edit" }
            );

            routes.MapRoute(
                "Categories Add",
                "Categories/Add",
                new { controller = "Categories", action = "Add" }
            );

            routes.MapRoute(
                "Categories Save New",
                "Categories/Save/{id}",
                new { controller = "Categories", action = "Save", id = new Guid() }
            );

            routes.MapRoute(
                "Categories Edit",
                "Categories/{id}",
                new { controller = "Categories", action = "Edit" }
            );

            routes.MapRoute(
                "Contacts Add",
                "Contacts/Add",
                new { controller = "Contacts", action = "Add" }
            );

            routes.MapRoute(
                "Contacts Save New",
                "Contacts/Save/{id}",
                new { controller = "Contacts", action = "Save", id = new Guid() }
            );

            routes.MapRoute(
                "Contacts Edit",
                "Contacts/{id}",
                new { controller = "Contacts", action = "Edit" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Index", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}