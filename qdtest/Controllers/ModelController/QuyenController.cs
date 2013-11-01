using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;

namespace qdtest.Controllers.ModelController
{
    public class QuyenController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public QuyenController()
        {

        }
        public QuyenController(BanGiayDBContext db)
        {
            this._db = db;
        }
        public Quyen get_by_id(int obj_id)
        {
            return _db.ds_quyen.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean is_exist(int obj_id)
        {
            return this.get_by_id(obj_id)== null ? false : true;
        }
        public List<Quyen> list_id_to_list_obj(List<int> id_list)
        {
            return this._db.ds_quyen.Where(x => id_list.Contains(x.id)).ToList();
            /* RUN OK as above code
            List<Quyen> re = new List<Quyen>();
            foreach (var item in id_list)
            {
                Quyen tmp = this.get_by_id(item);
                if (tmp != null)
                {
                    re.Add(tmp);
                }
            }
            return re;
            */
        }
    }
}