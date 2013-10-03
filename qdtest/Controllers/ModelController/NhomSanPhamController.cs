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
        private List<NhomSanPham2> _tmp_for_get_tree=new List<NhomSanPham2>();
        private List<NhomSanPham2> _get_tree(NhomSanPham root, int level)
        {
            List<NhomSanPham> list;
            //lấy các child
            if (root == null)
            {
                list = this._db.ds_nhomsanpham.Where(x => x.nhomcha == null).ToList();
            }
            else
            {
                 list = root.ds_nhomcon;
            }
            
            if (list == null) return new List<NhomSanPham2>();
            foreach (NhomSanPham item in list)
            {
                //add child đang xét vô cùng với level
                NhomSanPham2 tmp = new NhomSanPham2();
                tmp.Load_From(item);//load id, ten, ...
                tmp.level = level;
                this._tmp_for_get_tree.Add(tmp);
                //call recursive for this child
                this._get_tree(item, level+1);
            }
            //finish
            return this._tmp_for_get_tree;
        }
        public List<NhomSanPham2> get_tree(NhomSanPham root=null, int level=0)
        {
            this._tmp_for_get_tree = new List<NhomSanPham2>();
            List<NhomSanPham2> tmp = this._get_tree(root,0);
            this._tmp_for_get_tree = new List<NhomSanPham2>();
            return tmp;
        }
        public List<NhomSanPham2> timkiem(String id="", String ten="", String mota="", String active="")
        {
            List<NhomSanPham2> list = this.get_tree(null, 0);
            if (!id.Equals(""))
            {
                //find by id
                int id_i = TextLibrary.ToInt(id);
                list = list.Where(x => x.id == id_i).ToList();
                if (list == null)
                {
                    list = new List<NhomSanPham2>();
                }
                return list;
            }
            if (list == null)
            {
                list = new List<NhomSanPham2>();
            }
            //find by LIKE element
            list = list.Where(x => x.ten.Contains(ten)
                && x.mota.Contains(mota)).ToList();
            if (list == null)
            {
                list = new List<NhomSanPham2>();
            }
            
            //Filter again by by active
            if (!active.Equals(""))
            {
                Boolean active_b = TextLibrary.ToBoolean(active);
                list = list.Where(x => x.active == active_b).ToList();
            }
            if (list == null)
            {
                list = new List<NhomSanPham2>();
            }
            //FINAL return
            return list;
        }
    }
}
