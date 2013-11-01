using qdtest._Library;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qdtest.Controllers.ModelController
{
    public class LoaiNhanVienController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public LoaiNhanVienController()
        {

        }
        public LoaiNhanVienController(BanGiayDBContext db)
        {
            this._db = db;
        }
        public LoaiNhanVien get_by_id(int obj_id)
        {
            return _db.ds_loainhanvien.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean is_exist(int obj_id)
        {
            return this.get_by_id(obj_id)== null ? false : true;
        }
        public int add(LoaiNhanVien obj)
        {
            this._db.ds_loainhanvien.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_loainhanvien.Max(x => x.id);
        }
        public Boolean gan_quyen_han(int id, List<int> quyen_list)
        {
            //xoa het quyen han qua loainhanvien hien tai
            LoaiNhanVien lnv = this.get_by_id(id);
            if (lnv == null) return false;
            lnv.ds_quyen.Clear();
            this._db.SaveChanges();
            //add moi
            QuyenController ctr_quyen = new QuyenController(this._db);
            lnv.ds_quyen.AddRange(ctr_quyen.list_id_to_list_obj(quyen_list));
            this._db.SaveChanges();
            return true;
        }
        public Boolean delete(int obj_id)
        {
            //get entity
            LoaiNhanVien obj = this.get_by_id(obj_id);
            //check null
            if (obj == null) return false;
            //remove
            this._db.ds_loainhanvien.Remove(obj);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String ten = "")
        {
            return timkiem(id, ten).Count;
        }
        public List<LoaiNhanVien> timkiem(String id = "", String ten = "", String order_by="id", Boolean order_desc=true, int start_point=0, int count=-1)
        {
            List<LoaiNhanVien> obj_list = new List<LoaiNhanVien>();
            //find by LIKE element
            obj_list = this._db.ds_loainhanvien.Where(x => x.ten.Contains(ten)
                ).ToList();
            //filter by id                
            if (!id.Equals(""))
            {
                int id_i = TextLibrary.ToInt(id);
                obj_list = obj_list.Where(x => x.id == id_i).ToList();
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

        public List<string> validate(LoaiNhanVien obj)
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