using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest._Library;
using qdtest.Models;

namespace CuaHangBanGiay.Controllers
{
    public class ChiTietSPController : Controller
    {
        //
        // GET: /SanPham_Tag/

        public BanGiayDBContext _db;
        public ChiTietSPController(BanGiayDBContext _db)
        {
            this._db = _db;
        }
        public ChiTietSPController()
        {
            this._db = new BanGiayDBContext();
        }
        public ChiTietSP get_by_id(int id)
        {
            return _db.ds_sanpham_tag.FirstOrDefault(x => x.id == id);
        }
        public Boolean is_exist(int id)
        {
           ChiTietSP u = (from spt in _db.ds_sanpham_tag
                             where spt.id == id
                             select spt).FirstOrDefault();
            return u == null ? false : true;
        }
        public int add(ChiTietSP obj)
        {
            this._db.ds_sanpham_tag.Add(obj);
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_sanpham_tag.Max(x => x.id);
        }
        public Boolean delete(int id)
        {
            //Xóa object có dính khóa ngoại trước
            ChiTietSP obj = this._db.ds_sanpham_tag.Where(x => x.id == id).FirstOrDefault();
            if (obj == null) return false;
            this._db.ds_sanpham_tag.Remove(obj);
            this._db.SaveChanges();
            return true;
        }
        public Boolean edit(ChiTietSP spt)
        {
            if (this.is_exist(spt.id))
            {
                ChiTietSP obj = get_by_id(spt.id);
                obj.kichthuoc = spt.kichthuoc;
                obj.mausac = spt.mausac;
                obj.soluong = spt.soluong;
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public List<ChiTietSP> timkiem(String id = "", String mausac_id = "", String kichthuoc_id = "", String active = "",String id_sp="")
        {
            List<ChiTietSP> list=_db.ds_sanpham_tag.ToList();
            if (!id.Equals(""))
            {
                //find by id
                int id_i = TextLibrary.ToInt(id);
                list =_db.ds_sanpham_tag.Where(x => x.id == id_i).ToList();
                if (list == null)
                {
                    list = new List<ChiTietSP>();
                }
                return list;
            }
            if(!id_sp.Equals(""))
            {
                int spid=TextLibrary.ToInt(id_sp);
                list=list.Where(x=>x.sanpham.id==spid).ToList();
                if(list==null)
                {
                    list=new List<ChiTietSP>();
                }
            }
            if(!mausac_id.Equals(""))
            {
                int id_mau=TextLibrary.ToInt(mausac_id);
                list=list.Where(x=>x.mausac.id==id_mau).ToList();
                if(list==null)
                {
                    list=new List<ChiTietSP>();
                }
            }
            if(!kichthuoc_id.Equals(""))
            {
                int id_kt=TextLibrary.ToInt(kichthuoc_id);
                list=list.Where(x=>x.kichthuoc.id==id_kt).ToList();
                if(list==null)
                {
                    list=new List<ChiTietSP>();
                }
            }
            //Filter again by by active
            if (!active.Equals(""))
            {
                Boolean active_b = TextLibrary.ToBoolean(active);
                list = list.Where(x => x.active == active_b).ToList();
                if (list == null)
                {
                    list = new List<ChiTietSP>();
                }
            }
            //FINAL return
            return list;
        
    }
    }
}
