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
        private BlogDBContext _db = new BlogDBContext();
        private UserModel _user = new UserModel();
        //
        // GET: /AdminLogin/
        public ActionResult Index()
        {
            HttpCookie _tmp = Request.Cookies.Get("admin");
            if (_tmp == null) return View();
            //Nếu đã đang nhập rồi thì return home
            UserModelController uc = new UserModelController();
            if (uc.get_by_id_password(_tmp["user_id"],_tmp["user_password"])!=null)
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
            UserModelController uc = new UserModelController();
            String username = Request["user_username"];
            String password = Request["user_password"];
            //validate
            //UserModelController c = new UserModelController();
            if (uc.login(username,password))
            {
                Debug.WriteLine("Đăng nhập thành công");
                UserModel u = (from user in _db.Users
                     where user.username == username
                     select user).FirstOrDefault();
                //set Cookies
                HttpCookie _tmp = new HttpCookie("admin");
                _tmp["user_id"] = u.id.ToString();
                _tmp["user_password"] = u.password.ToString();
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
            //re new cookies
            HttpCookie _tmp = new HttpCookie("admin");
            _tmp.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(_tmp);
            return RedirectToAction("Index","AdminLogin");
        }
    }
}
