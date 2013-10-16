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
            int obj_id = TextLibrary.ToInt(Request["nhanvien_id"]);
            NhanVienController ctr=new NhanVienController();
            NhanVien obj;
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                obj = new NhanVien();
                edit_mode = false;
            }
            else
            {
                if (ctr.is_exist(obj_id))
                {
                    //update model
                    //get instance of record of table
                    obj = ctr.get_by_id(obj_id);
                }
                else
                {
                    //nvid khong ton tai
                    return RedirectToAction("Index","AdminUsers");
                }
            }
            //assign data
            Boolean validate_ok = true;
            obj.email = TextLibrary.ToString(Request["nhanvien_email"]);
            LoaiNhanVien loai = ctr._db.ds_loainhanvien.Where(x => x.id == TextLibrary.ToInt(Request["nhanvien_group_id"])).FirstOrDefault();
            obj.loainhanvien = loai;
            obj.tendangnhap = TextLibrary.ToString(Request["nhanvien_tendangnhap"]);
            obj.tendaydu = TextLibrary.ToString(Request["nhanvien_tendaydu"]);
            obj.active = TextLibrary.ToBoolean(Request["nhanvien_active"]);
            //action
            if (validate_ok)
            {
                if (edit_mode)
                {
                    if (!this._permission.Contains("user_edit"))
                    {
                        return _fail_permission("user_edit");
                    }
                    //update properties
                    ctr._db.SaveChanges();
                    //call set password
                    ctr.set_password(obj.id, TextLibrary.ToString(Request["nhanvien_matkhau"]));
                    this._state.Add("edit_ok");
                }
                else
                {
                    if (!this._permission.Contains("user_add"))
                    {
                        return _fail_permission("user_add");
                    }
                    //hash password before add
                    obj.matkhau = TextLibrary.GetSHA1HashData(Request["nhanvien_matkhau"]);
                    //call add
                    int maxid = ctr.add(obj);
                    //re assign id
                    obj.id = maxid;
                    this._state.Add("add_ok");
                }
            }
            ViewBag.NhanVien = obj;
            ViewBag.Title += " - Submit";
            ViewBag.State = this._state;
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
