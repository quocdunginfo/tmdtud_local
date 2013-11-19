using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest.Controllers.ModelController;
using qdtest.Models;
using qdtest._Library;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using qdtest.Controllers.ModelController;

namespace qdtest.Controllers.ModelController
{
    public class TinhTPController
    {
        private BanGiayDBContext _db;
        public TinhTPController(BanGiayDBContext db)
        {
            this._db = db;
        }
        public TinhTPController()
        {
            this._db = new BanGiayDBContext();
        }
        public TinhTP get_by_id(int id)
        {
            return _db.ds_tinhtp.FirstOrDefault(x => x.id == id);
        }
        public Boolean is_exist(int id)
        {
            TinhTP u = (from kt in _db.ds_tinhtp
                           where kt.id == id
                           select kt).FirstOrDefault();
            return u == null ? false : true;
        }
        public List<TinhTP> get_all()
        {
            return this._db.ds_tinhtp.ToList();
        }
    }
}
