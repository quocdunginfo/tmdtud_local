using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class CategoryController : Controller
    {
        private BlogDBContext db = new BlogDBContext();
        //
        // GET: /Category/

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        public ActionResult Add()
        {
            CategoryModel cat = new CategoryModel();
            cat.name = "Tên cat";
            db.Categories.Add(cat);
            db.SaveChanges();
            return View();
        }

    }
}
