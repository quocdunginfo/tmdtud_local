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
    public class FrontKhachHangController : FrontController
    {
        public ActionResult Index()
        {
            if (!this._is_logged_in() || this._khachhang==null)
            {
                //dẫn link sau khi dăng nhập
                Session["link_after_login"] = Url.Action("Index","FrontKhachHang");
                return RedirectToAction("Index","FrontLogin");
            }
            ViewBag.State = new List<string>();
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            KhachHangController ctr = new KhachHangController();
            List<string> validate = new List<string>();
            //get post value
            int id = TextLibrary.ToInt(Request["khachhang_id"]);
            string tendaydu = TextLibrary.ToString(Request["khachhang_tendaydu"]);
            string matkhau = TextLibrary.ToString(Request["khachhang_matkhau"]);
            string matkhau2 = TextLibrary.ToString(Request["khachhang_matkhau2"]);
            string diachi = TextLibrary.ToString(Request["khachhang_diachi"]);
            string sdt = TextLibrary.ToString(Request["khachhang_sdt"]);
            string email = TextLibrary.ToString(Request["khachhang_email"]);
            //load obj first
            KhachHang obj = ctr.get_by_id(id);
            if (obj == null)
            {
                return RedirectToAction("Index", "FrontKhachHang");
            }
            //pass to obj
            obj.diachi = diachi;
            obj.email = email;
            obj.sdt = sdt;
            obj.tendaydu = tendaydu;
            //validate
            validate.AddRange(ctr.validate(obj,matkhau,matkhau2));
            //update
            if (validate.Count == 0)
            {
                //call update for properties first
                ctr._db.SaveChanges();
                //call set password
                ctr.set_password(obj.id, matkhau2);
                obj = ctr.get_by_id(obj.id);
                this._khachhang = obj;
                Session["khachhang"] = this._khachhang;
                validate.Add("edit_ok");
            }

            ViewBag.khachhang = this._khachhang;
            //report back
            ViewBag.State = validate;
            return View("Index");
        }
    }
}
