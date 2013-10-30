using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangBanGiay.Controllers;
using qdtest.Models;
using qdtest._Library;

namespace CuaHangBanGiay.Controllers
{
    public class HinhAnhController : Controller
    {
        //
        // GET: /HinhAnh/
        private BanGiayDBContext db;
        public HinhAnhController(BanGiayDBContext db)
        {
            this.db = db;
        }
        public HinhAnhController()
        {
            this.db = new BanGiayDBContext();
        }
        public HinhAnh get_by_id(int id)
        {
            return db.ds_hinhanh.FirstOrDefault(x => x.id == id);
        }
        public Boolean is_exist(int id)
        {
            HinhAnh u = (from kt in db.ds_hinhanh
                           where kt.id == id
                           select kt).FirstOrDefault();
            return u == null ? false : true;
        }
        public Boolean edit(HinhAnh kt)
        {
            if (this.is_exist(kt.id))
            {
                HinhAnh obj = this.get_by_id(kt.id);
                obj.duongdan = kt.duongdan;
                db.SaveChanges();
                return true;
            }
            return false;
        } 
        public int add(HinhAnh dt)
        {
            db.ds_hinhanh.Add(dt);
            db.SaveChanges();
            return db.ds_hinhanh.Max(x => x.id);
        }
        public Boolean delete(int id)
        {
            HinhAnh kq = db.ds_hinhanh.Where(x => x.id == id).FirstOrDefault();
            if (kq == null) return false;
            db.ds_hinhanh.Remove(kq);
            db.SaveChanges();
            return true;
        }

    }
}
