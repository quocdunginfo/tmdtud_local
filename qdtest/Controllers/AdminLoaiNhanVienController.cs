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
    public class AdminLoaiNhanVienController : AdminController
    {
        public ActionResult Index(int id = 0)
        {
            if (!this._nhanvien_permission.Contains("loainhanvien_view"))
            {
                return _fail_permission("loainhanvien_view");
            }
            LoaiNhanVienController ctr = new LoaiNhanVienController();
            LoaiNhanVien obj = ctr.get_by_id(id);

            if (obj == null)
            {
                //kich thuoc khong ton tai
                return RedirectToAction("Index", "AdminLoaiNhanViens");
            }
            ViewBag.LoaiNhanVien = obj;
            ViewBag.Quyen_ListAll = ctr._db.ds_quyen.ToList();
            ViewBag.Title += " - View";
            return View();
        }
        public ActionResult Add()
        {
            if (!this._nhanvien_permission.Contains("loainhanvien_add"))
            {
                return _fail_permission("loainhanvien_add");
            }
            LoaiNhanVienController ctr = new LoaiNhanVienController();
            LoaiNhanVien obj = new LoaiNhanVien();
            ViewBag.LoaiNhanVien = obj;
            ViewBag.Title += " - Add";
            ViewBag.Quyen_ListAll = ctr._db.ds_quyen.ToList();
            return View("Index");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get nv id first
            int obj_id = TextLibrary.ToInt(Request["loainhanvien_id"]);
            LoaiNhanVienController ctr = new LoaiNhanVienController();
            LoaiNhanVien obj;
            //check mode
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                if (!this._nhanvien_permission.Contains("loainhanvien_add"))
                {
                    return _fail_permission("loainhanvien_add");
                }

                obj = new LoaiNhanVien();
                edit_mode = false;
            }
            else
            {
                //edit mode
                if (!this._nhanvien_permission.Contains("loainhanvien_edit"))
                {
                    return _fail_permission("loainhanvien_edit");
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
                    return RedirectToAction("Index", "AdminLoaiNhanViens");
                }
            }
            //assign value
                obj.ten = TextLibrary.ToString(Request["loainhanvien_ten"]);
            //get quyen han
                List<int> quyen_id_list = TextLibrary.ToListInt(Request["loainhanvien_checkbox[]"]);//1,4,6,23
                Debug.WriteLine("Quyen han: " + Request["loainhanvien_checkbox[]"]);
            //validate properties
                List<String> validate = ctr.validate(obj);
            //action
            if (validate.Count == 0)
            {
                if (edit_mode)
                {
                    //update properties first
                    ctr._db.SaveChanges();
                    ctr.gan_quyen_han(obj.id, quyen_id_list);
                    this._state.Add("edit_ok");
                }
                else
                {
                    //call add
                    int maxid = ctr.add(obj);
                    //re assign id
                    obj.id = maxid;
                    //set quyen han
                    ctr.gan_quyen_han(obj.id, quyen_id_list);
                    this._state.Add("add_ok");
                }
                //successfull redirect
                return RedirectToAction("Index", "AdminLoaiNhanViens");
            }
            //fail redirect
            this._state.AddRange(validate);
            ViewBag.State = this._state;
            ViewBag.LoaiNhanVien = obj;
            ViewBag.Title += " - Submit";
            return View("Index");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Nhóm quyền hạn";
            //đặt activetab cố định cho controller này
            this._current_activetab();
        }
        private void _current_activetab()
        {
            this._set_activetab(new String[] { "LoaiNhanVien", "QuanTriHeThong" });
        }
    }
}
