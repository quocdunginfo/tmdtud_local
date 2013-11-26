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
    public class FrontForgotPasswordController : FrontController
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //generate session for KH
            KhachHangController ctr=new KhachHangController();
            String email = TextLibrary.ToString(Request["khachhang_email"]);
            String session = "";
            Boolean valid_session = ctr.generate_forgot_password_session(email, out session);
            if (valid_session && ValidateLibrary.is_valid_email(email))
            {
                GMailLibrary gmail = new GMailLibrary();
                gmail.receive_email = email;
                gmail.Generate_ForgotPassword_Html(Url.Action("Request_Password_Change", "FrontForgotPassword", new { session = session }, this.Request.Url.Scheme));
                gmail.Send();
                ViewBag.Message = "Đường link khôi phục mật khẩu đã được gửi vào email.";
            }
            else
            {
                ViewBag.Message = "Không tìm thấy khách hàng nào có email đó cả.";
            }
            return View("Request_Password_Change_Submit");
        }
        [HttpGet]
        public ActionResult Request_Password_Change(String session)
        {
            Debug.WriteLine("Request change password from session " + session);
            KhachHangController ctr = new KhachHangController();
            KhachHang obj = ctr.timkiem("","","",null,0,0,"","","",session).FirstOrDefault();
            if(obj!=null)
            {
                ViewBag.khachhang_264 = obj;
                return View();
            }
            else
            {
                return RedirectToAction("Index","FrontHome");
            }
            
        }
        [HttpPost]
        public ActionResult Request_Password_Change_Submit()
        {
            int obj_id = TextLibrary.ToInt(Request["khachhang_id"]);
            String session = TextLibrary.ToString(Request["khachhang_session"]);
            String new_pass = TextLibrary.ToString(Request["khachhang_matkhau"]);
            KhachHangController ctr = new KhachHangController();
            if (ctr.set_password_by_session(obj_id, session, new_pass))
            {
                ViewBag.Message = "Mật khẩu đã được khôi phục lại.";
            }
            else
            {
                ViewBag.Message = "Oops. Are you trying to hack my system ?";
            }
            return View();
        }

    }
}
