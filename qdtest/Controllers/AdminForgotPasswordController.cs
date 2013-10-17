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
        //
        // GET: /AdminForgotPassword/

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
            List<NhanVien> obj_list = ctr.timkiem("", "", "", email, "", "");
            String session = TextLibrary.GetSHA1HashData(DateTime.Now.ToLongTimeString());
            foreach (NhanVien obj in obj_list)
            {
                obj.forgot_password_session = session;
                Debug.WriteLine("Make new "+session);
                ctr._db.SaveChanges();
            }
            GMailLibrary gmail = new GMailLibrary();
            gmail.receive_email = email;
            gmail.Generate_ForgotPassword_Html(Url.Action("Request_Password_Change", "AdminForgotPassword", new { session = session }));
            //gmail.Send();
            return View("Index");
        }
        public ActionResult Request_Password_Change(String session)
        {
            Debug.WriteLine(session);
            NhanVienController ctr = new NhanVienController();
            List<NhanVien> obj_list = ctr.timkiem("", "", "","", "", "",session);
            foreach (NhanVien obj in obj_list)
            {
                ViewBag.NhanVien = obj;
                break;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Request_Password_Change_Submit()
        {
            int obj_id = TextLibrary.ToInt(Request["nhanvien_id"]);
            String session = TextLibrary.ToString(Request["nhanvien_session"]);
            String new_pass = TextLibrary.ToString(Request["nhanvien_matkhau"]);
            NhanVienController ctr = new NhanVienController();
            List<NhanVien> obj_list = ctr.timkiem(obj_id.ToString(), "", "", "", "", "", session);
            foreach (NhanVien obj in obj_list)
            {
                ctr.set_password(obj.id, new_pass);
            }
            return View("Index");
        }

    }
}
