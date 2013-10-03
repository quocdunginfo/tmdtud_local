using qdtest._Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qdtest.Models
{
    public class NhomSanPham2
    {
        public int id { get; set; }
        public int level { get; set; }
        public String ten { get; set; }
        public String mota { get; set; }
        public Boolean active { get; set; }
        public NhomSanPham2()
        {
            this.level = 0;
            this.ten = "";
            this.mota = "";
            this.active = true;
            this.id = 0;
        }
        public void Load_From(NhomSanPham obj)
        {
            this.id = obj.id;
            //đề phòng null, 
            this.ten = TextLibrary.ToString(obj.ten);
            this.mota = TextLibrary.ToString(obj.mota);
            this.active = obj.active;
        }
    }
}