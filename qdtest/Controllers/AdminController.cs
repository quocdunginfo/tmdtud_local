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
    public class AdminController : Controller
    {
        protected NhanVien _user = null;
        public List<String> _permission = null;
        public BanGiayDBContext _db = null;
        public AdminController()
        {
            //vi du về sự thay đổi
            this._db = new BanGiayDBContext();
            this._permission = new List<String>();
        }
        public ActionResult Index2()
        {
            if (this._user != null)
            {
                return this._login_page();
            }
            return this._home_page();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Debug.WriteLine("Kiểm tra User");
            base.OnActionExecuting(filterContext);
            //Lấy thông tin user từ cookies
            HttpCookie _tmp = Request.Cookies.Get("admin");
            if (_tmp != null)
            {
                NhanVienController uc = new NhanVienController();
                Debug.WriteLine("Phát hiện user_id=" + _tmp["user_id"]);
                this._user = uc.get_by_id_hash_password(TextLibrary.ToInt(_tmp["user_id"]),_tmp["user_password"]);
                //Gán permission
                this._reset_permission(this._user.group_id);
            }
            //nếu chưa đăng nhập thì chuyển tới trang đăng nhập
            if (this._user == null)
            {
                Debug.WriteLine("Kiểm tra User thất bại");
                filterContext.Result = RedirectToAction("Index", "AdminLogin");
            }
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
            TempData.Add("admin_fail_message","You are not allowed in "+permission+" area!");
            return RedirectToAction("Index", "AdminFail");
        }
        [NonAction]
        protected void _build_common_data()
        {
            //for menu active item
            ViewBag.Title = "Admin";
            ViewBag.ActiveTab = new List<String>();
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
        private void _reset_permission(int group_id)
        {
            this._permission = new List<string>();
            if (group_id == 0)
            {
                //admin group
                this._permission.Add("home_view");
                this._permission.Add("home_edit");
                this._permission.Add("home_delete");
                this._permission.Add("home_add");

                this._permission.Add("product_view");
                this._permission.Add("product_edit");
                this._permission.Add("product_delete");
                this._permission.Add("product_add");

                this._permission.Add("user_view");
                this._permission.Add("user_edit");
                this._permission.Add("user_delete");
                this._permission.Add("user_add");
            }
            if (group_id == 1)
            {
                //admin group
                this._permission.Add("home_view");
                //this._permission.Add("home_edit");
                //this._permission.Add("home_delete");
                //this._permission.Add("home_add");

                this._permission.Add("product_view");
                //this._permission.Add("product_edit");
                //this._permission.Add("product_delete");
                //this._permission.Add("product_add");

                //this._permission.Add("user_view");
                //this._permission.Add("user_edit");
                //this._permission.Add("user_delete");
                //this._permission.Add("user_add");
            }
            if (group_id == 2)
            {
                
            }
        }
        
    }
}
