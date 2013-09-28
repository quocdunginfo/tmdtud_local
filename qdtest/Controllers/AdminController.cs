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
        protected UserModel _user = null;
        public List<String> _permission = null;
        public BlogDBContext _db = null;
        public AdminController()
        {
            this._db = new BlogDBContext();
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
                UserModelController uc = new UserModelController();
                Debug.WriteLine("Phát hiện user_id=" + _tmp["user_id"]);
                this._user = uc.get_by_id_password(_tmp["user_id"],_tmp["user_password"]);
                //Gán permission
                this._reset_permission(0);
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
                
            }
            if (group_id == 2)
            {
                
            }
        }
    }
}
