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
            int uid = 0;
            String password = "";
            if (Session["user_id"] != null)
            {
                //uu tien lay thong tin user tu session
                uid = TextLibrary.ToInt(Session["user_id"].ToString());
                password = TextLibrary.ToString(Session["user_password"].ToString());
                Debug.WriteLine("Lay thong tin tu session, uid=" + uid);
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
            //nếu đăng nhập roi thì chuyển tới trang đăng nhập
            if (this._user != null)
            {
                Debug.WriteLine("Kiểm tra User thất bại");
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
            Boolean remember = TextLibrary.ToBoolean(Request["user_remember"]);
            //validate
            //NhanVienController c = new NhanVienController();
            if (uc.login(username,password))
            {
                Debug.WriteLine("Đăng nhập thành công");
                NhanVien u = uc.get_by_username(username);
                if (remember)
                {
                    //set Cookies
                    HttpCookie _tmp = new HttpCookie("admin");
                    _tmp["user_id"] = u.id.ToString();
                    _tmp["user_password"] = u.matkhau;
                    _tmp.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(_tmp);
                }
                else
                {
                    //set session
                    Session["user_id"] = u.id.ToString();
                    Session["user_password"] = u.matkhau.ToString();
                }
                //redirect
                return RedirectToAction("Index","AdminHome");
            }
            Debug.WriteLine("Đăng nhập thất bại");
            //load view
            return View("Index");
        }
        public ActionResult Logout()
        {
            //destroy session
            Session["user_id"] = null;
            Session.Clear();
            Session.Abandon();
            //renew cookies for admin
            HttpCookie _tmp = new HttpCookie("admin");
            _tmp.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(_tmp);
            //redirect
            return RedirectToAction("Index","AdminLogin");
        }
    }
}
