using qdtest._Library;
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
    public class AdminUsersController : AdminController
    {
        private HttpCookie timkiem_nhanvien;
        public AdminUsersController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_nhanvien = new HttpCookie("timkiem_nhanvien");
            timkiem_nhanvien.Expires = DateTime.Now.AddDays(1);
            this.timkiem_nhanvien["id"] = "";
            this.timkiem_nhanvien["tendangnhap"] = "";
            this.timkiem_nhanvien["tendaydu"] = "";
            this.timkiem_nhanvien["loainhanvien_id"] = "";
            this.timkiem_nhanvien["active"] = "";
            this.timkiem_nhanvien["email"] = "";
        }
        //
        // GET: /AdminUsers/
        public ActionResult Index()
        {
            //check
            if (!this._permission.Contains("user_view"))
            {
                return this._fail_permission("user_view");
            }
            NhanVienController ctr = new NhanVienController();
            //Chọn danh sách nhân viên để hiển thị theo cookies tìm kiếm
            ViewBag.User_List = ctr.timkiem(
                timkiem_nhanvien["id"],
                timkiem_nhanvien["tendangnhap"],
                timkiem_nhanvien["tendaydu"],
                timkiem_nhanvien["email"],
                timkiem_nhanvien["active"],
                timkiem_nhanvien["loainhanvien_id"]);
            //set search cookies
            ViewBag.User_Search = this.timkiem_nhanvien;
            ViewBag.LoaiNhanVien_List = ctr._db.ds_loainhanvien.ToList();
            //return View(this._db.Users.ToList());
            //this._build_common_data();
            ViewBag.Title += " - Management";
            return View();
        }
        public ActionResult Add()
        {
            if (!this._permission.Contains("user_add"))
            {
                return _fail_permission("user_add");
            }
            //ViewBag.Title += " - Add";
            return RedirectToAction("Add","AdminUser");
        }
        public ActionResult Edit(int id=0)
        {
            if (!this._permission.Contains("user_edit"))
            {
                return _fail_permission("user_edit");
            }
            return RedirectToAction("Index", "AdminUser", new { id = id });
        }
        public ActionResult Delete(int id=0)
        {
            if (!this._permission.Contains("user_delete"))
            {
                return _fail_permission("user_delete");
            }
            new NhanVienController().delete(id);
            return RedirectToAction("Index","AdminUsers");
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
                this.timkiem_nhanvien["id"] = Request["nhanvien_id"] == null ? "" : Request["nhanvien_id"];
                this.timkiem_nhanvien["tendangnhap"] = Request["nhanvien_tendangnhap"] == null ? "" : Request["nhanvien_tendangnhap"];
                this.timkiem_nhanvien["tendaydu"] = Request["nhanvien_tendaydu"] == null ? "" : Request["nhanvien_tendaydu"];
                this.timkiem_nhanvien["email"] = Request["nhanvien_email"] == null ? "" : Request["nhanvien_email"];
                this.timkiem_nhanvien["loainhanvien_id"] = Request["nhanvien_loainhanvien_id"] == null ? "" : Request["nhanvien_loainhanvien_id"];
                this.timkiem_nhanvien["active"] = Request["nhanvien_active"] == null ? "" : Request["nhanvien_active"];
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_nhanvien));
            //redirect
            return RedirectToAction("Index","AdminUsers");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Users";

            //build timkiem_nhanvien
            if (Request.Cookies.Get("timkiem_nhanvien") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_nhanvien));
            }
            else
            {
                this.timkiem_nhanvien = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_nhanvien"));
            }
            //set active tab
            this._set_activetab(new String[] { "NhanVien", "QuanTriHeThong" });
        }
    }
}
