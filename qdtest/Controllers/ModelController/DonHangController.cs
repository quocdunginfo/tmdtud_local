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
        public int add(DonHang obj)
        {
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
                obj_list = obj_list.OrderByDescending(x => x.id).ToList();
            }
            else
            {
                obj_list = obj_list.OrderBy(x => x.id).ToList();
            }

            //limit
            if (count >= 0)
            {
                obj_list = obj_list.Skip(start_point).Take(count).ToList();
            }
            //FINAL return
            return obj_list;
        }
        public List<String> validate(DonHang obj)
        {
            List<String> re = new List<string>();
            return re;
        }
    }
}