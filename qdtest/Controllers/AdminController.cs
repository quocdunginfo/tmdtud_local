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
        protected List<String> _permission = null;
        protected BanGiayDBContext _db = null;
        protected List<String> _state = null;//lưu thông báo trả về cho view
        public AdminController()
        {
            this._db = new BanGiayDBContext();
            this._permission = new List<String>();
            this._state = new List<String>();
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
            int uid = 0;
            String password = "";
            if (Session["user_id"] != null)
            {
                //uu tien lay thong tin user tu session
                uid = TextLibrary.ToInt(Session["user_id"].ToString());
                password = TextLibrary.ToString(Session["user_password"].ToString());
                Debug.WriteLine("Lay thong tin tu session, uid="+uid);
            }
            else
            {
                //lay thong tin tu cookies
                HttpCookie _tmp = Request.Cookies.Get("admin");
                if (_tmp != null)
                {
                    uid = TextLibrary.ToInt(_tmp["user_id"].ToString());
                    password = TextLibrary.ToString(_tmp["user_password"].ToString());
                    Debug.WriteLine("Lay thong tin tu cookies, uid=" + uid);
                }
            }
            //lay thong tin user theo yeu cau dang nhap
            NhanVienController ctr = new NhanVienController();
            this._user = ctr.get_by_id_hash_password(uid, password);
            //nếu chưa đăng nhập thì chuyển tới trang đăng nhập
            if (this._user == null)
            {
                Debug.WriteLine("Kiểm tra User thất bại");
                filterContext.Result = RedirectToAction("Index", "AdminLogin");
                return;
            }
            //Gán permission
                this._reset_permission(this._user.loainhanvien);
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
            ViewBag.Current_User = this._user;
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
            this._permission = new List<string>();
            foreach (Quyen item in obj.ds_quyen)
            {
                this._permission.Add(item.ten);
            }
        }       
    }
}
