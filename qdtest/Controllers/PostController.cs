using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class PostController : Controller
    {
        private BlogDBContext db = new BlogDBContext();
        //
        // GET: /Post/

        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }
        public ActionResult Add()
        {
            UserModel m = db.Users.First();
            CategoryModel c = db.Categories.Find(1);
            CategoryModel c2 = db.Categories.Find(2);
            PostModel k = new PostModel();
            k.title = "what the fuck";
            k.content = "what the hell";
            k.user = m;
            k.categories = new List<CategoryModel>();
            k.categories.Add(c);
            k.categories.Add(c2);
            db.Posts.Add(k);
            db.SaveChanges();
            return View();
        }

    }
}
