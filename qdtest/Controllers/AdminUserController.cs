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
    public class AdminUserController : AdminController
    {
        //
        // GET: /AdminUser/

        public ActionResult Index(int id=0)
        {
            if (!this._permission.Contains("user_view"))
            {
                return _fail_permission("user_view");
            }

            NhanVien u = new NhanVienController().get_by_id(id);

            if (u == null)
            {
                //user khong ton tai
                return RedirectToAction("Index", "AdminUsers");
            }
            ViewBag.NhanVien = u;
            ViewBag.Title += " - View";
            return View();
        }
        public ActionResult Add()
        {
            if (!this._permission.Contains("user_add"))
            {
                return _fail_permission("user_add");
            }
            NhanVien nv = new NhanVien();
            
            ViewBag.NhanVien = nv;
            ViewBag.Title += " - Add";
            return View("Index");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get nv id first
            int nvid = Int32.Parse("0"+Request["nhanvien_id"]);
            NhanVienController nvc=new NhanVienController();
            NhanVien nv;
            Boolean edit_mode = true;
            if (nvid == 0)
            {
                //add mode
                nv = new NhanVien();
                edit_mode = false;
            }
            else
            {
                if (new NhanVienController().is_exist(nvid))
                {
                    //update model
                    //get instance of record of table
                    nv = this._db.ds_nhanvien.Where(x => x.id == nvid).FirstOrDefault();
                }
                else
                {
                    //nvid khong ton tai
                    return RedirectToAction("Index","AdminUsers");
                }
            }
            //assign data
            nv.email = Request["nhanvien_email"];
            nv.group_id = TextLibrary.ToInt(Request["nhanvien_group_id"]);
            nv.tendangnhap = Request["nhanvien_tendangnhap"];
            nv.tendaydu = Request["nhanvien_tendaydu"];
            nv.active = Request["nhanvien_active"]=="1"?true:false;
            //action
            if (edit_mode)
            {
                if (!this._permission.Contains("user_edit"))
                {
                    return _fail_permission("user_edit");
                }
                //call set password
                nvc.set_password(nv.id, Request["nhanvien_matkhau"]);
                //call update
                Debug.WriteLine("Call DB Update");
                this._db.SaveChanges();
                ViewBag.State = "edit_ok";
            }
            else
            {
                if (!this._permission.Contains("user_add"))
                {
                    return _fail_permission("user_add");
                }
                //hash password before add
                nv.matkhau = TextLibrary.GetSHA1HashData(Request["nhanvien_matkhau"]);
                //call add
                Debug.WriteLine("Add Nhan vien");
                int maxid = nvc.add(nv);
                //re assign id
                nv.id = maxid;
                ViewBag.State = "add_ok";
            }
            ViewBag.NhanVien = nv;
            ViewBag.Title += " - Submit";
            
            return View("Index");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - User";
            //đặt activetab cố định cho controller này
            this._current_activetab();
        }
        private void _current_activetab()
        {
            this._set_activetab(new String[] { "QuanTriHeThong", "NhanVien"} );
        }

    }
}
