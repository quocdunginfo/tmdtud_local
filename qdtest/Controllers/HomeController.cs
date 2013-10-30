using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest.Controllers.ModelController;

namespace CuaHangBanGiay.Controllers.View_Controller
{
    public class FrontHomeController : FrontController
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {   
            return View();
        }

    }
}
