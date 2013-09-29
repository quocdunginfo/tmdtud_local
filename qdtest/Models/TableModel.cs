using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace qdtest.Models
{
    public class HinhAnh
    {
        public int id { get; set; }
        public String duongdan { get; set; }//đường dẫn tương đối
    }
    public class SanPham
    {
        public SanPham()
        {
            this.ds_sanpham_tag = new List<SanPham_Tag>();
            this.ds_dathang = new List<DatHang>();
            this.ds_hinhanh = new List<HinhAnh>();
        }
        [Key]
        public int id { get; set; }
        public String masp { get; set; }
        public String ten { get; set; }
        public String mota { get; set; }
        public int gia { get; set; }
        public Boolean active { get; set; }
        //external
        public virtual List<SanPham_Tag> ds_sanpham_tag { get; set; }
        public virtual List<DatHang> ds_dathang { get; set; }
        public virtual List<HinhAnh> ds_hinhanh { get; set; }
        public virtual HangSX hangsx { get; set; }
        public virtual NhomSanPham nhomsanpham { get; set; }
        public virtual NhanVien nguoidung { get; set; }
    }
    public class SanPham_Tag
    {
        [Key]
        public int id { get; set; }
        public int soluong { get; set; }
        //external
        public virtual KichThuoc kichthuoc { get; set; }
        public virtual MauSac mausac { get; set; }
        public virtual SanPham sanpham { get; set; }
    }
    public class KichThuoc
    {
        public KichThuoc()
        {
            this.ds_sanpham_tag = new List<SanPham_Tag>();
        }
        [Key]
        public int id { get; set; }
        public String giatri { get; set; }
        //external
        public virtual List<SanPham_Tag> ds_sanpham_tag { get; set; }
    }
    public class MauSac
    {
        public MauSac()
        {
            this.ds_sanpham_tag = new List<SanPham_Tag>();
        }
        [Key]
        public int id { get; set; }
        public String giatri { get; set; }
        //external
        public virtual List<SanPham_Tag> ds_sanpham_tag { get; set; }
    }
    public class HangSX
    {
        public HangSX()
        {
            this.ds_sanpham = new List<SanPham>();
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        //external
        public virtual List<SanPham> ds_sanpham { get; set; }
    }
    public class NhomSanPham
    {
        public NhomSanPham()
        {
            this.ds_sanpham = new List<SanPham>();
            this.ds_nhomcon = new List<NhomSanPham>();
        }
        [Key]
        public int id { get; set; }
        public String ten { get; set; }
        //external
        public virtual NhomSanPham nhomcha { get; set; }
        public virtual List<NhomSanPham> ds_nhomcon { get; set; }
        public virtual List<SanPham> ds_sanpham { get; set; }
    }
    public class DatHang
    {
        public DatHang()
        {
            this.ds_chitiet_dathang = new List<ChiTiet_DatHang>();
        }
        [Key]
        public int id { get; set; }
        public DateTime ngay { get; set; }
        public int tongtien { get; set; }
        public Boolean dathanhtoan { get; set; }
        public Boolean dagiaohang { get; set; }
        public String diachi_nguoinhan { get; set; }
        public String ten_nguoinhan { get; set; }
        public String sdt_nguoinhan { get; set; }
        //external
        public virtual List<ChiTiet_DatHang> ds_chitiet_dathang { get; set; }
        public virtual NhanVien nguoidung { get; set; }
        public virtual KhachHang khachhang { get; set; }
    }
    public class ChiTiet_DatHang
    {
        [Key]
        public int id { get; set; }
        public int soluong { get; set; }
        public int dongia { get; set; }
        //external
        public virtual SanPham_Tag sanpham_tag { get; set; }
    }
    public class KhachHang
    {
        public KhachHang()
        {
            this.ds_dathang = new List<DatHang>();
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
        public virtual List<DatHang> ds_dathang { get; set; }
    }
    public class NhanVien
    {
        public NhanVien()
        {
            this.ds_sanpham = new List<SanPham>();
            this.ds_dathang = new List<DatHang>();
        }
        [Key]
        public int id { get; set; }
        public String tendangnhap { get; set; }
        public String tendaydu { get; set; }
        public String email { get; set; }
        public String matkhau { get; set; }
        public Boolean active { get; set; }
        public int group_id { get; set; }//0: Admin, 1: Normal user, 2: Guest
        //external
        public virtual List<SanPham> ds_sanpham { get; set; }
        public virtual List<DatHang> ds_dathang { get; set; }
    }
    public class BanGiayDBContext : DbContext
    {
        public DbSet<DatHang> ds_dathang { get; set; }
        public DbSet<ChiTiet_DatHang> ds_chitiet_dathang { get; set; }
            public DbSet<SanPham> ds_sanpham { get; set; }
                public DbSet<SanPham_Tag> ds_sanpham_tag { get; set; }
                    public DbSet<MauSac> ds_mausac { get; set; }
                    public DbSet<KichThuoc> ds_kichthuoc { get; set; }
                public DbSet<HangSX> ds_hangsx { get; set; }
                public DbSet<NhomSanPham> ds_nhomsanpham { get; set; }
                public DbSet<HinhAnh> ds_hinhanh { get; set; }
        public DbSet<KhachHang> ds_khachhang { get; set; }
        public DbSet<NhanVien> ds_nhanvien { get; set; }
    }
}