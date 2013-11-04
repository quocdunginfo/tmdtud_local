using CuaHangBanGiay.Controllers;
using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminSanPhamController : AdminController
    {
        /*
        public ActionResult Index(int id = 0, int chitietsp_id=0)
        {
            if (!this._permission.Contains("sanpham_view"))
            {
                return _fail_permission("sanpham_view");
            }
            SanPhamController ctr = new SanPhamController();
            SanPham obj = ctr.get_by_id(id);

            if (obj == null)
            {
                //khong ton tai
                return RedirectToAction("Index", "AdminSanPhams");
            }
            ViewBag.SanPham = obj;
            //active only
                NhomSanPhamController ctr_nhom = new NhomSanPhamController();
                ViewBag.NhomSanpham2_ListAll = ctr_nhom.timkiem("", "", "", "1");
                //active only
                HangSXController ctr_hangsx = new HangSXController();
                ViewBag.HangSX_ListAll = ctr_hangsx.timkiem("", "", "1");
                //
                KichThuocController ctr_kichthuoc = new KichThuocController();
                ViewBag.KichThuoc_ListAll = ctr_kichthuoc.timkiem("", "", "", "1");
                //
                MauSacController ctr_mausac = new MauSacController();
                ViewBag.MauSac_ListAll = ctr_mausac.timkiem("", "", "", "1");
            ViewBag.Title += " - View";

            ChiTietSPController ctr_chitietsp = new ChiTietSPController(ctr._db);
            ChiTietSP ctsp = ctr_chitietsp.get_by_id(chitietsp_id);
            ViewBag.ChiTietSP = ctsp == null ? new ChiTietSP() : ctsp;
            
            
            return View();
        }
         **/
        /*
        public ActionResult Add()
        {
            if (!this._permission.Contains("sanpham_add"))
            {
                return _fail_permission("sanpham_add");
            }
            SanPhamController ctr = new SanPhamController();
            SanPham obj = new SanPham();
            //add new blank record
            int id = ctr.add(obj);
            return RedirectToAction("Index", "AdminSanPham", new {id=id });
        }
        */
        //WORK OK
        public ActionResult Index(int id = 0, int chitietsp_id = 0)
        {
            if (!this._permission.Contains("sanpham_view"))
            {
                return _fail_permission("sanpham_view");
            }
            SanPhamController ctr = new SanPhamController();
            SanPham obj = ctr.get_by_id(id);
            Boolean un_save = false;
            if (obj == null)//tuwsc laf id=0, ddang owr chees dooj SANPHAM chuwa luuw
            {
                //lay tu session ra
                if (Session != null && Session["sanpham_new_tmp"] != null)
                {
                    obj = (SanPham)Session["sanpham_new_tmp"];
                }
                else
                {
                    //Chưa gọi set session trước 
                    return RedirectToAction("Index", "AdminSanPhams");
                }
                un_save = true;
            }
            else
            {
                //một khi đã hiển thi sản phẩm đã được save thì xóa ngay và luôn session
                if (Session != null && Session["sanpham_new_tmp"] != null)
                {
                    Session["sanpham_new_tmp"] = null;
                }
            }
            ViewBag.SanPham = obj;
            //active only
            NhomSanPhamController ctr_nhom = new NhomSanPhamController();
            ViewBag.NhomSanpham2_ListAll = ctr_nhom.timkiem("", "", "", "1");
            //active only
            HangSXController ctr_hangsx = new HangSXController();
            ViewBag.HangSX_ListAll = ctr_hangsx.timkiem("", "", "1");
            //
            KichThuocController ctr_kichthuoc = new KichThuocController();
            ViewBag.KichThuoc_ListAll = ctr_kichthuoc.timkiem("", "", "", "1");
            //
            MauSacController ctr_mausac = new MauSacController();
            ViewBag.MauSac_ListAll = ctr_mausac.timkiem("", "", "", "1");
            ViewBag.Title += " - View";

            ChiTietSPController ctr_chitietsp = new ChiTietSPController(ctr._db);
            ChiTietSP ctsp;
            if (un_save)
            {
                if (obj.ds_chitietsp.Where(x => x.id == chitietsp_id).FirstOrDefault()!=null)
                {
                    //lay tư trong session ra
                    ctsp = obj.ds_chitietsp.Where(x => x.id == chitietsp_id).FirstOrDefault();
                }
                else
                {
                    ctsp = new ChiTietSP();
                }
            }
            else
            {
                ctsp = ctr_chitietsp.get_by_id(chitietsp_id);
            }
            ViewBag.ChiTietSP = ctsp == null ? new ChiTietSP() : ctsp;

            return View();
        }
        //WORK OK
        public ActionResult Add()
        {
            if (!this._permission.Contains("sanpham_add"))
            {
                return _fail_permission("sanpham_add");
            }
            SanPhamController ctr = new SanPhamController();
            SanPham obj = new SanPham();
            //Save obj to session
            Session["sanpham_new_tmp"] = obj;
            Session["sanpham_new_dbcontext"] = ctr._db;
            return RedirectToAction("Index", "AdminSanPham", new { id = 0 });
        }
        /*
        [HttpPost]
        public ActionResult Submit()
        {
            //get obj id first
            int obj_id = TextLibrary.ToInt(Request["sanpham_id"]);
            SanPhamController ctr = new SanPhamController();
            SanPham obj;
            //check mode
            Boolean edit_mode = true;
            if (obj_id == 0)
            {
                //add mode
                if (!this._permission.Contains("sanpham_add"))
                {
                    return _fail_permission("sanpham_add");
                }

                obj = new SanPham();
                edit_mode = false;
            }
            else
            {
                //edit mode
                if (!this._permission.Contains("sanpham_edit"))
                {
                    return _fail_permission("sanpham_edit");
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
                    return RedirectToAction("Index", "AdminSanPhams");
                }
            }
            //assign value
                obj.active = TextLibrary.ToBoolean(Request["sanpham_active"]);
                obj.gia = TextLibrary.ToInt(Request["sanpham_gia"]);
                obj.masp = TextLibrary.ToString(Request["sanpham_masp"]);
                obj.mota = TextLibrary.ToString(Request["sanpham_mota"]);
                obj.ten = TextLibrary.ToString(Request["sanpham_ten"]);
                //get external object 
                HangSXController ctr_hangsx = new HangSXController(ctr._db);
                obj.hangsx = ctr_hangsx.get_by_id(TextLibrary.ToInt(Request["sanpham_hangsx_id"]));

                NhomSanPhamController ctr_nhomsanpham = new NhomSanPhamController(ctr._db);
                obj.nhomsanpham = ctr_nhomsanpham.get_by_id(TextLibrary.ToInt(Request["sanpham_nhomsanpham_id"]));
                
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
                //get search value
                if (!TextLibrary.ToString(Request["sanpham_upload_hinhanh"]).Equals(""))
                {
                    HinhAnhController ctr_hinhanh = new HinhAnhController(ctr._db);
                    List<HinhAnh> hinhanh_list = ctr_hinhanh.upload(Server,Request.Files);
                    obj.ds_hinhanh.AddRange(hinhanh_list);
                    ctr._db.SaveChanges();
                }
                //successfull redirect
                return RedirectToAction("Index", "AdminSanPham", new { id=obj.id});
            }
            //fail redirect
            this._state.AddRange(validate);
            ViewBag.State = this._state;
            ViewBag.SanPham = obj;
            ViewBag.Title += " - Submit";
            return View("Index");
        } 
         */
        [HttpPost]
        public ActionResult Submit()
        {
            //get obj id first
            int obj_id = TextLibrary.ToInt(Request["sanpham_id"]);
            //must use same dbcontext neu khong se bao loi (chi can thiet khi co cap nhat thao tac tren doi tuong sanpham trong session)
            SanPhamController ctr = new SanPhamController(this._get_current_dbcontext());

            SanPham obj = (SanPham)Session["sanpham_new_tmp"];
            Boolean un_save = false;
            if (obj != null)
            {
                //un save mode
                un_save = true;
                //add mode
                if (!this._permission.Contains("sanpham_add"))
                {
                    return _fail_permission("sanpham_add");
                }
            }
            else
            {
                //get obj from csdl
                obj = ctr.get_by_id(obj_id);
                //edit mode
                if (!this._permission.Contains("sanpham_edit"))
                {
                    return _fail_permission("sanpham_edit");
                }
                if (obj == null)
                {
                    return RedirectToAction("Index", "AdminSanPhams");
                }                
            }
            
            //assign value
                obj.active = TextLibrary.ToBoolean(Request["sanpham_active"]);
                obj.gia = TextLibrary.ToInt(Request["sanpham_gia"]);
                obj.masp = TextLibrary.ToString(Request["sanpham_masp"]);
                obj.mota = TextLibrary.ToString(Request["sanpham_mota"]);
                obj.ten = TextLibrary.ToString(Request["sanpham_ten"]);
                //get external object 
                HangSXController ctr_hangsx = new HangSXController(ctr._db);
                obj.hangsx = ctr_hangsx.get_by_id(TextLibrary.ToInt(Request["sanpham_hangsx_id"]));

                NhomSanPhamController ctr_nhomsanpham = new NhomSanPhamController(ctr._db);
                obj.nhomsanpham = ctr_nhomsanpham.get_by_id(TextLibrary.ToInt(Request["sanpham_nhomsanpham_id"]));
                
            //validate properties
            List<String> validate = ctr.validate(obj);

            //action
            if (validate.Count == 0)
            {
                if (!un_save)
                {
                    //update properties first
                    ctr._db.SaveChanges();
                    this._state.Add("edit_ok");
                    //get image
                    if (!TextLibrary.ToString(Request["sanpham_upload_hinhanh"]).Equals(""))
                    {
                        HinhAnhController ctr_hinhanh = new HinhAnhController(ctr._db);
                        List<HinhAnh> hinhanh_list = ctr_hinhanh.upload(Server, Request.Files);
                        obj.ds_hinhanh.AddRange(hinhanh_list);
                        ctr._db.SaveChanges();
                    }

                    //successfull redirect
                    return RedirectToAction("Index", "AdminSanPham", new { id = obj.id });
                }
                else
                {
                    //người dùng bấm nút upload hình ảnh chứ không phải nút lưu
                    if (!TextLibrary.ToString(Request["sanpham_upload_hinhanh"]).Equals(""))
                    {
                        //khoan hãy lưu vào csdl
                        HinhAnhController ctr_hinhanh = new HinhAnhController(ctr._db);
                        List<HinhAnh> hinhanh_list = ctr_hinhanh.upload(Server, Request.Files);
                        obj.ds_hinhanh.AddRange(hinhanh_list);
                        //successfull redirect
                        return RedirectToAction("Index", "AdminSanPham", new { id = 0 });
                    }
                    else
                    {
                        //call add
                        int maxid = ctr.add(obj);
                        //re assign id
                        obj.id = maxid;
                        this._state.Add("add_ok");
                        //successfull redirect
                        return RedirectToAction("Index", "AdminSanPham", new { id = obj.id });
                    }
                    
                }
            }
            //fail or redirect
            this._state.AddRange(validate);
            ViewBag.State = this._state;
            ViewBag.SanPham = obj;
            ViewBag.Title += " - Submit";
            return View("Index");
        }
        /*
        [HttpGet]
        public ActionResult ChiTietSP_Add(int for_sanpham_id=0)
        {
            return RedirectToAction("Index", "AdminSanPham", new { id = for_sanpham_id });
        }
        */
        //WORK OK
        [HttpGet]
        public ActionResult ChiTietSP_Add(int for_sanpham_id = 0)
        {
            if (!this._permission.Contains("chitietsp_add"))
            {
                return _fail_permission("chitietsp_add");
            }
            //redirect
            return RedirectToAction("Index", "AdminSanPham", new { id = for_sanpham_id });
        }
        /*
        [HttpGet]
        public ActionResult ChiTietSP_Edit(int id)
        {
            if (!this._permission.Contains("chitietsp_edit"))
            {
                return _fail_permission("chitietsp_edit");
            }
            ChiTietSPController ctr_chitietsp = new ChiTietSPController();
            ChiTietSP ctsp = ctr_chitietsp.get_by_id(id);
            if (ctsp == null)
            {
                return RedirectToAction("Index", "AdminSanPhams");
            }
            return RedirectToAction("Index", "AdminSanPham", new { id = ctsp.sanpham.id, chitietsp_id = ctsp.id });
        }
         * */
        //WORK OK
        [HttpGet]
        public ActionResult ChiTietSP_Edit(int id)
        {
            if (!this._permission.Contains("chitietsp_edit"))
            {
                return _fail_permission("chitietsp_edit");
            }
            SanPhamController ctr = new SanPhamController();
            ChiTietSPController ctr_chitietsp = new ChiTietSPController();
            ChiTietSP ctsp = new ChiTietSP();
            SanPham sanpham =(SanPham) Session["sanpham_new_tmp"];
            if (sanpham != null)
            {
                //vẫn còn đang ở un_save mode thì lấy từ session ra
                ctsp = sanpham.ds_chitietsp.Where(x => x.id == id).FirstOrDefault();
                return RedirectToAction("Index", "AdminSanPham", new { id = 0, chitietsp_id = ctsp.id });
            }
            //đã save rồi
            else
            {
                ctsp = ctr_chitietsp.get_by_id(id);
                sanpham = ctr.get_by_id(ctsp.sanpham.id);
                if (ctsp == null)
                {
                    return RedirectToAction("Index", "AdminSanPhams");
                }
                return RedirectToAction("Index", "AdminSanPham", new { id = ctsp.sanpham.id, chitietsp_id = ctsp.id });
            }
        }
        /*
        [HttpPost]
        public ActionResult ChiTietSP_Submit()
        {
            int sanpham_id = TextLibrary.ToInt(Request["sanpham_id"]);
            SanPhamController ctr = new SanPhamController();
            ChiTietSPController ctr_chitietsp = new ChiTietSPController(ctr._db);
            KichThuocController ctr_kichthuoc = new KichThuocController(ctr._db);
            MauSacController ctr_mausac = new MauSacController(ctr._db);
            SanPham sanpham_obj = ctr.get_by_id(sanpham_id);
            Boolean un_save_mode = false;
            //neu sanpham_id=0, dang o mode chua save
            if (sanpham_obj == null)
            {
                sanpham_obj = (SanPham)Session["sanpham_new_tmp"];
                un_save_mode = true;
            }
            //create new
            ChiTietSP obj = new ChiTietSP();
            //nếu là edit mode
            int chitietsp_id = TextLibrary.ToInt(Request["chitietsp_id"]);
            Boolean edit_mode = ctr_chitietsp.get_by_id(chitietsp_id) != null;
            if (edit_mode)
            {
                if (!this._permission.Contains("chitietsp_edit"))
                {
                    return _fail_permission("chitietsp_edit");
                }
                obj = ctr_chitietsp.get_by_id(chitietsp_id);
            }
            else 
            {
                if (!this._permission.Contains("chitietsp_add"))
                {
                    return _fail_permission("chitietsp_add");
                }
                //kiểm tra phải có ít nhất 1 kích thước
                    if (ctr_kichthuoc.timkiem_count("", "", "", "1") <= 0)
                    {
                        return _show_notification("Yêu cầu phải có ít nhất 1 kích thước active mới thêm được chi tiết sản phẩm");
                    }
                //kiểm tra phải có ít nhất 1 màu sắc
                    if (ctr_mausac.timkiem_count("", "", "", "1") <= 0)
                    {
                        return _show_notification("Yêu cầu phải có ít nhất 1 màu sắc active mới thêm được chi tiết sản phẩm");
                }
            }
            
            
            //assign data
                obj.soluong = TextLibrary.ToInt(Request["chitietsp_soluong"]);
                //ctr_kichthuoc = new KichThuocController(ctr._db);
                obj.kichthuoc = ctr_kichthuoc.get_by_id(TextLibrary.ToInt(Request["chitietsp_kichthuoc_id"]));

                //ctr_mausac = new MauSacController(ctr._db);
                obj.mausac = ctr_mausac.get_by_id(TextLibrary.ToInt(Request["chitietsp_mausac_id"]));

                obj.active = TextLibrary.ToBoolean(Request["chitietsp_active"]);
            if(edit_mode)
            {
                //thing
            }
            else
            {
                //add to sanpham
                sanpham_obj.ds_chitietsp.Add(obj);
            }

            //finally call update
            ctr._db.SaveChanges();
            return RedirectToAction("Index", "AdminSanPham", new { id = sanpham_id});
        }
         */
        //WORK OK
        [HttpPost]
        public ActionResult ChiTietSP_Submit()
        {
            int sanpham_id = TextLibrary.ToInt(Request["sanpham_id"]);
            //must use same dbcontext neu khong se bao loi (chi can thiet khi co cap nhat thao tac tren doi tuong sanpham trong session)
            SanPhamController ctr = new SanPhamController(this._get_current_dbcontext());

            ChiTietSPController ctr_chitietsp = new ChiTietSPController(ctr._db);
            KichThuocController ctr_kichthuoc = new KichThuocController(ctr._db);
            MauSacController ctr_mausac = new MauSacController(ctr._db);
            SanPham sanpham_obj = ctr.get_by_id(sanpham_id);
            Boolean un_save_mode = false;
            //neu sanpham_id=0, dang o mode chua save
            if (sanpham_obj == null)
            {
                sanpham_obj = (SanPham)Session["sanpham_new_tmp"];
                un_save_mode = true;
            }
            //create new
            ChiTietSP obj;
            int chitietsp_id = TextLibrary.ToInt(Request["chitietsp_id"]);
            Boolean edit_mode;
            if (un_save_mode)
            {
                edit_mode  = sanpham_obj.ds_chitietsp.Where(x=>x.id==chitietsp_id).FirstOrDefault()!=null?true:false;
                if (edit_mode)
                {
                    if (!this._permission.Contains("chitietsp_edit"))
                    {
                        return _fail_permission("chitietsp_edit");
                    }
                    obj = sanpham_obj.ds_chitietsp.Where(x => x.id==chitietsp_id).FirstOrDefault();
                }
                else
                {
                    if (!this._permission.Contains("chitietsp_add"))
                    {
                        return _fail_permission("chitietsp_add");
                    }
                    obj = new ChiTietSP();
                    //kiểm tra phải có ít nhất 1 kích thước
                    if (ctr_kichthuoc.timkiem_count("", "", "", "1") <= 0)
                    {
                        return _show_notification("Yêu cầu phải có ít nhất 1 kích thước active mới thêm được chi tiết sản phẩm");
                    }
                    //kiểm tra phải có ít nhất 1 màu sắc
                    if (ctr_mausac.timkiem_count("", "", "", "1") <= 0)
                    {
                        return _show_notification("Yêu cầu phải có ít nhất 1 màu sắc active mới thêm được chi tiết sản phẩm");
                    }
                }
            }
            //chế dộ đã save rồi
            else
            {
                edit_mode = ctr_chitietsp.get_by_id(chitietsp_id) != null;
                if (edit_mode)
                {
                    if (!this._permission.Contains("chitietsp_edit"))
                    {
                        return _fail_permission("chitietsp_edit");
                    }
                    obj = ctr_chitietsp.get_by_id(chitietsp_id);
                }
                else
                {
                    if (!this._permission.Contains("chitietsp_add"))
                    {
                        return _fail_permission("chitietsp_add");
                    }
                    obj = new ChiTietSP();
                    //kiểm tra phải có ít nhất 1 kích thước
                    if (ctr_kichthuoc.timkiem_count("", "", "", "1") <= 0)
                    {
                        return _show_notification("Yêu cầu phải có ít nhất 1 kích thước active mới thêm được chi tiết sản phẩm");
                    }
                    //kiểm tra phải có ít nhất 1 màu sắc
                    if (ctr_mausac.timkiem_count("", "", "", "1") <= 0)
                    {
                        return _show_notification("Yêu cầu phải có ít nhất 1 màu sắc active mới thêm được chi tiết sản phẩm");
                    }
                }
            }

            
            //assign data
                obj.soluong = TextLibrary.ToInt(Request["chitietsp_soluong"]);
                //ctr_kichthuoc = new KichThuocController(ctr._db);
                obj.kichthuoc = ctr_kichthuoc.get_by_id(TextLibrary.ToInt(Request["chitietsp_kichthuoc_id"]));

                //ctr_mausac = new MauSacController(ctr._db);
                obj.mausac = ctr_mausac.get_by_id(TextLibrary.ToInt(Request["chitietsp_mausac_id"]));

                obj.active = TextLibrary.ToBoolean(Request["chitietsp_active"]);
            if (un_save_mode)
            {
                if (edit_mode)
                {
                    //do nothing
                }
                else
                {
                    //add to sanpham tmp
                    //must set id
                    try
                    {
                        obj.id = sanpham_obj.ds_chitietsp.Max(x => x.id) + 1;
                    }
                    catch (Exception ex)
                    {
                        obj.id = 1;
                    }
                    sanpham_obj.ds_chitietsp.Add(obj);
                    //re assign to session
                    Session["sanpham_new_tmp"] = sanpham_obj;
                }
            }
            //chế độ đã save rồi
            else
            {
                if (edit_mode)
                {
                    //nothing
                }
                else
                {
                    sanpham_obj.ds_chitietsp.Add(obj);
                }
                //finally call update
                ctr._db.SaveChanges();
            }
            return RedirectToAction("Index", "AdminSanPham", new { id = sanpham_id});
        }
        [HttpGet]
        public ActionResult HinhAnh_SetDefault(int for_sanpham_id, int hinhanh_id)
        {
            if (!this._permission.Contains("hinhanh_edit"))
            {
                return _fail_permission("hinhanh_edit");
            }
            HinhAnhController ctr = new HinhAnhController();
            ctr.set_default(hinhanh_id);
            return RedirectToAction("Index", "AdminSanPham", new { id=for_sanpham_id });
        }
        [HttpGet]
        public ActionResult HinhAnh_Delete(int for_sanpham_id, int hinhanh_id)
        {
            if (!this._permission.Contains("chitietsp_delete"))
            {
                return _fail_permission("chitietsp_delete");
            }
            HinhAnhController ctr = new HinhAnhController();
            ctr.delete(hinhanh_id, Server);
            //final action
            return RedirectToAction("Index", "AdminSanPham", new { id = for_sanpham_id });
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Sản phẩm";
            //đặt activetab cố định cho controller này
            this._current_activetab();
        }
        private void _current_activetab()
        {
            this._set_activetab(new String[] { "SanPham", "Catalog" });
        }
        private BanGiayDBContext _get_current_dbcontext()
        {
            if (Session != null && Session["sanpham_new_dbcontext"]!=null)
            {
                return (BanGiayDBContext)Session["sanpham_new_dbcontext"];
            }
            else
            {
                return new BanGiayDBContext();
            }
        }
        private void _add_state_tempdata(String value)
        {
            if(TempData["state_tempdata"]==null)
            {

            }
            //List<String> 
        }
    }
}
