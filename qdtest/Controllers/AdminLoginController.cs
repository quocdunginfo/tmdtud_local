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
            //vd ve su thay dọic
            HttpCookie _tmp = Request.Cookies.Get("admin");
            if (_tmp == null) return View();
            //Nếu đã đang nhập rồi thì return home
            NhanVienController uc = new NhanVienController();
            if (uc.get_by_id_hash_password(TextLibrary.ToInt(_tmp["user_id"]),_tmp["user_password"])!=null)
            {
                return RedirectToAction("Index","AdminHome");
            }
            //hien thi form login
            return View();
            
        }
        [HttpPost]
        public ActionResult Test_Login()
        {
            //get data from client
            NhanVienController uc = new NhanVienController();
            String username = TextLibrary.ToString(Request["user_username"]);
            String password = TextLibrary.ToString(Request["user_password"]);
            //validate
            //NhanVienController c = new NhanVienController();
            if (uc.login(username,password))
            {
                Debug.WriteLine("Đăng nhập thành công");
                NhanVien u = uc.get_by_username(username);
                //set Cookies
                HttpCookie _tmp = new HttpCookie("admin");
                _tmp["user_id"] = u.id.ToString();
                _tmp["user_password"] = u.matkhau;
                _tmp.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(_tmp);

                return RedirectToAction("Index","AdminHome");
            }
            Debug.WriteLine("Đăng nhập thất bại");
            //load view
            return View("Index");
        }
        public ActionResult Logout()
        {
            //renew cookies for admin
            HttpCookie _tmp = new HttpCookie("admin");
            _tmp.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(_tmp);
            return RedirectToAction("Index","AdminLogin");
        }
    }
}
