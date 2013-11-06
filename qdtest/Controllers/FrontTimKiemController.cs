using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;

namespace qdtest.Controllers
{
    public class FrontTimKiemController : FrontController
    {
        public FrontTimKiemController()
        {
            
        }
        
        public ActionResult Index(int page=1)
        {
            SanPhamController ctr = new SanPhamController();
            //calculate offset
                int max_item_per_page = 3;//TextLibrary.ToInt(this._timkiem_sanpham["max_item_per_page"]);
                int start_point = (page - 1) * max_item_per_page;
                if (start_point <= 0) start_point = 0;
            //get list
                List<SanPham> listnew = ctr.timkiem("", "", this._timkiem_sanpham["ten"], "", -1, -1, null, null, "1", "id", true, start_point, max_item_per_page);
                ViewBag.SanPham_List = listnew;
                ViewBag._timkiem_sanpham = this._timkiem_sanpham;
            //pagination
                int Current_Page = page;
                int Total_Page = ctr.timkiem_count(
                    "",
                    _timkiem_sanpham["masp"],
                    _timkiem_sanpham["ten"],
                    _timkiem_sanpham["mota"],
                    TextLibrary.ToInt(_timkiem_sanpham["gia_from"]),
                    TextLibrary.ToInt(_timkiem_sanpham["gia_to"]),
                    null, null, "1"
                    ) / max_item_per_page + 1;
                Boolean CanNextPage = Current_Page >= Total_Page ? false : true;
                Boolean CanPrevPage = Current_Page == 1 ? false : true;
                ViewBag.CanNextPage = CanNextPage;
                ViewBag.CanPrevPage = CanPrevPage;
                ViewBag.Current_Page = Current_Page;
                ViewBag.Total_page = Total_Page;
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get search value
            if (!TextLibrary.ToString(Request["submit_reset"]).Equals(""))
            {
                //reset button click
                this._khoitao_cookie();
            }
            else
            {
                //search button click
                this._timkiem_sanpham["masp"] = TextLibrary.ToString(Request["sanpham_masp"]);
                this._timkiem_sanpham["ten"] = TextLibrary.ToString(Request["sanpham_ten"]);
                this._timkiem_sanpham["mota"] = TextLibrary.ToString(Request["sanpham_mota"]);
                this._timkiem_sanpham["gia_from"] = TextLibrary.ToString(Request["sanpham_gia_from"]);
                this._timkiem_sanpham["gia_to"] = TextLibrary.ToString(Request["sanpham_gia_to"]);
                this._timkiem_sanpham["hangsx_ten"] = TextLibrary.ToString(Request["sanpham_hangsx_ten"]);
                this._timkiem_sanpham["nhomsanpham_ten"] = TextLibrary.ToString(Request["sanpham_nhomsanpham_ten"]);
                this._timkiem_sanpham["max_item_per_page"] = TextLibrary.ToString(Request["sanpham_max_item_per_page"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this._timkiem_sanpham));
            //redirect
            return RedirectToAction("Index", "FrontTimKiem");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Tìm kiếm sản phẩm";
        }

    }
}
