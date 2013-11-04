using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest.Controllers.ModelController;
using qdtest.Models;

namespace CuaHangBanGiay.Controllers.View_Controller
{
    public class FrontController : Controller
    {
        //
        // GET: /Layout/

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NhomSanPhamController ctr = new NhomSanPhamController();
            ViewBag.NhomSanPham2_List_All = ctr.timkiem("", "", "", "");
           
        }

    }
}
