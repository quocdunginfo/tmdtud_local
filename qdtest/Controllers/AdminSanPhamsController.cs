using qdtest._Library;
using qdtest.Controllers.ModelController;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminSanPhamsController : AdminController
    {
        private HttpCookie timkiem_sanpham;
        public AdminSanPhamsController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_sanpham = new HttpCookie("timkiem_sanpham");
            timkiem_sanpham.Expires = DateTime.Now.AddDays(1);
            this.timkiem_sanpham["id"] = "";
            this.timkiem_sanpham["ten"] = "";
            this.timkiem_sanpham["masp"] = "";
            this.timkiem_sanpham["mota"] = "";
            this.timkiem_sanpham["active"] = "";
            this.timkiem_sanpham["gia_from"] = "-1";
            this.timkiem_sanpham["gia_to"] = "-1";
            this.timkiem_sanpham["hangsx_ten"] = "";
            this.timkiem_sanpham["nhomsanpham_ten"] = "";
            this.timkiem_sanpham["max_item_per_page"] = "5";
        }
        [HttpGet]
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._permission.Contains("sanpham_view"))
            {
                return this._fail_permission("sanpham_view");
            }
            //calculate offset limit
                int max_item_per_page = TextLibrary.ToInt(this.timkiem_sanpham["max_item_per_page"]);
                Debug.WriteLine("max_item_per_page"+page);
                int start_point = (page - 1) * max_item_per_page;
                if (start_point <= 0) start_point = 0;
            //Chọn danh sách nhân viên để hiển thị theo cookies tìm kiếm
            SanPhamController ctr = new SanPhamController();
            ViewBag.SanPham_List = ctr.timkiem(
                timkiem_sanpham["id"],
                timkiem_sanpham["masp"],
                timkiem_sanpham["ten"],
                timkiem_sanpham["mota"],
                TextLibrary.ToInt( timkiem_sanpham["gia_from"]),
                TextLibrary.ToInt(timkiem_sanpham["gia_to"]),
                null, null, timkiem_sanpham["active"],"id",true, start_point, max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_sanpham = this.timkiem_sanpham;
            ViewBag.Title += " - Quản lý";
            //pagination
                int Current_Page = page;
                int Total_Page = ctr.timkiem_count(
                    timkiem_sanpham["id"],
                    timkiem_sanpham["masp"],
                    timkiem_sanpham["ten"],
                    timkiem_sanpham["mota"],
                    TextLibrary.ToInt(timkiem_sanpham["gia_from"]),
                    TextLibrary.ToInt(timkiem_sanpham["gia_to"]),
                    null, null, timkiem_sanpham["active"]
                    ) / max_item_per_page + 1;
                Boolean CanNextPage = Current_Page >= Total_Page ? false : true;
                Boolean CanPrevPage = Current_Page == 1 ? false : true;
                ViewBag.CanNextPage = CanNextPage;
                ViewBag.CanPrevPage = CanPrevPage;
                ViewBag.Current_Page = Current_Page;
                ViewBag.Total_page = Total_Page;


            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!this._permission.Contains("sanpham_delete"))
            {
                return this._fail_permission("sanpham_delete");
            }
            SanPhamController ctr = new SanPhamController();
            try
            {
                if (ctr.delete(id))
                {
                    //notification
                }
                return RedirectToAction("Index", "AdminSanPhams");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "AdminSanPhams");
            }
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get search value
            if (!TextLibrary.ToString(Request["submit_reset"]).Equals(""))
            {
                //reset button click
                this.khoitao_cookie();
            }
            else
            {
                //search button click
                this.timkiem_sanpham["id"] = TextLibrary.ToString(Request["sanpham_id"]);
                this.timkiem_sanpham["masp"] = TextLibrary.ToString(Request["sanpham_masp"]);
                this.timkiem_sanpham["ten"] = TextLibrary.ToString(Request["sanpham_ten"]);
                this.timkiem_sanpham["mota"] = TextLibrary.ToString(Request["sanpham_mota"]);
                this.timkiem_sanpham["gia_from"] = TextLibrary.ToString(Request["sanpham_gia_from"]);
                this.timkiem_sanpham["gia_to"] = TextLibrary.ToString(Request["sanpham_gia_to"]);
                this.timkiem_sanpham["active"] = TextLibrary.ToString(Request["sanpham_active"]);
                this.timkiem_sanpham["hangsx_ten"] = TextLibrary.ToString(Request["sanpham_hangsx_ten"]);
                this.timkiem_sanpham["nhomsanpham_ten"] = TextLibrary.ToString(Request["sanpham_nhomsanpham_ten"]);
                this.timkiem_sanpham["max_item_per_page"] = TextLibrary.ToString(Request["sanpham_max_item_per_page"]);
                //this.timkiem_sanpham["diachi"] = TextLibrary.ToString(Request["sanpham_diachi"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_sanpham));
            //redirect
            return RedirectToAction("Index", "AdminSanPhams");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Sản phẩm";

            //build timkiem_nhanvien
            if (Request.Cookies.Get("timkiem_sanpham") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_sanpham));
            }
            else
            {
                this.timkiem_sanpham = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_sanpham"));
            }
            
            //set active tab
            this._set_activetab(new String[] { "SanPham" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._permission.Contains("sanpham_add"))
            {
                return this._fail_permission("sanpham_add");
            }
            //check nhom sanpham co it nhat 1
                NhomSanPhamController ctr_nhomsanpham = new NhomSanPhamController();
                if (ctr_nhomsanpham.timkiem_count("", "", "", "1") <= 0)
                {
                    return this._show_notification("Yêu cầu phải có ít nhất 1 nhóm sản phẩm active mới được thêm sản phẩm mới!");
                }
            //check hangsx co it nhat 1
                HangSXController ctr_hangsx = new HangSXController();
                if (ctr_hangsx.timkiem_count("", "", "1") <= 0)
                {
                    return this._show_notification("Yêu cầu phải có ít nhất 1 hãng sản xuất active mới được thêm sản phẩm mới!");
                }
            

            return RedirectToAction("Add", "AdminSanPham");
        }
        public ActionResult Edit(int id=0)
        {
            //check
            if (!this._permission.Contains("sanpham_edit"))
            {
                return this._fail_permission("sanpham_edit");
            }
            return RedirectToAction("Index", "AdminSanPham", new { id=id});
        }
    }
}
