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
    public class AdminKhachHangController : AdminController
    {
        public ActionResult Index(int id = 0)
        {
            if (!this._nhanvien_permission.Contains("khachhang_view"))
            {
                return _fail_permission("khachhang_view");
            }
            //get controller
            KhachHangController ctr = new KhachHangController();
            LoaiKhachHangController ctr_lkh = new LoaiKhachHangController();
            //kiểm tra tồn tại
                if (!ctr.is_exist(id)) return RedirectToAction("Index", "AdminKhachHangs");
            //get info
                ViewBag.KhachHang = ctr.get_by_id(id);
                ViewBag.Title += " - Xem chi tiết";
                ViewBag.LoaiKhachHang_List = ctr_lkh.timkiem();
                return View();
        }
        public ActionResult Add()
        {
            //check
            if (!this._nhanvien_permission.Contains("khachhang_add"))
            {
                return this._fail_permission("khachhang_add");
            }
            LoaiKhachHangController ctr_lkh = new LoaiKhachHangController();
            KhachHang obj = new KhachHang();
            //nếu chưa có loại kh nào thì báo lỗi k thể thêm
            if (ctr_lkh.timkiem("", "", "1").Count == 0)
            {
                return _show_notification("Yêu cầu phải có ít nhất 1 loại khách hàng active mới có thể thêm mới khách hàng");
            }
            ViewBag.KhachHang = obj;
            ViewBag.Title += " - Thêm mới";
            ViewBag.LoaiKhachHang_List = ctr_lkh.timkiem();
            ViewBag.add_mode = "1";
            return View("Index");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get obj id first
            int obj_id = TextLibrary.ToInt(Request["khachhang_id"]);
            KhachHangController ctr = new KhachHangController();
            LoaiKhachHangController ctr_lkh = new LoaiKhachHangController(ctr._db);
            //khoi tao moi doi tuong
            KhachHang obj;
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                //check permission
                if (!this._nhanvien_permission.Contains("khachhang_add"))
                {
                    return this._fail_permission("khachhang_add");
                }
                obj = new KhachHang();
                edit_mode = false;
            }
            else
            {
                //update mode
                //check permission
                if (!this._nhanvien_permission.Contains("khachhang_edit"))
                {
                    return _fail_permission("khachhang_edit");
                }
                //kiem tra ton tai
                if (ctr.is_exist(obj_id))
                {
                    //get obj
                    obj = ctr.get_by_id(obj_id);
                }
                else
                {
                    //id khong ton tai
                    return RedirectToAction("Index", "AdminKhachHangs");
                }
            }
            //assign data
            List<string> validate = new List<string>();
            string matkhau = TextLibrary.ToString(Request["khachhang_matkhau"]);
            string matkhau2 = TextLibrary.ToString(Request["khachhang_matkhau2"]);
            obj.email = TextLibrary.ToString(Request["khachhang_email"]);
            obj.sdt = TextLibrary.ToString(Request["khachhang_sdt"]);
            obj.diachi = TextLibrary.ToString(Request["khachhang_diachi"]);
            obj.tendangnhap =  TextLibrary.ToString(Request["khachhang_tendangnhap"]);
            obj.tendaydu =  TextLibrary.ToString(Request["khachhang_tendaydu"]);
            obj.bad = TextLibrary.ToBoolean(Request["khachhang_bad"]);
            obj.active = TextLibrary.ToBoolean(Request["khachhang_active"]);
            //validate
            validate.AddRange(ctr.validate(obj,matkhau,matkhau2));
            //action
            if (validate.Count==0)
            {
                if (edit_mode)
                {
                    //call update for properties
                    ctr._db.SaveChanges();
                    //call set password
                    ctr.set_password(obj.id,matkhau2);
                    validate.Add("edit_ok");
                }
                else
                {
                    //add mode mới có chuyện set loại KH
                        obj.loaikhachhang = ctr_lkh.get_by_id(
                        TextLibrary.ToInt(Request["khachhang_loaikhachhang_id"])
                        );
                        obj.diem = obj.loaikhachhang.mucdiem;
                    //call update loaikh first
                    obj._Update_LoaiKhachHang(ctr._db);
                    //set raw password
                    obj.matkhau = matkhau2;
                    //call add
                    int maxid = ctr.add(obj);
                    //re assign id
                    obj.id = maxid;
                    validate.Add("add_ok");
                }
            }
            obj.loaikhachhang = ctr_lkh.get_by_id(
                TextLibrary.ToInt(Request["khachhang_loaikhachhang_id"])
            );
            ViewBag.KhachHang = obj;
            ViewBag.Title += " - Submit";
            ViewBag.State = validate;
            ViewBag.LoaiKhachHang_List = ctr_lkh.timkiem();
            ViewBag.add_mode = obj_id==0?"1":"0";
            return View("Index");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Khách hàng";
            //đặt activetab cố định cho controller này
            this._current_activetab();
        }
        private void _current_activetab()
        {
            this._set_activetab(new String[] { "QuanLyKhachHang" });
        }

    }
}
