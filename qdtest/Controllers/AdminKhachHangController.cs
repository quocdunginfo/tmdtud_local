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
    public class AdminKhachHangController : AdminController
    {
        public ActionResult Index(int id = 0)
        {
            if (!this._permission.Contains("khachhang_view"))
            {
                return _fail_permission("khachhang_view");
            }
            //get controller
            KhachHangController ctr = new KhachHangController();
            //kiểm tra tồn tại
                if (!ctr.is_exist(id)) return RedirectToAction("Index", "AdminKhachHangs");
            //get info
                ViewBag.KhachHang = ctr.get_by_id(id);
                ViewBag.Title += " - Xem chi tiết";
                return View();
        }
        public ActionResult Add()
        {
            //check
            if (!this._permission.Contains("khachhang_add"))
            {
                return this._fail_permission("khachhang_add");
            }
            KhachHang obj = new KhachHang();

            ViewBag.KhachHang = obj;
            ViewBag.Title += " - Thêm mới";
            return View("Index");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get obj id first
            int obj_id = TextLibrary.ToInt(Request["khachhang_id"]);
            KhachHangController ctr = new KhachHangController();
            //khoi tao moi doi tuong
            KhachHang obj;
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                //check permission
                if (!this._permission.Contains("khachhang_add"))
                {
                    return this._fail_permission("khachhang_add");
                }
                obj = new KhachHang();
                edit_mode = false;
            }
            else
            {
                //update mode
                //check permission
                if (!this._permission.Contains("khachhang_edit"))
                {
                    return _fail_permission("khachhang_edit");
                }
                //kiem tra ton tai
                if (ctr.is_exist(obj_id))
                {
                    //get obj
                    obj = ctr.get_by_id(obj_id);
                }
                else
                {
                    //id khong ton tai
                    return RedirectToAction("Index", "AdminKhachHangs");
                }
            }
            //assign data
            Boolean validate_ok = true;
            obj.email = TextLibrary.ToString(Request["khachhang_email"]);
                if (obj.email.Equals("")) { this._state.Add("email_fail"); validate_ok = false; }

            obj.sdt = TextLibrary.ToString(Request["khachhang_sdt"]);
                if (obj.sdt.Equals("")) { this._state.Add("sdt_fail"); validate_ok = false; }

            obj.diachi = TextLibrary.ToString(Request["khachhang_diachi"]);
                if (obj.diachi.Equals("")) { this._state.Add("diachi_fail"); validate_ok = false; }
            
            obj.tendangnhap =  TextLibrary.ToString(Request["khachhang_tendangnhap"]);
                if (obj.tendangnhap.Equals("")) { this._state.Add("tendangnhap_fail"); validate_ok = false; }
            
            obj.tendaydu =  TextLibrary.ToString(Request["khachhang_tendaydu"]);
                if (obj.tendaydu.Equals("")) { this._state.Add("tendaydu_fail"); validate_ok = false; }
            obj.active = TextLibrary.ToBoolean(Request["khachhang_active"]);
            //action
            if (validate_ok)
            {
                if (edit_mode)
                {
                    //call update for properties
                    ctr._db.SaveChanges();
                    //call set password
                    ctr.set_password(obj.id, TextLibrary.ToString(Request["khachhang_matkhau"]));
                    ViewBag.State = "edit_ok";
                }
                else
                {
                    //set raw password
                    obj.matkhau = TextLibrary.ToString(Request["khachhang_matkhau"]);
                    //call add
                    int maxid = ctr.add(obj);
                    //re assign id
                    obj.id = maxid;
                    ViewBag.State = "add_ok";
                }
            }
            ViewBag.KhachHang = obj;
            ViewBag.Title += " - Submit";
            ViewBag.State = this._state;
            return View("Index");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Khách hàng";
            //đặt activetab cố định cho controller này
            this._current_activetab();
        }
        private void _current_activetab()
        {
            this._set_activetab(new String[] { "KhachHang" });
        }

    }
}
