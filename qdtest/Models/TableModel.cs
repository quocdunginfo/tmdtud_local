using qdtest._Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Models
{
    public class HinhAnh
    {
        public HinhAnh()
        {
            this.id = 0;
            this.duongdan = this.duongdan_thumb = "";
            this.macdinh = false;
        }
        public int id { get; set; }
        public String duongdan { get; set; }//đường dẫn tương đối
        public String duongdan_thumb { get; set; }//đường dẫn tương đối
        public Boolean macdinh { get; set; }//Hình ảnh hiện trên trang danh mục
        //external
        public virtual SanPham sanpham { get; set; }
        //method
        public string _get_duongdan_url()
        {
            string path = "~/_Upload/HinhAnh/";
            path += this.duongdan;
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return helper.Content(path);
        }
        public string _get_duongdan_thumb_url()
        {
            string path = "~/_Upload/HinhAnh/";
            path += this.duongdan_thumb;
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return helper.Content(path);
        }
    }
    public class SanPham
    {
        public SanPham()
        {
            this.ds_chitietsp = new List<ChiTietSP>();
            this.ds_hinhanh = new List<HinhAnh>();
            this.masp = "";
            this.ten = "";
            this.mota = "";
            this.gia = 0;
        }
        [Key]
        public int id { get; set; }
        public String masp { get; set; }
        public String ten { get; set; }
        public String mota { get; set; }
        public int gia { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<ChiTietSP> ds_chitietsp { get; set; }
        public virtual List<HinhAnh> ds_hinhanh { get; set; }
        public virtual HangSX hangsx { get; set; }
        public virtual NhomSanPham nhomsanpham { get; set; }
        public HinhAnh _get_hinhanh_macdinh()
        {
            HinhAnh re= this.ds_hinhanh.Where(x => x.macdinh == true).FirstOrDefault();
            if (re == null)
            {
                re = new HinhAnh();
                re.duongdan = "default_no_image.jpg";
                re.duongdan_thumb = "_thumb_default_no_image.jpg";
                re.macdinh = true;
                re.sanpham = this;
            }
            return re;
        }
        public string _get_mota_lite(int max_length=200)
        {
            if (this.mota.Equals(""))
            {
                return "(Chưa có mô tả cho sản phẩm)";
            }
            try
            {
                string tmp = this.mota;
                tmp = TextLibrary.HTML_Strip(tmp);
                tmp = TextLibrary.Unicode_Substring(tmp, max_length);
                return tmp+"...";
            }catch(Exception ex)
            {
                return this.mota+"...";
            }
        }
    }
    public class Topic
    {
        public Topic()
        {
            this.ds_phanhoi = new List<PhanHoi>();
            this.ten = "";
            this.noidung = "";
            this.ngay = DateTime.Now;
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        public String noidung { get; set; }
        public DateTime ngay { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual KhachHang nguoitao { get; set; }
        public virtual List<PhanHoi> ds_phanhoi { get; set; }
    }
    public class PhanHoi
    {
        public PhanHoi()
        {
            this.ten = "";
            this.noidung = "";
            this.ngay = DateTime.Now;
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        public String noidung { get; set; }
        public DateTime ngay { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual NhanVien nhanvien { get; set; }//Nhân viên gửi
        public virtual KhachHang khachhang { get; set; }//Khách Hàng gửi
        public virtual Topic topic { get; set; }//thuộc Topic nào
    }
    public class ChiTietSP
    {
        public ChiTietSP()
        {
            this.ds_chitiet_donhang = new List<ChiTiet_DonHang>();
            this.ds_chitiet_nhaphang = new List<ChiTiet_NhapHang>();
            this.ds_tonkho = new List<TonKho>();
            this.soluong = 0;
        }
        [Key]
        public int id { get; set; }
        public int soluong { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<ChiTiet_DonHang> ds_chitiet_donhang { get; set; }
        public virtual List<ChiTiet_NhapHang> ds_chitiet_nhaphang { get; set; }
        public virtual List<TonKho> ds_tonkho { get; set; }
        public virtual KichThuoc kichthuoc { get; set; }
        public virtual MauSac mausac { get; set; }
        public virtual SanPham sanpham { get; set; }
    }
    public class KichThuoc
    {
        public KichThuoc()
        {
            this.ds_chitietsp = new List<ChiTietSP>();
            this.giatri = "";
            this.active = true;
        }
        [Key]
        public int id { get; set; }
        public String giatri { get; set; }
        public String mota { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<ChiTietSP> ds_chitietsp { get; set; }
    }
    public class MauSac
    {
        public MauSac()
        {
            this.ds_chitietsp = new List<ChiTietSP>();
            this.giatri = "";
            this.active = true;
            this.mamau = "#000000";//mac dinh la mau den
        }
        [Key]
        public int id { get; set; }
        public String giatri { get; set; }
        public String mamau { get; set; }
        public String mota { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<ChiTietSP> ds_chitietsp { get; set; }
    }
    public class HangSX
    {
        public HangSX()
        {
            this.ds_sanpham = new List<SanPham>();
            this.ten = "";
            this.active = true;
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<SanPham> ds_sanpham { get; set; }
    }
    public class NhomSanPham
    {
        public NhomSanPham()
        {
            this.ds_sanpham = new List<SanPham>();
            this.ds_nhomcon = new List<NhomSanPham>();
            this.ten = "";
            this.mota = "";
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        public String mota { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual NhomSanPham nhomcha { get; set; }
        public virtual List<NhomSanPham> ds_nhomcon { get; set; }
        public virtual List<SanPham> ds_sanpham { get; set; }
    }
    public class NhaCC
    {
        public NhaCC()
        {
            this.ds_nhaphang = new List<NhapHang>();
            this.ten_ncc = "";
            this.diachi_ncc = "";
            this.sdt_ncc = "";
        }
        [Key]
        public int id { get; set; }
        public String ten_ncc { get; set; }
        public String diachi_ncc { get; set; }
        public String sdt_ncc { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<NhapHang> ds_nhaphang { get; set; }
    }
    public class NhapHang
    {
        public NhapHang()
        {
            this.ds_chitiet_nhaphang = new List<ChiTiet_NhapHang>();
            this.ngay = DateTime.Now;
            this.tongtien = 0;
            this.thanhtoan = 0;
            this.ten_nhacungcap = "";
            this.ten_nhanvien = "";
        }
        [Key]
        public int id { get; set; }
        public DateTime ngay { get; set; }
        public int tongtien { get; set; }
        public int thanhtoan { get; set; }
        public String ten_nhanvien { get; set; }
        public String ten_nhacungcap { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual NhaCC nhacc { get; set; }
        public virtual NhanVien nhanvien { get; set; }
        public virtual List<ChiTiet_NhapHang> ds_chitiet_nhaphang { get; set; }

    }
    public class ChiTiet_NhapHang
    {
        [Key]
        public int id { get; set; }
        public int soluong { get; set; }
        public int dongia { get; set; }
        //external
        public virtual NhapHang nhaphang { get; set; }
        public virtual ChiTietSP chitietsp { get; set; }
    }
    public class DonHang
    {
        public DonHang()
        {
            this.ds_chitiet_donhang = new List<ChiTiet_DonHang>();
            this.ngay = DateTime.Now;
            this.nguoinhan_diachi = "";
            this.nguoinhan_ten = "";
            this.nguoinhan_sdt = "";
            this.trangthai = "chualienlac";
            this.thanhtoan_tructuyen = false;
            this.tongtien = 0;
            this.active = true;
            this.phivanchuyen = 0;
        }
        [Key]
        public int id { get; set; }
        public DateTime ngay { get; set; }
        public int tongtien { get; set; }
        public String trangthai { get; set; }//"chualienlac","chuagiao","dagiao", "dabihuy"
        public String nguoinhan_diachi { get; set; }
        public String nguoinhan_ten { get; set; }
        public String nguoinhan_sdt { get; set; }
        public Boolean active { get; set; }
        public Boolean thanhtoan_tructuyen { get; set; }//thanhtoantructuyen, thanhtoantainha
        public int phivanchuyen { get; set; }//dua vao tinhtp nguoi nhan
        //external
        public virtual List<ChiTiet_DonHang> ds_chitiet_donhang { get; set; }

        public virtual int? khachhang_id { get; set; }
        public virtual KhachHang khachhang { get; set; }

        public virtual int? khachhang_nhanvien_id { get; set; }
        public virtual NhanVien khachhang_nhanvien { get; set; }

        public virtual int? nhanvien_id { get; set; }
        public virtual NhanVien nhanvien { get; set; }

        public virtual TinhTP nguoinhan_diachi_tinhtp { get; set; }
        //method
        public String _get_trangthai()
        {
            if (!this.active)
            {
                return "Đã bị hủy";
            }
            if (this.trangthai.Equals("dabihuy"))
            {
                return "Đã bị hủy";
            }
            if (this.trangthai.Equals("chualienlac"))
            {
                return "Chưa liên lạc - đơn hàng mới";
            }
            if (this.trangthai.Equals("chuagiao"))
            {
                return "Đã thanh toán - chưa giao hàng";
            }
            if (this.trangthai.Equals("dagiao"))
            {
                return "Đã giao hàng - hoàn tất";
            }
            return "Unknown";
        }
        public String _get_ngay()
        {
            String format = "dd/MM/yyyy";
            return String.Format("{0:"+format+"}", this.ngay);
        }
        public String _get_khachhang_tendaydu()
        {
            if (this.khachhang != null)
            {
                return khachhang.tendaydu;
            }
            if (this.khachhang_nhanvien != null)
            {
                return "[Nhân viên]: "+khachhang_nhanvien.tendaydu;
            }
            return "";
        }
        public string _get_phivanchuyen()
        {
            return TextLibrary.ToCommaStringNumber(this.phivanchuyen);
        }
        public String _get_khachhang_diachi()
        {
            if (this.khachhang != null)
            {
                return khachhang.diachi;
            }
            if (this.khachhang_nhanvien != null)
            {
                return "(Nhân viên mua hàng - không có địa chỉ)";
            }
            return "";
        }
        public String _get_khachhang_sdt()
        {
            if (this.khachhang != null)
            {
                return khachhang.sdt;
            }
            if (this.khachhang_nhanvien != null)
            {
                return "(Nhân viên mua hàng - không có số điện thoại)";
            }
            return "";
        }
        public String _get_khachhang_email()
        {
            if (this.khachhang != null)
            {
                return khachhang.sdt;
            }
            if (this.khachhang_nhanvien != null)
            {
                return khachhang_nhanvien.email;
            }
            return "";
        }
        public String _get_tongtien()
        {
            if (this.phivanchuyen + this.__get_tongtien_notinclude_phivanchuyen() != this.tongtien)
            {
                return TextLibrary.ToCommaStringNumber(this.phivanchuyen + this.__get_tongtien_notinclude_phivanchuyen());
            }
            return TextLibrary.ToCommaStringNumber(this.tongtien);
        }
        public Boolean _is_nhanvien_muahang()
        {
            return this.khachhang_nhanvien==null?false:true;
        }
        public String _get_url_to_khachhang_both()
        {
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (this._is_nhanvien_muahang())
            {
                return helper.Action("Index", "AdminUser", new { id = this.khachhang_nhanvien.id });
            }
            else if (this.khachhang!=null)
            {
                return helper.Action("Index", "AdminKhachHang", new { id = this.khachhang.id });
            }
            return helper.Action("Index", "AdminDonHang", new { id = this.id });
            
        }
        private int __get_tongtien_notinclude_phivanchuyen()
        {
            int sum = 0;
            foreach (var item in this.ds_chitiet_donhang)
            {
                sum += item.dongia * item.soluong;
            }
            return sum;
        }
        public string _get_tongtien_notinclude_phivanchuyen()
        {
            int sum = 0;
            foreach (var item in this.ds_chitiet_donhang)
            {
                sum += item.dongia * item.soluong;
            }
            return TextLibrary.ToCommaStringNumber(sum);
        }
    }
    public class ChiTiet_DonHang
    {
        [Key]
        public int id { get; set; }
        public int soluong { get; set; }
        public int dongia { get; set; }
        //external
        public virtual DonHang donhang { get; set; }
        public virtual ChiTietSP chitietsp { get; set; }
        //method
        public string _get_total()
        {
            return TextLibrary.ToCommaStringNumber(this.soluong * this.dongia);
        }
    }
    public class KhachHang
    {
        public KhachHang()
        {
            this.ds_donhang = new List<DonHang>();
            this.tendangnhap = "";
            this.tendaydu = "";
            this.matkhau = "";
            this.diachi = "";
            this.email = "";
            this.sdt = "";
            this.active = true;
        }
        [Key]
        public int id { get; set; }
        public String tendangnhap { get; set; }
        public String tendaydu { get; set; }
        public String matkhau { get; set; }
        public String diachi { get; set; }
        public String email { get; set; }
        public String sdt { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<DonHang> ds_donhang { get; set; }
    }
    public class NhanVien
    {
        public NhanVien()
        {
            this.id = 0;
            this.active = true;
            // this.ds_sanpham = new List<SanPham>();
            this.ds_donhang = new List<DonHang>();
            this.ds_nhaphang = new List<NhapHang>();
            this.tendangnhap = "";
            this.tendaydu = "";
            this.email = "";
            this.matkhau = "";
            this.forgot_password_session = "";
        }
        [Key]
        public int id { get; set; }
        public String tendangnhap { get; set; }
        public String tendaydu { get; set; }
        public String email { get; set; }
        public String matkhau { get; set; }
        public Boolean active { get; set; }
        public String forgot_password_session { get; set; }
        //external
        //public virtual List<SanPham> ds_sanpham { get; set; }
        public virtual List<DonHang> ds_donhang { get; set; }
        public virtual List<DonHang> ds_donhang_mua { get; set; }
        public virtual List<NhapHang> ds_nhaphang { get; set; }
        public virtual LoaiNhanVien loainhanvien { get; set; }
    }
    public class LoaiNhanVien
    {
        public LoaiNhanVien()
        {
            this.ds_nhanvien = new List<NhanVien>();
            this.ds_quyen = new List<Quyen>();
            this.ten = "";
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        //external
        public virtual List<Quyen> ds_quyen { get; set; }
        public virtual List<NhanVien> ds_nhanvien { get; set; }
    }
    public class Quyen
    {
        public Quyen()
        {
            this.ds_loainhanvien = new List<LoaiNhanVien>();
            this.ten = "";
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        //external
        public virtual List<LoaiNhanVien> ds_loainhanvien { get; set; }
    }
    public class TonKho
    {
        public TonKho()
        {
        }
        [Key]
        public int id { get; set; }
        public DateTime ngay { get; set; }
        public int soluongnhap { get; set; }
        public int soluongban { get; set; }
        //external
        public virtual ChiTietSP chitietsp { get; set; }
    }
    public class TinhTP
    {
        public TinhTP()
        {
            this.id = 0;
            this.ten = "";
            this.noithanh = true;
            this.phivanchuyen = 0;
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        public int phivanchuyen { get; set; }
        public Boolean noithanh { get; set; }
        //external
        public virtual List<DonHang> ds_donhang { get; set; }
        //method
        public string _get_phivanchuyen()
        {
            return this.phivanchuyen.ToString("#,#", CultureInfo.InvariantCulture);
        }
    }
    public class BanGiayDBContext : DbContext
    {
        public DbSet<DonHang> ds_donhang { get; set; }
        public DbSet<ChiTiet_DonHang> ds_chitiet_donhang { get; set; }
        public DbSet<SanPham> ds_sanpham { get; set; }
        public DbSet<ChiTietSP> ds_chitietsp { get; set; }
        public DbSet<MauSac> ds_mausac { get; set; }
        public DbSet<KichThuoc> ds_kichthuoc { get; set; }
        public DbSet<HangSX> ds_hangsx { get; set; }
        public DbSet<NhomSanPham> ds_nhomsanpham { get; set; }
        public DbSet<HinhAnh> ds_hinhanh { get; set; }
        public DbSet<KhachHang> ds_khachhang { get; set; }
        public DbSet<NhanVien> ds_nhanvien { get; set; }
        public DbSet<ChiTiet_NhapHang> ds_chitiet_nhaphang { get; set; }
        public DbSet<NhapHang> ds_nhaphang { get; set; }
        public DbSet<NhaCC> ds_nhacc { get; set; }
        public DbSet<TonKho> ds_tonkho { get; set; }
        public DbSet<LoaiNhanVien> ds_loainhanvien { get; set; }
        public DbSet<Quyen> ds_quyen { get; set; }
        public DbSet<TinhTP> ds_tinhtp { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>()
                        .HasOptional(m => m.nhanvien)
                        .WithMany(t => t.ds_donhang)
                        .HasForeignKey(m => m.nhanvien_id)
                        .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<DonHang>()
                        .HasOptional(m => m.khachhang)
                        .WithMany(t => t.ds_donhang)
                        .HasForeignKey(m => m.khachhang_id)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<DonHang>()
                        .HasOptional(m => m.khachhang_nhanvien)
                        .WithMany(t => t.ds_donhang_mua)
                        .HasForeignKey(m => m.khachhang_nhanvien_id)
                        .WillCascadeOnDelete(false);
        }
    }
}