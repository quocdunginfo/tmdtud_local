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
    public class FrontCartController : FrontController
    {
        [HttpGet]
        public ActionResult Index()
        {
            DonHangController ctr = new DonHangController();
            //
            ViewBag.giohang = this._giohang;
            List<string> validate = ctr.validate(this._giohang, out this._giohang);
            validate.AddRange( TempData["state"] == null ? new List<string>() : (List<string>)TempData["state"] );
            ViewBag.State = validate;
            return View();
        }
        [HttpGet]
        public ActionResult Confirm()
        {
            //kiểm tra đơn hàng lần nữa xem có bị lỗi hết hàng
            //kiểm tra thông tin người nhận lần nữa để đảm bảo
            //hiện thông tin đơn hàng chuẩn bị lưu vào hệ thống
            //kết thúc chu trình
            return View();
        }
        [HttpPost]
        public ActionResult CheckOut_Submit()
        {
            DonHangController ctr = new DonHangController();
            TinhTPController ctr_tinhtp = new TinhTPController();
            List<string> validate= new List<string>();
            validate.AddRange(ctr.validate(this._giohang, out this._giohang));
            //giỏ hàng chưa sẵn sàng để qua bước này
            if(validate.Count>0)
            {
                return RedirectToAction("Index","FrontCart");
            }
            //get post value
            string nguoinhan_ten = TextLibrary.ToString(Request["nguoinhan_ten"]);
            string nguoinhan_sdt = TextLibrary.ToString(Request["nguoinhan_sdt"]);
            string nguoinhan_diachi= TextLibrary.ToString(Request["nguoinhan_diachi"]);
            Boolean thanhtoan_tructuyen = TextLibrary.ToBoolean(Request["thanhtoan_tructuyen"]);
            int tinhtp_id = TextLibrary.ToInt(Request["nguoinhan_tinhtp_id"]);
            //assign
            this._giohang.nguoinhan_ten = nguoinhan_ten;
            this._giohang.nguoinhan_diachi = nguoinhan_diachi;
            this._giohang.nguoinhan_sdt = nguoinhan_sdt;
            this._giohang.thanhtoan_tructuyen = thanhtoan_tructuyen;
            this._giohang.nguoinhan_diachi_tinhtp = ctr_tinhtp.get_by_id(tinhtp_id);
            //validate
            validate.AddRange(ctr.validate_checkout(this._giohang, out this._giohang));
            if (validate.Count <= 0)
            {
                //OK
                //nếu thanh toán trực tuyến
                if (this._giohang.thanhtoan_tructuyen)
                {
                    return RedirectToAction("Online_Payment", "FrontCart");
                }
                //chuyển đến trang xác nhận đơn hàng lần cuối
                return RedirectToAction("Confirm", "FrontCart");
            }
            //nếu vẫn chưa thì hiển thị lỗi và thông tin ngược lại
            ViewBag.State = validate;
            ViewBag.giohang = this._giohang;
            ViewBag.tinhtp_list = ctr_tinhtp.get_all();
            return View("CheckOut");
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            DonHangController ctr = new DonHangController();
            TinhTPController ctr_tinhtp = new TinhTPController();
            List<string> validate = ctr.validate(this._giohang, out this._giohang);
            //kiểm tra giỏ hàng
            if (validate.Count > 0)
            {
                //giỏ hàng chưa thể thanh toán
                return RedirectToAction("Index","FrontCart");
            }
            //nếu chưa đang nhập thì chuyển đến trang đăng ký
            if (!this._is_logged_in())
            {
                return RedirectToAction("Login_Or_Register", "FrontCart");
            }
            //nếu đã đăng nhập thì chuyển đến giao diện nhập thông tin người nhận
            //tự động pass thông tin người đang đăng nhập vào thông tin người nhận
                this._giohang.nguoinhan_diachi_tinhtp = ctr_tinhtp.get_all().FirstOrDefault();//mặc định lấy tỉnh đầu
                this._giohang.thanhtoan_tructuyen = false;
                if (this._nhanvien != null)
                {
                    this._giohang.nguoinhan_diachi = "";
                    this._giohang.nguoinhan_sdt = "";
                    this._giohang.nguoinhan_ten = this._nhanvien.tendaydu;
                }
                else if (this._khachhang != null)
                {
                    this._giohang.nguoinhan_diachi = this._khachhang.diachi;
                    this._giohang.nguoinhan_sdt = this._khachhang.sdt;
                    this._giohang.nguoinhan_ten = this._khachhang.tendaydu;
                }
            //kiểm tra thông tin người nhận
            validate.AddRange(ctr.validate_checkout(this._giohang,out this._giohang));

            ViewBag.giohang = this._giohang;
            ViewBag.tinhtp_list = ctr_tinhtp.get_all();
            ViewBag.State = validate;
            return View();
        }
        [HttpGet]
        public ActionResult Remove(int chitietsp_id)
        {
            ChiTiet_DonHang obj= this._giohang.ds_chitiet_donhang.Where(x => x.chitietsp.id == chitietsp_id).FirstOrDefault();
            if (obj != null)
            {
                this._giohang.ds_chitiet_donhang.Remove(obj);
                this._save_cart_to_session();
            }
            return RedirectToAction("Index", "FrontCart");
        }
        [HttpPost]
        public ActionResult Add_Or_Update()
        {
            //get obj id
            int ctsp_id = TextLibrary.ToInt(Request["giohang_chitietsp_id"]);
            //get soluong
            int soluong = TextLibrary.ToInt(Request["giohang_soluong"]);
            //update from cart
            Boolean update_from_cart = TextLibrary.ToBoolean(Request["update_from_cart"]);
            if (this._Add_Or_Update(ctsp_id, soluong, update_from_cart))
            {
                return RedirectToAction("Index", "FrontCart");
            }
            return RedirectToAction("Index", "FrontHome");
        }
        [HttpGet]
        public ActionResult Add_Or_Update(int ctsp_id=0, int soluong=0)
        {
            if (this._Add_Or_Update(ctsp_id, soluong, false))
            {
                return RedirectToAction("Index", "FrontCart");
            }
            return RedirectToAction("Index", "FrontHome");
        }
        [NonAction]
        protected Boolean _Add_Or_Update(int ctsp_id=0, int soluong=0, Boolean update_from_cart=false)
        {
            List<string> validate = new List<string>();
            ChiTietSPController ctr = new ChiTietSPController();
            DonHangController ctr_donhang = new DonHangController();
            
            //validate id
            if (!ctr.is_exist(ctsp_id))
            {
                return false;
            }
            
            //get obj
            ChiTiet_DonHang obj = this._giohang.ds_chitiet_donhang.Where(x => x.chitietsp.id == ctsp_id).FirstOrDefault();
            //add or update
            if (obj == null)
            {
                //chưa có trong cart => thêm vào
                ChiTiet_DonHang tmp = new ChiTiet_DonHang();
                tmp.chitietsp = new ChiTietSPController().get_by_id(ctsp_id);
                tmp.dongia = tmp.chitietsp.sanpham.gia;
                tmp.soluong = soluong;
                //just add
                this._giohang.ds_chitiet_donhang.Add(tmp);
            }
            else
            {
                //đã có trong cart
                if (update_from_cart)
                {
                    //cập nhật từ giỏ hàng
                    obj.soluong = soluong;
                }
                else
                {
                    //thêm từ nơi khác
                    obj.soluong += soluong;
                }
            }
            //validate giohang
            validate.AddRange(ctr_donhang.validate(this._giohang, out this._giohang));
            //finally call save
            this._save_cart_to_session();

            TempData["state"] = validate;
            return true;
        }
        [HttpGet]
        public ActionResult Login_Or_Register()
        {
            Session["link_after_login"] = Url.Action("CheckOut", "FrontCart");
            return View();
        }
    }
}
