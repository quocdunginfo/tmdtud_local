﻿using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;
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
            this.timkiem_sanpham["active"] = "";
            this.timkiem_sanpham["gia_from"] = "-1";
            this.timkiem_sanpham["gia_to"] = "-1";
            this.timkiem_sanpham["hangsx_id"] = "";
            this.timkiem_sanpham["nhomsanpham_id"] = "";
            this.timkiem_sanpham["order_by"] = "id";
            this.timkiem_sanpham["order_desc"] = "1";
            this.timkiem_sanpham["nhomsanpham_id"] = "";
            this.timkiem_sanpham["max_item_per_page"] = "5";
        }
        [HttpGet]
        public ActionResult Index(int page=1)
        {
            //check
            if (!this._nhanvien_permission.Contains("sanpham_view"))
            {
                return this._fail_permission("sanpham_view");
            }
            SanPhamController ctr = new SanPhamController();
            NhomSanPhamController ctr_nhom = new NhomSanPhamController();
            HangSXController ctr_hangsx = new HangSXController();
            List<HangSX> hangsx_list = ctr_hangsx.timkiem(timkiem_sanpham["hangsx_id"]);
            NhomSanPham nhom_obj = ctr_nhom.get_by_id(TextLibrary.ToInt(timkiem_sanpham["nhomsanpham_id"]));
            List<NhomSanPham> nhomsanpham_list = ctr_nhom.get_tree2(nhom_obj);
            //pagination
                int max_item_per_page = TextLibrary.ToInt(this.timkiem_sanpham["max_item_per_page"]);
                Pagination pg = new Pagination();
                pg.set_current_page(page);
                pg.set_max_item_per_page(max_item_per_page);
                pg.set_total_item(
                    ctr.timkiem_count(
                        timkiem_sanpham["id"],
                        timkiem_sanpham["masp"],
                        timkiem_sanpham["ten"],
                        "",
                        TextLibrary.ToInt(timkiem_sanpham["gia_from"]),
                        TextLibrary.ToInt(timkiem_sanpham["gia_to"]),
                        hangsx_list, nhomsanpham_list, timkiem_sanpham["active"]
                        )
                    );
                pg.update();
            //Chọn danh sách nhân viên để hiển thị theo cookies tìm kiếm
            
            ViewBag.SanPham_List = ctr.timkiem(
                timkiem_sanpham["id"],
                timkiem_sanpham["masp"],
                timkiem_sanpham["ten"],
                "",
                TextLibrary.ToInt( timkiem_sanpham["gia_from"]),
                TextLibrary.ToInt(timkiem_sanpham["gia_to"]),
                hangsx_list, nhomsanpham_list, timkiem_sanpham["active"], timkiem_sanpham["order_by"], TextLibrary.ToBoolean(timkiem_sanpham["order_desc"]), pg.start_point, max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_sanpham = this.timkiem_sanpham;
            ViewBag.Title += " - Quản lý";
            ViewBag.pagination = pg;
            ViewBag.NhomSanPham2_List = ctr_nhom.timkiem();
            ViewBag.HangSX_List = ctr_hangsx.timkiem();
            return View();
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
                this.timkiem_sanpham["gia_from"] = TextLibrary.ToString(Request["sanpham_gia_from"]);
                this.timkiem_sanpham["gia_to"] = TextLibrary.ToString(Request["sanpham_gia_to"]);
                this.timkiem_sanpham["active"] = TextLibrary.ToString(Request["sanpham_active"]);
                this.timkiem_sanpham["hangsx_id"] = TextLibrary.ToString(Request["sanpham_hangsx_id"]);
                this.timkiem_sanpham["nhomsanpham_id"] = TextLibrary.ToString(Request["sanpham_nhomsanpham_id"]);
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
                try
                {
                    this.timkiem_sanpham = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_sanpham"));
                }
                catch (Exception)
                {
                    this.khoitao_cookie();
                    Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_sanpham));
                }
            }
            
            //set active tab
            this._set_activetab(new String[] { "Catalog","SanPham" });
        }
        public ActionResult Add()
        {
            //check
            if (!this._nhanvien_permission.Contains("sanpham_add"))
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
            if (!this._nhanvien_permission.Contains("sanpham_edit"))
            {
                return this._fail_permission("sanpham_edit");
            }
            return RedirectToAction("Index", "AdminSanPham", new { id=id});
        }
        public ActionResult Delete(int id = 0)
        {
            //check
            if (!this._nhanvien_permission.Contains("sanpham_delete"))
            {
                return this._fail_permission("sanpham_delete");
            }

            SanPhamController controller = new SanPhamController();
            if (!controller.is_exist(id))
            {
                return RedirectToAction("Index", "AdminSanPhams");
            }
            try
            {
                controller.delete(id);
            }
            catch (Exception)
            {
                return _show_notification("Sản phẩm này có dính khóa ngoại với chi tiết sản phẩm, hình ảnh hoặc đơn hàng hiện có nên không xóa được");
            }
            return RedirectToAction("Index", "AdminSanPhams");
        }
        public ActionResult Active(int id=0)
        {
            //controller
            SanPhamController ctr= new SanPhamController();
            //get obj
            SanPham obj = ctr.get_by_id(id);
            //check
            if (obj == null)
            {
                return RedirectToAction("Index", "AdminSanPhams");
            }
            //đảo ngược active
            obj.active = !obj.active;
            //lưu
            ctr._db.SaveChanges();
            //redirect
            return RedirectToAction("Index","AdminSanPhams");
        }
        [HttpGet]
        public ActionResult OrderBy(string order_by = "id", string order_desc = "1")
        {
            this.timkiem_sanpham["order_by"] = order_by;
            this.timkiem_sanpham["order_desc"] = order_desc;
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_sanpham));

            return RedirectToAction("Index", "AdminSanPhams");
        }
    }
}
