using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class FrontAboutController : FrontController
    {
        //
        // GET: /FrontAbout/

        public ActionResult Index()
        {
            return View();
        }

    }
}
