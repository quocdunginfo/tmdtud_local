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
    public class AdminController : WebController
    {
        protected List<String> _nhanvien_permission;
        protected BanGiayDBContext _db;
        protected List<String> _state;//lưu thông báo trả về cho view
        public AdminController()
        {
            this._db = new BanGiayDBContext();
            this._nhanvien_permission = new List<String>();
            this._state = new List<String>();
        }
        public override ActionResult Index2()
        {
            if (this._nhanvien != null)
            {
                return this._login_page();
            }
            return this._home_page();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //nếu chưa đăng nhập thì chuyển tới trang đăng nhập
            if (this._nhanvien == null)
            {
                filterContext.Result = RedirectToAction("Index", "AdminLogin");
                return;
            }
            //Gán permission
                this._reset_permission(this._nhanvien.loainhanvien);
            //tạo data ban đầu
                this._build_common_data();
            //Mọi class controller extends từ class AdminController sẽ chạy sau dòng định nghĩa này
            //...
            
        }
        protected ActionResult _login_page()
        {
            return RedirectToAction("Index","AdminLogin");
        }
        protected ActionResult _home_page()
        {
            return RedirectToAction("Index", "AdminHome");
        }
        protected ActionResult _fail_permission(String permission)
        {
            TempData.Add("admin_fail_message","You are not allowed in \""+permission+"\" area!");
            return RedirectToAction("Index", "AdminFail");
        }
        protected ActionResult _show_notification(String message)
        {
            TempData.Add("admin_fail_message", message);
            return RedirectToAction("Index", "AdminFail");
        }
        [NonAction]
        protected void _build_common_data()
        {
            //for menu active item
            ViewBag.Title = "Admin";
            ViewBag.ActiveTab = new List<String>();
            ViewBag.State = new List<String>();
            ViewBag.Current_User = this._nhanvien;
        }
        [NonAction]
        protected void _set_activetab(String[] tabs_name)
        {
            //for menu active item
            if (tabs_name == null || tabs_name.Length==0)
            {
                ViewBag.AcTiveTab = new List<String>();
                return;
            }
            ViewBag.ActiveTab = new List<String>(tabs_name);
        }
        [NonAction]
        private void _reset_permission(LoaiNhanVien obj)
        {
            this._nhanvien_permission = new List<string>();
            foreach (Quyen item in obj.ds_quyen)
            {
                this._nhanvien_permission.Add(item.ten);
            }
        }       
    }
}
