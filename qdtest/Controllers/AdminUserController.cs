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
            NhanVienController ctr = new NhanVienController();
            NhanVien u = ctr.get_by_id(id);

            if (u == null)
            {
                //user khong ton tai
                return RedirectToAction("Index", "AdminUsers");
            }
            ViewBag.NhanVien = u;
            ViewBag.Title += " - View";
            ViewBag.LoaiNhanVien_List = ctr._db.ds_loainhanvien.ToList();
            return View();
        }
        public ActionResult Add()
        {
            if (!this._permission.Contains("user_add"))
            {
                return _fail_permission("user_add");
            }
            NhanVien nv = new NhanVien();
            NhanVienController ctr=new NhanVienController();
            ViewBag.NhanVien = nv;
            ViewBag.Title += " - Add";
            ViewBag.LoaiNhanVien_List = ctr._db.ds_loainhanvien.ToList();
            return View("Index");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get nv id first
            int obj_id = TextLibrary.ToInt(Request["nhanvien_id"]);
            NhanVienController ctr=new NhanVienController();
            NhanVien obj;
            //check mode
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                if (!this._permission.Contains("user_add"))
                {
                    return _fail_permission("user_add");
                }

                obj = new NhanVien();
                edit_mode = false;
            }
            else
            {
                //edit mode
                if (!this._permission.Contains("user_edit"))
                {
                    return _fail_permission("user_edit");
                }

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
            //assign value
            obj.email = TextLibrary.ToString(Request["nhanvien_email"]);
            obj.tendangnhap = TextLibrary.ToString(Request["nhanvien_tendangnhap"]);
            obj.tendaydu = TextLibrary.ToString(Request["nhanvien_tendaydu"]);
            //validate properties
            List<String> validate = ctr.validate(obj,
                TextLibrary.ToString(Request["nhanvien_matkhau"]),
                TextLibrary.ToString(Request["nhanvien_matkhau2"]));
            //check self modify
            //bản thân không thể tự thay đổi active hoặc nhóm người dùng
                if (edit_mode && this._user.id != obj.id)
                {
                    //active
                    obj.active = TextLibrary.ToBoolean(Request["nhanvien_active"]);
                    //loainhanvien
                    int lnv_id = TextLibrary.ToInt(Request["nhanvien_loainhanvien_id"]);
                    LoaiNhanVien loai = ctr._db.ds_loainhanvien.Where(x => x.id == lnv_id).FirstOrDefault();
                    obj.loainhanvien = loai;
                    if (obj.loainhanvien == null)
                    {
                        return RedirectToAction("Index", "AdminUsers");
                    }
                }
                else
                {
                    if (obj.active != TextLibrary.ToBoolean(Request["nhanvien_active"]))
                    {
                        validate.Add("self_active_edit_fail");
                    }
                    if (obj.loainhanvien.id != TextLibrary.ToInt(Request["nhanvien_loainhanvien_id"]))
                    {
                        validate.Add("self_loainguoidung_edit_fail");
                    }
                }
            //action
            if (validate.Count==0)
            {
                if (edit_mode)
                {
                    //update properties first
                    ctr._db.SaveChanges();
                    //call set password
                    ctr.set_password(obj.id, TextLibrary.ToString(Request["nhanvien_matkhau"]));
                    this._state.Add("edit_ok");
                }
                else
                {
                    //hash password before add
                    obj.matkhau = TextLibrary.GetSHA1HashData(Request["nhanvien_matkhau"]);
                    //call add
                    int maxid = ctr.add(obj);
                    //re assign id
                    obj.id = maxid;
                    this._state.Add("add_ok");
                }
            }
            this._state.AddRange(validate);
            ViewBag.State = this._state;
            ViewBag.NhanVien = obj;
            ViewBag.Title += " - Submit";
            ViewBag.LoaiNhanVien_List = ctr._db.ds_loainhanvien.ToList();
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
