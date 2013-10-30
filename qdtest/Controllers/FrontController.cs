using System;
using System.Collections.Generic;
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
            //ViewBag.NhomSanPham2_List = ctr.timkiem(
            //    ctr.timkiem_nhomsanpham["id"],
            //    this.timkiem_nhomsanpham["ten"],
            //    this.timkiem_nhomsanpham["mota"],
            //    this.timkiem_nhomsanpham["active"]);
            ViewBag.NhomSanPham2_List_All = ctr.timkiem("", "", "", "");
            //ViewBag.Title += " - View";
            //ViewBag.timkiem_nhomsanpham = this.timkiem_nhomsanpham;
        }

    }
}
