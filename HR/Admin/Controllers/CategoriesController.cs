using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;

namespace Admin.Controllers {
    public class CategoriesController : AuthController {

        public ActionResult Index() {

            // Get all the categories
            List<CategoryWithParent> cats = CategoryModel.GetAll();
            ViewBag.cats = cats;

            ViewBag.msg = TempData["msg"];
            TempData.Clear();

            return View();
        }

        public ActionResult Add() {

            List<Category> cats = CategoryModel.GetParentCategories();
            ViewBag.cats = cats;

            ViewBag.name = TempData["name"];
            ViewBag.short_desc = TempData["short_desc"];
            ViewBag.long_desc = TempData["long_desc"];
            ViewBag.parent = TempData["parent"];
            ViewBag.msg = TempData["msg"];
            TempData.Clear();

            return View();
        }

        public ActionResult Edit(Guid id) {

            if (id == null || id == Guid.Empty) { return RedirectToAction("Index", "Categories"); }

            // Retrieve the category we're editing
            Category cat = CategoryModel.Get(id);
            ViewBag.cat = cat;

            // Get the listings for this category
            List<Job> listings = CategoryModel.GetListings(id);
            ViewBag.listings = listings;

            // Get all the categories
            List<Category> cats = CategoryModel.GetParentCategories();
            ViewBag.cats = cats;

            // Assign TempData into the ViewBag
            ViewBag.name = TempData["name"];
            ViewBag.short_desc = TempData["short_desc"];
            ViewBag.long_desc = TempData["long_desc"];
            ViewBag.parent = TempData["parent"];
            ViewBag.msg = TempData["msg"];
            TempData.Clear();
            return View();
        }

        public dynamic Save(Guid id = new Guid(), string name = "", string short_desc = "", string long_desc = "", Guid parent = new Guid()) {
            try {
                string msg = "";
                TempData["name"] = name;
                TempData["short_desc"] = short_desc;
                TempData["long_desc"] = long_desc;
                TempData["parent"] = parent;

                if (id == null || id == Guid.Empty) {
                    msg = CategoryModel.Create(name, short_desc, long_desc, parent);
                    if (msg.Length == 0) {
                        return RedirectToAction("Index", "Categories");
                    } else {
                        TempData["msg"] = msg;
                        return RedirectToAction("Add", "Categories");
                    }
                } else {
                    msg = CategoryModel.Update(id, name, short_desc, long_desc, parent);
                    if (msg.Length == 0) {
                        return RedirectToAction("Index", "Categories");
                    } else {
                        TempData["msg"] = msg;
                        return Redirect("/Admin/Categories/"+id);
                    }
                }
            } catch (Exception) {
                TempData["msg"] = "Failed to save category.";
                return RedirectToAction("Index","Categories");
            }
        }

        public string Delete(Guid id) {
            return CategoryModel.Delete(id);
        }

    }
}
