using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest.Controllers.ModelController;
using qdtest.Models;

namespace qdtest.Controllers
{
    public class FrontController : Controller
    {
        //
        // GET: /Layout/

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NhomSanPhamController ctr = new NhomSanPhamController();
            List<NhomSanPham2> list1 = ctr.timkiem("", "", "", "");
            SanPhamController ctr2 = new SanPhamController(ctr._db);
            List<SanPham>list2=ctr2.get_bestseller(4);
            if (list1 != null && list2 != null)
            {
                ViewBag.NhomSanPham2_List_All = list1;
                ViewBag.SanPham_BestSeller = list2;
            }
            else
            {
                ViewBag.NhomSanPham2_List_All = new List<NhomSanPham2>();
                ViewBag.SanPham_BestSeller = new List<SanPham>();
            }
        }

    }
}
