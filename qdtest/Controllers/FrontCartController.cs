using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class FrontCartController : FrontController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Remove(int chitietsp_id)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Update(int chitietsp_id)
        {
            return View();
        }

    }
}
