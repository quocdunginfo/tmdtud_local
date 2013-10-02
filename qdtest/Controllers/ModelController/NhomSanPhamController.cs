using qdtest._Library;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers.ModelController
{
    public class NhomSanPhamController : Controller
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
       
        public NhomSanPham get_by_id(int id)
        {
            return _db.ds_nhomsanpham.FirstOrDefault(x => x.id == id);
        }
        public Boolean is_exist(int id)
        {
            NhomSanPham u = (from user in _db.ds_nhomsanpham
                          where user.id == id
                          select user).FirstOrDefault();
            return u == null ? false : true;
        }
        public int add(NhomSanPham obj)
        {
            this._db.ds_nhomsanpham.Add(obj);
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_nhomsanpham.Max(x => x.id);
        }
        public Boolean delete(int id)
        {
            //Xóa object có dính khóa ngoại trước
            NhomSanPham obj = this._db.ds_nhomsanpham.Where(x => x.id == id).FirstOrDefault();
            if (obj == null) return false;
            this._db.ds_nhomsanpham.Remove(obj);
            this._db.SaveChanges();
            return true;
        }
        public Boolean delete(int id, int transfer_id)
        {
            //tranfer all object has this ...
            NhomSanPham transfer = this._db.ds_nhomsanpham.Where(x => x.id == transfer_id).FirstOrDefault();
            NhomSanPham canxoa = this._db.ds_nhomsanpham.Where(x => x.id == id).FirstOrDefault();
            if(transfer==null || canxoa==null)
            {
                return false;
            }
            canxoa.ds_sanpham.ForEach(x => x.nhomsanpham = transfer);
            //remove
            this._db.ds_nhomsanpham.Remove(canxoa);
            this._db.SaveChanges();
            return true;
        }
        public List<NhomSanPham> timkiem(String id = "", String ten = "")
        {
            List<NhomSanPham> list = new List<NhomSanPham>();
            if (!id.Equals(""))
            {
                //find by id
                int id_i = TextLibrary.ToInt(id);
                list = this._db.ds_nhomsanpham.Where(x => x.id == id_i).ToList();
                if (list == null)
                {
                    list = new List<NhomSanPham>();
                }
                return list;
            }
            //find by LIKE elament
            list = this._db.ds_nhomsanpham.Where(x => x.ten.Contains(ten)).ToList();
            if (list == null)
            {
                list = new List<NhomSanPham>();
            }

            return list;
        }
    }
}
