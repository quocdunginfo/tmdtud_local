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
        protected HttpCookie _timkiem_sanpham;
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
            //build timkiem_nhanvien
            if (Request.Cookies.Get("front_timkiem_sanpham") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this._khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this._timkiem_sanpham));
            }
            else
            {
                try
                {
                    this._timkiem_sanpham = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_sanpham"));
                }
                catch (Exception ex)
                {
                    this._khoitao_cookie();
                    Response.Cookies.Add(CookieLibrary.Base64Encode(this._timkiem_sanpham));
                }
            }
            ViewBag.timkiem_sanpham = this._timkiem_sanpham;
        }
        [NonAction]
        protected void _khoitao_cookie()
        {
            _timkiem_sanpham = new HttpCookie("front_timkiem_sanpham");
            _timkiem_sanpham.Expires = DateTime.Now.AddDays(1);
            this._timkiem_sanpham["ten"] = "";
            this._timkiem_sanpham["masp"] = "";
            this._timkiem_sanpham["mota"] = "";
            this._timkiem_sanpham["gia_from"] = "-1";
            this._timkiem_sanpham["gia_to"] = "-1";
            this._timkiem_sanpham["hangsx_ten"] = "";
            this._timkiem_sanpham["nhomsanpham_ten"] = "";
            this._timkiem_sanpham["max_item_per_page"] = "9";
        }

    }
}
