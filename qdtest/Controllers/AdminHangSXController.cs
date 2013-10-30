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
    public class AdminHangSXController : AdminController
    {
        public ActionResult Index(int id = 0)
        {
            if (!this._permission.Contains("hangsx_view"))
            {
                return _fail_permission("hangsx_view");
            }
            HangSXController ctr = new HangSXController();
            HangSX obj = ctr.get_by_id(id);

            if (obj == null)
            {
                //kich thuoc khong ton tai
                return RedirectToAction("Index", "AdminHangSXs");
            }
            ViewBag.HangSX = obj;
            ViewBag.Title += " - View";
            return View();
        }
        public ActionResult Add()
        {
            if (!this._permission.Contains("hangsx_add"))
            {
                return _fail_permission("hangsx_add");
            }
            HangSXController ctr = new HangSXController();
            HangSX obj = new HangSX();
            ViewBag.HangSX = obj;
            ViewBag.Title += " - Add";
            return View("Index");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get nv id first
            int obj_id = TextLibrary.ToInt(Request["hangsx_id"]);
            HangSXController ctr = new HangSXController();
            HangSX obj;
            //check mode
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                if (!this._permission.Contains("hangsx_add"))
                {
                    return _fail_permission("hangsx_add");
                }

                obj = new HangSX();
                edit_mode = false;
            }
            else
            {
                //edit mode
                if (!this._permission.Contains("hangsx_edit"))
                {
                    return _fail_permission("hangsx_edit");
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
                    return RedirectToAction("Index", "AdminHangSXs");
                }
            }
            //assign value
            obj.ten = TextLibrary.ToString(Request["hangsx_ten"]);
            obj.active = TextLibrary.ToBoolean(Request["hangsx_active"]);
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
                return RedirectToAction("Index", "AdminHangSXs");
            }
            //fail redirect
            this._state.AddRange(validate);
            ViewBag.State = this._state;
            ViewBag.HangSX = obj;
            ViewBag.Title += " - Submit";
            return View("Index");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Hãng sản xuất";
            //đặt activetab cố định cho controller này
            this._current_activetab();
        }
        private void _current_activetab()
        {
            this._set_activetab(new String[] { "HangSX", "Catalog" });
        }
    }
}
