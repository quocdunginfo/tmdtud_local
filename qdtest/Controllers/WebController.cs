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
        protected NhanVien _nhanvien = null;
        public virtual ActionResult Index2()
        {
            return RedirectToAction("Index","FrontHome");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            int uid = 0;
            String password = "";
            if (Session["user_id"] != null)
            {
                //uu tien lay thong tin user tu session
                uid = TextLibrary.ToInt(Session["user_id"].ToString());
                password = TextLibrary.ToString(Session["user_password"].ToString());
            }
            else
            {
                //lay thong tin tu cookies
                HttpCookie _tmp = Request.Cookies.Get("admin");
                if (_tmp != null)
                {
                    uid = TextLibrary.ToInt(_tmp["user_id"].ToString());
                    password = TextLibrary.ToString(_tmp["user_password"].ToString());
                }
            }
            //lay thong tin user theo yeu cau dang nhap
            NhanVienController ctr = new NhanVienController();
            this._nhanvien = ctr.get_by_id_hash_password(uid, password);
        }

    }
}
