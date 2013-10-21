using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminKichThuocController : AdminController
    {
        public ActionResult Index(int id = 0)
        {
            if (!this._permission.Contains("kichthuoc_view"))
            {
                return _fail_permission("kichthuoc_view");
            }
            KichThuocController ctr = new KichThuocController();
            KichThuoc obj = ctr.get_by_id(id);

            if (obj == null)
            {
                //kich thuoc khong ton tai
                return RedirectToAction("Index", "AdminKichThuocs");
            }
            ViewBag.KichThuoc = obj;
            ViewBag.Title += " - View";
            return View();
        }
        public ActionResult Add()
        {
            if (!this._permission.Contains("kichthuoc_add"))
            {
                return _fail_permission("kichthuoc_add");
            }
            KichThuocController ctr = new KichThuocController();
            KichThuoc obj = new KichThuoc();
            ViewBag.KichThuoc = obj;
            ViewBag.Title += " - Add";
            return View("Index");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get nv id first
            int obj_id = TextLibrary.ToInt(Request["kichthuoc_id"]);
            KichThuocController ctr = new KichThuocController();
            KichThuoc obj;
            //check mode
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                if (!this._permission.Contains("kichthuoc_add"))
                {
                    return _fail_permission("kichthuoc_add");
                }

                obj = new KichThuoc();
                edit_mode = false;
            }
            else
            {
                //edit mode
                if (!this._permission.Contains("kichthuoc_edit"))
                {
                    return _fail_permission("kichthuoc_edit");
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
                    return RedirectToAction("Index", "AdminKichThuocs");
                }
            }
            //assign value
            obj.giatri = TextLibrary.ToString(Request["kichthuoc_giatri"]);
            obj.mota = TextLibrary.ToString(Request["kichthuoc_mota"]);
            obj.active = TextLibrary.ToBoolean(Request["kichthuoc_active"]);
            //validate properties
            List<String> validate = ctr.validate(obj);

            //action
            if (validate.Count == 0)
            {
                if (edit_mode)
                {
                    //update properties first
                    ctr._db.SaveChanges();
                    this._state.Add("edit_ok");
                }
                else
                {
                    //call add
                    int maxid = ctr.add(obj);
                    //re assign id
                    obj.id = maxid;
                    this._state.Add("add_ok");
                }
                //successfull redirect
                return RedirectToAction("Index", "AdminKichThuocs");
            }
            //fail redirect
            this._state.AddRange(validate);
            ViewBag.State = this._state;
            ViewBag.KichThuoc = obj;
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
            this._set_activetab(new String[] { "KichThuoc", "Catalog" });
        }
    }
}
