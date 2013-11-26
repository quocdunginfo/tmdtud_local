using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;

namespace qdtest.Controllers.ModelController
{
    public class LoaiKhachHangController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public LoaiKhachHangController()
        {

        }
        public LoaiKhachHangController(BanGiayDBContext db)
        {
            this._db = db;
        }
        public LoaiKhachHang get_by_id(int obj_id)
        {
            return _db.ds_loaikhachhang.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean is_exist(int obj_id)
        {
            return this.get_by_id(obj_id)== null ? false : true;
        }
        public int add(LoaiKhachHang obj)
        {
            this._db.ds_loaikhachhang.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_loaikhachhang.Max(x => x.id);
        }
        public Boolean delete(int obj_id)
        {
            //get entity
            LoaiKhachHang obj = this.get_by_id(obj_id);
            //check null
            if (obj == null) return false;
            //remove
            this._db.ds_loaikhachhang.Remove(obj);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String ten = "", String active = "")
        {
            return timkiem(id, ten, active).Count;
        }
        public List<LoaiKhachHang> timkiem(String id = "", String ten = "",String active = "", String order_by="mucdiem", Boolean order_desc=false, int start_point=0, int count=-1)
        {
            List<LoaiKhachHang> obj_list = new List<LoaiKhachHang>();
            //find by LIKE element
            obj_list = this._db.ds_loaikhachhang.Where(x => x.ten.Contains(ten)
                ).ToList();
            //filter by id                
            if (!id.Equals(""))
            {
                int id_i = TextLibrary.ToInt(id);
                obj_list = obj_list.Where(x => x.id == id_i).ToList();
            }
            
            //Filter again by by active
                if (!active.Equals(""))
                {
                    Boolean active_b = TextLibrary.ToBoolean(active);
                    obj_list = obj_list.Where(x => x.active==active_b).ToList();
                }
            //order
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.mucdiem).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.mucdiem).ToList();
                }

            //limit
                if (count >= 0)
                {
                    obj_list = obj_list.Skip(start_point).Take(count).ToList();
                }
            //FINAL return
            return obj_list;
        }

        public List<string> validate(LoaiKhachHang obj)
        {
            List<String> re = new List<string>();
            if (obj.ten.Equals(""))
            {
                re.Add("ten_fail");
            }
            return re;
        }
    }
}