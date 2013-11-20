using qdtest._Library;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace qdtest.Controllers.ModelController
{
    public class DonHangController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public DonHangController()
        {

        }
        public DonHangController(BanGiayDBContext db)
        {
            this._db = db;
        }
        public DonHang get_by_id(int obj_id)
        {
            return _db.ds_donhang.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean is_exist(int obj_id)
        {
            DonHang obj = this.get_by_id(obj_id);
            return obj == null ? false : true;
        }
        /* Sử dụng khi lưu đon hàng, do có quá nhiều DBContext tracking nên bắt buộc phải Clone ra obj mới theo
         * this._db thì mới lưu được không sẽ bị báo lỗi
         * 
         */
        private DonHang _Clone(DonHang obj)
        {
            DonHang tmp = new DonHang();
            tmp.active = obj.active;
            tmp.ds_chitiet_donhang = new List<ChiTiet_DonHang>();
            foreach (var item in obj.ds_chitiet_donhang)
            {
                ChiTiet_DonHang ct = new ChiTiet_DonHang();
                ct.chitietsp = this._db.ds_chitietsp.Where(x => x.id == item.chitietsp.id).FirstOrDefault();
                ct.dongia = item.dongia;
                ct.soluong = item.soluong;
                tmp.ds_chitiet_donhang.Add(ct);
            }
            if (obj.khachhang != null)
            {
                tmp.khachhang = this._db.ds_khachhang.Where(x => x.id == obj.khachhang.id).FirstOrDefault();
            }
            if (obj.khachhang_nhanvien != null)
            {
                tmp.khachhang_nhanvien = this._db.ds_nhanvien.Where(x => x.id == obj.khachhang_nhanvien.id).FirstOrDefault();
            }
            tmp.ngay = obj.ngay;
            tmp.nguoinhan_diachi = obj.nguoinhan_diachi;
            tmp.nguoinhan_diachi_tinhtp = this._db.ds_tinhtp.Where(x => x.id == obj.nguoinhan_diachi_tinhtp.id).FirstOrDefault();
            tmp.nguoinhan_sdt = obj.nguoinhan_sdt;
            tmp.nguoinhan_ten = obj.nguoinhan_ten;
            if (obj.nhanvien != null)
            {
                tmp.nhanvien = this._db.ds_nhanvien.Where(x => x.id == obj.nhanvien.id).FirstOrDefault();
            }
            tmp.phivanchuyen = obj.phivanchuyen;
            tmp.thanhtoan_tructuyen = obj.thanhtoan_tructuyen;
            tmp.tongtien = obj.tongtien;
            tmp.trangthai = obj.trangthai;
            return tmp;
        }
        public int add(DonHang obj_)
        {
            DonHang obj = this._Clone(obj_);//rất quan trọng thưa các bạn,
            //tính toán các dữ liệu cần thiết
                //cập nhật trạng thái đơn hàng
                if (obj.thanhtoan_tructuyen)
                {
                    obj.trangthai = "chuagiao";
                }
                else
                {
                    obj.trangthai = "chualienlac";
                }
                //tính phí
                obj.phivanchuyen = obj.nguoinhan_diachi_tinhtp.phivanchuyen;
                obj.tongtien = obj.__get_tongtien_notinclude_phivanchuyen() + obj.phivanchuyen;
                //tính điểm cho KH nếu là kh mua
                if (obj.khachhang != null)
                {
                    obj.khachhang.diem += obj.tongtien / 10000;//10000 = 1 điểm
                    //cập nhật lại loại KH
                    obj.khachhang._Update_LoaiKhachHang();
                }
                //trừ tồn kho ngay lập tức
                foreach (var item in obj.ds_chitiet_donhang)
                {
                    item.chitietsp.soluong -= item.soluong;
                    if (item.chitietsp.soluong < 0)
                    {
                        item.chitietsp.soluong = 0;
                    }
                }
            //call add
            this._db.ds_donhang.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_donhang.Max(x => x.id);
        }
        public Boolean delete(int obj_id)
        {
            //get entity
            DonHang obj = this.get_by_id(obj_id);
            //check null
            if (obj == null) return false;
            //remove
            this._db.ds_donhang.Remove(obj);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String khachhang_ten = "", int tongtien_from = 0, int tongtien_to = 0, Boolean timkiem_ngay = false, DateTime ngay_from = new DateTime(), DateTime ngay_to = new DateTime(), String trangthai = "", String active = "")
        {
            return timkiem(id, khachhang_ten, tongtien_from, tongtien_to, timkiem_ngay, ngay_from, ngay_to, trangthai, active).Count;
        }
        public List<DonHang> timkiem(String id = "", String khachhang_ten = "", int tongtien_from = 0, int tongtien_to = 0, Boolean timkiem_ngay = false, DateTime ngay_from = new DateTime(), DateTime ngay_to = new DateTime(), String trangthai = "", String active = "", String order_by = "id", Boolean order_desc = true, int start_point = 0, int count = -1)
        {
            List<DonHang> obj_list = new List<DonHang>();
            //find by LIKE element
            obj_list = this._db.ds_donhang.Where(x =>
                x.khachhang==null || x.khachhang.tendaydu.Contains(khachhang_ten)
                ).ToList();

            //filter by id
            if (!id.Equals(""))
            {
                int id_i = TextLibrary.ToInt(id);
                obj_list = obj_list.Where(x => x.id == id_i).ToList();
            }
            //filter by trangthai
            if (!trangthai.Equals(""))
            {
                obj_list = obj_list.Where(x => x.trangthai.Equals(trangthai)).ToList();
            }

            //Filter by tongtien
            if (tongtien_from>0 || tongtien_to>0)
            {
                obj_list = obj_list.Where(x => x.tongtien >= tongtien_from && x.tongtien<=tongtien_to).ToList();
            }
            //Filter by ngay
            if(timkiem_ngay==true)
            {
                ngay_from = new DateTime(ngay_from.Year,ngay_from.Month,ngay_from.Day);//thủ thuật nhỏ để tránh bị lỗi
                ngay_to = new DateTime(ngay_to.Year, ngay_to.Month, ngay_to.Day, 23, 59, 59);//thủ thuật nhỏ để tránh bị lỗi
                obj_list = obj_list.Where(x => x.ngay.CompareTo(ngay_from) >=0 && x.ngay.CompareTo(ngay_to)<=0).ToList();
            }
            //Filter again by by active
            if (!active.Equals(""))
            {
                Boolean active_b = TextLibrary.ToBoolean(active);
                obj_list = obj_list.Where(x => x.active == active_b).ToList();
            }

            //order
            if (order_desc)
            {
                if (order_by.ToLower().Equals("id"))
                {
                    obj_list = obj_list.OrderByDescending(x => x.id).ToList();
                }
                else if (order_by.ToLower().Equals("khachhang.tendaydu"))
                {
                    obj_list = obj_list.OrderByDescending(x => x.khachhang.tendaydu).ToList();
                }
                else if (order_by.ToLower().Equals("tongtien"))
                {
                    obj_list = obj_list.OrderByDescending(x => x.tongtien).ToList();
                }
                else if (order_by.ToLower().Equals("trangthai"))
                {
                    obj_list = obj_list.OrderByDescending(x => x.trangthai).ToList();
                }
                else if (order_by.ToLower().Equals("ngay"))
                {
                    obj_list = obj_list.OrderByDescending(x => x.ngay).ToList();
                }
            }
            else
            {
                if (order_by.ToLower().Equals("id"))
                {
                    obj_list = obj_list.OrderBy(x => x.id).ToList();
                }
                else if (order_by.ToLower().Equals("khachhang.tendaydu"))
                {
                    obj_list = obj_list.OrderBy(x => x.khachhang.tendaydu).ToList();
                }
                else if (order_by.ToLower().Equals("tongtien"))
                {
                    obj_list = obj_list.OrderBy(x => x.tongtien).ToList();
                }
                else if (order_by.ToLower().Equals("trangthai"))
                {
                    obj_list = obj_list.OrderBy(x => x.trangthai).ToList();
                }
                else if (order_by.ToLower().Equals("ngay"))
                {
                    obj_list = obj_list.OrderBy(x => x.ngay).ToList();
                }
            }

            //limit
            if (count >= 0)
            {
                obj_list = obj_list.Skip(start_point).Take(count).ToList();
            }
            //FINAL return
            return obj_list;
        }
        public List<string> validate_checkout(DonHang giohang, out DonHang giohang_return)
        {
            List<string> re = new List<string>();
            if (giohang.nguoinhan_ten.Equals(""))
            {
                re.Add("ten_fail");
            }
            if (giohang.nguoinhan_sdt.Equals(""))
            {
                re.Add("sdt_fail");
            }
            if (giohang.nguoinhan_diachi.Equals(""))
            {
                re.Add("diachi_fail");
            }
            if (giohang.nguoinhan_diachi_tinhtp == null)
            {
                re.Add("tinhtp_fail");
                giohang.nguoinhan_diachi_tinhtp = this._db.ds_tinhtp.FirstOrDefault();
            }
            giohang_return = giohang;
            return re;
        }
        public List<string> validate(DonHang giohang, out DonHang giohang_return)
        {
            //
            ChiTietSPController ctr = new ChiTietSPController(this._db);
            //
            List<string> re = new List<string>();
            if (giohang.ds_chitiet_donhang.Count <= 0)
            {
                re.Add("rong_fail");
            }
            //xet soluong vuot qua ton kho hoac dat qua 3...
            ChiTietSP tmp;
            foreach (var item in giohang.ds_chitiet_donhang)
            {
                tmp = ctr.get_by_id(item.chitietsp.id);
                if (item.soluong > 3)
                {
                    re.Add(item.chitietsp.id + "_vuot3_fail");
                    item.soluong = 3;
                }
                if (item.soluong <= 0)
                {
                    re.Add(item.chitietsp.id + "_fail");
                    item.soluong = 1;
                }
                //xet ton kho cuoi cung
                if (tmp.soluong < item.soluong)
                {
                    re.Add(item.chitietsp.id + "_vuottonkho_fail");
                    item.soluong = tmp.soluong;
                }
            }

            giohang_return = giohang;
            return re;
        }
    }
}