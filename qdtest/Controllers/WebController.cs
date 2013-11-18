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
    public class WebController : Controller
    {
        protected NhanVien _nhanvien;
        public WebController()
        {
            this._nhanvien = null;
        }
        public virtual ActionResult Index2()
        {
            return RedirectToAction("Index","FrontHome");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            if (Session["nhanvien"] != null)
            {
                this._nhanvien = (NhanVien)Session["nhanvien"];
            }
            else
            {
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
                this._nhanvien = ctr.get_by_id_hash_password(uid, password);
            }
            
        }
    }
}
