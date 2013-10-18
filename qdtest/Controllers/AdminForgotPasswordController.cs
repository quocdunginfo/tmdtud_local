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
    public class AdminForgotPasswordController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //generate session for NhanVien
            NhanVienController ctr=new NhanVienController();
            String email = TextLibrary.ToString(Request["user_email"]);
            String session = "";
            Boolean valid_session = ctr.generate_forgot_password_session(email, out session);
            if (valid_session && ValidateLibrary.is_valid_email(email))
            {
                GMailLibrary gmail = new GMailLibrary();
                gmail.receive_email = email;
                gmail.Generate_ForgotPassword_Html(Url.Action("Request_Password_Change", "AdminForgotPassword", new { session = session }, this.Request.Url.Scheme));
                gmail.Send();
                ViewBag.Message = "Đường link khôi phục mật khẩu đã được gửi vào email.";
            }
            else
            {
                ViewBag.Message = "Không tìm thấy nhân viên nào có email đó cả.";
            }
            return View("Request_Password_Change_Submit");
        }
        [HttpGet]
        public ActionResult Request_Password_Change(String session)
        {
            Debug.WriteLine("Request change password from session " + session);
            NhanVienController ctr = new NhanVienController();
            NhanVien obj = ctr.timkiem("", "", "","", "", "",session).FirstOrDefault();
            if(obj!=null)
            {
                ViewBag.NhanVien = obj;
                return View();
            }
            else
            {
                return RedirectToAction("Index","Admin");
            }
            
        }
        [HttpPost]
        public ActionResult Request_Password_Change_Submit()
        {
            int obj_id = TextLibrary.ToInt(Request["nhanvien_id"]);
            String session = TextLibrary.ToString(Request["nhanvien_session"]);
            String new_pass = TextLibrary.ToString(Request["nhanvien_matkhau"]);
            NhanVienController ctr = new NhanVienController();
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
