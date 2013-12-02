using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;

namespace qdtest.Controllers
{
    public class FrontController : WebController
    {
        protected KhachHang _khachhang;
        protected DonHang _giohang;
        public FrontController():base()
        {
            this._khoitao_cookie();
            this._khachhang = null;
            this._giohang = new DonHang();
        }
        //
        // GET: /Layout/
        protected HttpCookie front_timkiem_sanpham=null;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            KhachHangController ctr_kh = new KhachHangController();
            NhomSanPhamController ctr = new NhomSanPhamController();
            List<NhomSanPham2> list1 = ctr.timkiem("", "", "", "1");
            SanPhamController ctr2 = new SanPhamController(ctr._db);
            List<SanPham>list2=ctr2.get_bestseller(4);
            if (list1 != null && list2 != null)
            {
                ViewBag.NhomSanPham2_List_All = list1;
                ViewBag.SanPham_BestSeller = list2;
            }
            else
            {
                ViewBag.NhomSanPham2_List_All = new List<NhomSanPham2>();
                ViewBag.SanPham_BestSeller = new List<SanPham>();
            }
            //tim kiem
            //build timkiem_sanpham
            if (Request.Cookies.Get("front_timkiem_sanpham") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this._khoitao_cookie();
                this._luu_cookie();
            }
            else
            {
                try
                {
                    this.front_timkiem_sanpham = CookieLibrary.Base64Decode(Request.Cookies.Get("front_timkiem_sanpham"));
                }
                catch (Exception)
                {
                    this._khoitao_cookie();
                    this._luu_cookie();
                }
            }
            ViewBag.front_timkiem_sanpham = this.front_timkiem_sanpham;
            //Load thong tin KhachHang
            if (!this._is_logged_in())
            {
                //Chưa có ai đăng nhập hệ thống
                if (Session["khachhang"] != null)
                {
                    //nếu như KH đã đăng nhập rồi
                    this._khachhang = ctr_kh.get_by_id(((KhachHang)Session["khachhang"]).id);
                }
                else
                {
                    //lấy từ cookies lên
                    //lay thong tin tu cookies
                    HttpCookie _tmp = Request.Cookies.Get("khachhang");
                    if (_tmp != null)
                    {
                        int uid = TextLibrary.ToInt(_tmp["khachhang_id"].ToString());
                        string password = TextLibrary.ToString(_tmp["khachhang_password"].ToString());
                        //lay thong tin user theo yeu cau dang nhap
                        this._khachhang = ctr_kh.get_by_id_hash_password(uid, password);
                    }
                }
            }
            //
            //
            ViewBag.nhanvien = this._nhanvien;
            ViewBag.khachhang = this._khachhang;
            //get cart
                if (Session["giohang"] != null)
                {
                    try
                    {
                        this._giohang = (DonHang)Session["giohang"];
                    }
                    catch (Exception)
                    {
                        this._giohang = new DonHang();
                    }
                }
                else
                {
                    this._giohang = new DonHang();
                }
                //gán khach hang va nhan vien
                this._giohang.khachhang = this._khachhang;
                this._giohang.khachhang_nhanvien = this._nhanvien;
                //save cart
                this._save_cart_to_session();
            ViewBag.giohang = this._giohang;
        }
        [NonAction]
        protected void _luu_cookie()
        {
            if (this.front_timkiem_sanpham != null)
            {
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.front_timkiem_sanpham));
            }
        }
        [NonAction]
        protected void _khoitao_cookie()
        {
            front_timkiem_sanpham = new HttpCookie("front_timkiem_sanpham");
            this.front_timkiem_sanpham["front_ten"] = "";
            this.front_timkiem_sanpham["front_masp"] = "";
            this.front_timkiem_sanpham["front_mota"] = "";
            this.front_timkiem_sanpham["front_gia_from"] = "0";
            this.front_timkiem_sanpham["front_gia_to"] = "0";
            this.front_timkiem_sanpham["front_hangsx_ten"] = "";
            this.front_timkiem_sanpham["front_nhomsanpham_id"] = "";
            this.front_timkiem_sanpham["front_max_item_per_page"] = "6";
            this.front_timkiem_sanpham["front_orderby"] = "id";
            this.front_timkiem_sanpham["front_desc"] = "1";
         //   this.front_timkiem_sanpham["front_current_page"] = "0";
        }
        protected string _who_logged_in()
        {
            if (this._nhanvien != null)
            {
                return "nhanvien";
            }
            else if (this._khachhang != null)
            {
                return "khachhang";
            }
            return "";
        }
        protected Boolean _is_logged_in()
        {
            return this._who_logged_in().Equals("") ? false : true ;
        }
        protected Boolean _save_cart_to_session()
        {
            Session["giohang"] = this._giohang;
            return true;
        }
    }
}
