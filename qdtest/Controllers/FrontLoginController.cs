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
    public class FrontLoginController : FrontController
    {
        public ActionResult Index()
        {
            if (this._is_logged_in())
            {
                return RedirectToAction("Index","FrontHome");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Test_Login()
        {
            //get data from client
            KhachHangController ctr = new KhachHangController();
            String username = TextLibrary.ToString(Request["khachhang_username"]);
            String password = TextLibrary.ToString(Request["khachhang_password"]);
            Boolean remember = TextLibrary.ToBoolean(Request["khachhang_remember"]);
            //validate
            if (ctr.login(username, password))
            {
                Debug.WriteLine("Đăng nhập thành công");
                KhachHang obj = ctr.get_by_username(username);
                if (remember)
                {
                    //set Cookies
                    HttpCookie _tmp = new HttpCookie("khachhang");
                    _tmp["khachhang_id"] = obj.id.ToString();
                    _tmp["khachhang_password"] = obj.matkhau;
                    _tmp.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(_tmp);
                }
                else
                {
                    //set session
                    Session["khachhang"] = obj;
                }
                //redirect
                return RedirectToAction("Index", "FrontHome");
            }
            Debug.WriteLine("Đăng nhập thất bại");
            //load view
            return RedirectToAction("Index","FrontLogin");
        }
        public ActionResult Logout()
        {
            //destroy session
            Session["khachhang"] = null;
            //renew cookies for khachhang
            HttpCookie _tmp = new HttpCookie("khachhang");
            _tmp.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(_tmp);
            //redirect
            return RedirectToAction("Index", "FrontHome");
        }
    }
}
