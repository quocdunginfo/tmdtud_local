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
    public class AdminNhomSanPhamController : AdminController
    {
        private HttpCookie timkiem_nhomsanpham;
        public AdminNhomSanPhamController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_nhomsanpham = new HttpCookie("timkiem_nhomsanpham");
            timkiem_nhomsanpham.Expires = DateTime.Now.AddDays(1);
            this.timkiem_nhomsanpham["id"] = "";
            this.timkiem_nhomsanpham["ten"] = "";
            this.timkiem_nhomsanpham["mota"] = "";
            this.timkiem_nhomsanpham["active"] = "";
        }
        //
        // GET: /AdminNhomSanPham/
        public ActionResult Index()
        {
            NhomSanPhamController ctr = new NhomSanPhamController();
            NhomSanPham root = null;
            ViewBag.NhomSanPham2_List = ctr.timkiem(
                this.timkiem_nhomsanpham["id"],
                this.timkiem_nhomsanpham["ten"],
                this.timkiem_nhomsanpham["mota"],
                this.timkiem_nhomsanpham["active"]);
            ViewBag.Title += " - View";
            ViewBag.timkiem_nhomsanpham = this.timkiem_nhomsanpham;
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
                this.timkiem_nhomsanpham["id"] = TextLibrary.ToString(Request["id"]);
                this.timkiem_nhomsanpham["ten"] = TextLibrary.ToString(Request["ten"]);
                this.timkiem_nhomsanpham["mota"] = TextLibrary.ToString(Request["mota"]);
                this.timkiem_nhomsanpham["active"] = TextLibrary.ToString(Request["active"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_nhomsanpham));
            //redirect
            return RedirectToAction("Index", "AdminNhomSanPham");
        }
        [HttpPost]
        public ActionResult EditSubmit()
        {
            NhomSanPhamController ctr = new NhomSanPhamController();
            //get data
            int id = TextLibrary.ToInt(Request["cat_id"]);
            if (!ctr.is_exist(id))
            {
                Debug.WriteLine("Mã nhóm sản phẩm không tồn tại, id=" + id);
                return RedirectToAction("Index", "AdminNhomSanPham");
            }
            String cat_name = TextLibrary.ToString(Request["cat_name"]);
            Boolean cat_active = TextLibrary.ToBoolean(Request["cat_active"]);
            String cat_description =  TextLibrary.ToString(Request["cat_description"]);
            //get curent cat object and pass update info
            NhomSanPham obj = ctr.get_by_id(id);
            obj.ten = cat_name;
            obj.mota = cat_description;
            obj.active = cat_active;
            //call update
            ctr._db.SaveChanges();
            Debug.WriteLine("Cập nhật thành công Nhóm Sản phẩm");
            return RedirectToAction("Index", "AdminNhomSanPham");
        }
        public ActionResult Edit(int id=0)
        {
            //get data
            NhomSanPhamController ctr = new NhomSanPhamController();
            if (!ctr.is_exist(id))
            {
                Debug.WriteLine("Mã nhóm sản phẩm không tồn tại, id="+id);
                return RedirectToAction("Index","AdminNhomSanPham");
            }
            ViewBag.nhom_san_pham = ctr.get_by_id(id);
            ViewBag.Title += " - Edit";
            return View();
        }
        [HttpPost]
        public ActionResult Add()
        {
            //get data
            int p_id = TextLibrary.ToInt(Request["cat_parent_id"]);
            //nên lấy instance của chính Controller đó để dùng sẽ không bị lỗi track changes
            NhomSanPhamController controller = new NhomSanPhamController();

            NhomSanPham obj = new NhomSanPham();
            obj.ten =  TextLibrary.ToString(Request["cat_name"]);
            obj.mota = TextLibrary.ToString(Request["cat_description"]);
            obj.active = TextLibrary.ToBoolean(Request["cat_active"]);
            if (obj.ten.Equals(""))
            {
                //lỗi
                Debug.WriteLine("Lỗi cat_name= rỗng");
                return RedirectToAction("Index", "AdminNhomSanPham");
            }
            NhomSanPham p_obj = controller._db.ds_nhomsanpham.Where(x => x.id == p_id).FirstOrDefault();
            if (p_obj != null)
            {
                obj.nhomcha = p_obj;
            }
            //call add
            controller.add(obj);
            Debug.WriteLine("Thêm Thành Công Nhóm Sản Phẩm");
            return RedirectToAction("Index","AdminNhomSanPham");
        }
        public ActionResult Delete(int id=0)
        {
            NhomSanPhamController controller = new NhomSanPhamController();
            if (!controller.is_exist(id))
            {
                Debug.WriteLine("Delete Nhóm sản phẩm id không tồn tại");
                return RedirectToAction("Index", "AdminNhomSanPham");
            }
            controller.delete(id);
            Debug.WriteLine("Delete Nhóm sản phẩm thành công (xóa luôn con)");
            return RedirectToAction("Index", "AdminNhomSanPham");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - NhomSanPham";

            //build timkiem_nhanvien
            if (Request.Cookies.Get("timkiem_nhomsanpham") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_nhomsanpham));
            }
            else
            {
                this.timkiem_nhomsanpham = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_nhomsanpham"));
            }
            this._set_activetab(new String[] { "NhomSanPham", "Catalog"});
        }

    }
}
