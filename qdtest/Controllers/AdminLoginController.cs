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
    public class AdminLoginController : Controller
    {
        private BanGiayDBContext _db = new BanGiayDBContext();
        private NhanVien _user = new NhanVien();
        //
        // GET: /AdminLogin/
        public ActionResult Index()
        {
            if (Session["nhanvien"] != null)
            {
                return RedirectToAction("Index", "AdminHome");
            }
            int uid = 0;
            String password = "";
            //lay thong tin tu cookies
            HttpCookie _tmp = Request.Cookies.Get("nhanvien");
            if (_tmp != null)
            {
                uid = TextLibrary.ToInt(_tmp["user_id"].ToString());
                password = TextLibrary.ToString(_tmp["user_password"].ToString());
            }

            //lay thong tin user theo yeu cau dang nhap
            NhanVienController ctr = new NhanVienController();
            this._user = ctr.get_by_id_hash_password(uid, password);
            //nếu đăng nhập roi thì chuyển tới trang đăng nhập
            if (this._user != null)
            {
                return RedirectToAction("Index", "AdminHome");
            }

            //hien thi form login
            ViewBag.State = new List<string>();
            return View();
        }
        [HttpPost]
        public ActionResult Test_Login()
        {
            //get data from client
            NhanVienController uc = new NhanVienController();
            String username = TextLibrary.ToString(Request["user_username"]);
            String password = TextLibrary.ToString(Request["user_password"]);
            Boolean remember = TextLibrary.ToBoolean(Request["user_remember"]);
            //validate
            //NhanVienController c = new NhanVienController();
            if (uc.login(username,password))
            {
                NhanVien obj = uc.get_by_username(username);
                if (remember)
                {
                    //set Cookies
                    HttpCookie _tmp = new HttpCookie("nhanvien");
                    _tmp["user_id"] = obj.id.ToString();
                    _tmp["user_password"] = obj.matkhau;
                    _tmp.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(_tmp);
                }
                else
                {
                    //set session
                    Session["nhanvien"] = obj;
                }
                //redirect
                return RedirectToAction("Index","AdminHome");
            }
            //load view
            List<String> validate= new List<String>();
            validate.Add("fail");
            ViewBag.State = validate;
            return View("Index");
        }
        public ActionResult Logout()
        {
            //destroy session
            Session["nhanvien"] = null;
            //renew cookies for admin
            HttpCookie _tmp = new HttpCookie("nhanvien");
            _tmp.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(_tmp);
            //redirect
            return RedirectToAction("Index","AdminLogin");
        }
    }
}
