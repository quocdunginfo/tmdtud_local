using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;

namespace qdtest.Controllers
{
    public class FrontController : Controller
    {
        public FrontController()
        {
            this._khoitao_cookie();
        }
        //
        // GET: /Layout/
        protected HttpCookie front_timkiem_sanpham=null;
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
            //tim kiem
            //build timkiem_sanpham
            if (Request.Cookies.Get("front_timkiem_sanpham") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this._khoitao_cookie();
                this._luu_cookie();
            }
            else
            {
                try
                {
                    this.front_timkiem_sanpham = CookieLibrary.Base64Decode(Request.Cookies.Get("front_timkiem_sanpham"));
                }
                catch (Exception ex)
                {
                    this._khoitao_cookie();
                    this._luu_cookie();
                }
            }
            ViewBag.front_timkiem_sanpham = this.front_timkiem_sanpham;
        }
        [NonAction]
        protected void _luu_cookie()
        {
            if (this.front_timkiem_sanpham != null)
            {
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.front_timkiem_sanpham));
            }
        }
        [NonAction]
        protected void _khoitao_cookie()
        {
            front_timkiem_sanpham = new HttpCookie("front_timkiem_sanpham");
            this.front_timkiem_sanpham["front_ten"] = "";
            this.front_timkiem_sanpham["front_masp"] = "";
            this.front_timkiem_sanpham["front_mota"] = "";
            this.front_timkiem_sanpham["front_gia_from"] = "-1";
            this.front_timkiem_sanpham["front_gia_to"] = "-1";
            this.front_timkiem_sanpham["front_hangsx_ten"] = "";
            this.front_timkiem_sanpham["front_nhomsanpham_ten"] = "";
            this.front_timkiem_sanpham["front_max_item_per_page"] = "1";
        }

    }
}
