using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;

namespace qdtest.Controllers.ModelController
{
    public class PhanHoiController
    {
        public BanGiayDBContext _db;
        public PhanHoiController()
        {
            this._db = new BanGiayDBContext();
        }
        public PhanHoiController(BanGiayDBContext db)
        {
            this._db = db;
        }
        public PhanHoi get_by_id(int obj_id)
        {
            return _db.ds_phanhoi.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean is_exist(int obj_id)
        {
            return this.get_by_id(obj_id)== null ? false : true;
        }
        public int add(PhanHoi obj)
        {
            this._db.ds_phanhoi.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_phanhoi.Max(x => x.id);
        }
        public Boolean delete(int obj_id)
        {
            //get entity
            PhanHoi obj = this.get_by_id(obj_id);
            //check null
            if (obj == null) return false;
            //remove
            this._db.ds_phanhoi.Remove(obj);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String tieude = "", String noidung = "", String nguoigui_ten = "", String nguoigui_email = "", String nguoigui_sdt = "")
        {
            return timkiem(id, tieude, noidung, nguoigui_ten,nguoigui_email,nguoigui_sdt).Count;
        }
        public List<PhanHoi> timkiem(String id = "", String tieude = "",String noidung = "", String nguoigui_ten="",String nguoigui_email="",String nguoigui_sdt="", String order_by="id", Boolean order_desc=true, int start_point=0, int count=-1)
        {
            List<PhanHoi> obj_list = new List<PhanHoi>();
            //find by LIKE element
            
            obj_list = this._db.ds_phanhoi.Where(x =>
                (
                    x.khachhang!=null
                    && x.khachhang.tendaydu.Contains(nguoigui_ten)
                    && x.khachhang.email.Contains(nguoigui_email)
                    && x.khachhang.sdt.Contains(nguoigui_sdt)
                )
                ||
                (
                    x.khachhang==null
                    && x.nguoigui_ten.Contains(nguoigui_ten) 
                    && x.nguoigui_email.Contains(nguoigui_email) 
                    && x.nguoigui_sdt.Contains(nguoigui_sdt)
                )
                && x.tieude.Contains(tieude)
                && x.noidung.Contains(noidung)
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

        public List<string> validate(PhanHoi obj)
        {
            List<String> re = new List<string>();
            if (obj.tieude.Equals(""))
            {
                re.Add("tieude_fail");
            }
            if (obj.noidung.Equals(""))
            {
                re.Add("noidung_fail");
            }
            if (obj.khachhang == null)
            {
                if (obj.nguoigui_ten.Equals(""))
                {
                    re.Add("nguoigui_ten_fail");
                }
                if (!ValidateLibrary.is_valid_email(obj.nguoigui_email))
                {
                    re.Add("nguoigui_email_fail");
                }
                if (obj.nguoigui_sdt.Equals(""))
                {
                    re.Add("nguoigui_sdt_fail");
                }
            }
            else
            {
                //nothing
            }
            return re;
            
        }
    }
}