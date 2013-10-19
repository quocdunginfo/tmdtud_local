using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;

namespace qdtest.Controllers.ModelController
{
    public class KichThuocController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public KichThuoc get_by_id(int obj_id)
        {
            return _db.ds_kichthuoc.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean is_exist(int obj_id)
        {
            KichThuoc obj = (from obj_tmp in _db.ds_kichthuoc
                  where obj_tmp.id == obj_id
                  select obj_tmp).FirstOrDefault();
            return obj==null?false:true;
        }
        public int add(KichThuoc obj)
        {
            this._db.ds_kichthuoc.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_kichthuoc.Max(x => x.id);
        }
        public Boolean delete(int obj_id)
        {
            //get entity
            KichThuoc obj = this.get_by_id(obj_id);
            //check null
            if (obj == null) return false;
            //remove
            this._db.ds_kichthuoc.Remove(obj);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String giatri = "", String mota="", String active = "")
        {
            return timkiem(id, giatri, mota, active).Count;
        }
        public List<KichThuoc> timkiem(String id = "", String giatri = "", String mota="",String active = "", String order_by="id", Boolean order_desc=true, int start_point=0, int count=-1)
        {
            List<KichThuoc> obj_list = new List<KichThuoc>();
            //find by LIKE element
            obj_list = this._db.ds_kichthuoc.Where(x => x.giatri.Contains(giatri)
                && x.mota.Contains(mota)
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
    }
}