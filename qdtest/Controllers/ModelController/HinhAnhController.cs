using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangBanGiay.Controllers;
using qdtest.Models;
using qdtest._Library;
using System.Drawing;
using System.IO;
using System.Diagnostics;

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
        public List<HinhAnh> Upload(HttpServerUtilityBase server_context, HttpFileCollectionBase file_list)
        {
            Debug.WriteLine("file count: "+file_list.Count);
            List<HinhAnh> re=new List<HinhAnh>();
            //pre setting
            int max_width_height = 300;
            String relative_directory = "~/_Upload/HinhAnh/";
            //
            String server_path = "";
            String server_path_thumb = "";
            foreach (string file_name in file_list)
            {
                Debug.WriteLine("file name: " + file_name);
                HttpPostedFileBase hpf = file_list[file_name];
                server_path = server_context.MapPath(Path.Combine(relative_directory, Path.GetFileName(hpf.FileName)));
                
                server_path_thumb = server_context.MapPath(Path.Combine(relative_directory, "_thumb_" + Path.GetFileName(hpf.FileName)));
                if (hpf.ContentLength == 0)
                {
                    continue;
                }
                HinhAnh hinhanh = new HinhAnh();
                //save origin
                    hpf.SaveAs(server_path);
                    hinhanh.duongdan = hpf.FileName;
                //save thumb
                    Image imgOriginal = Image.FromFile(server_path);
                    Image hinhanh_thumb = ImageLibrary.ScaleBySize(imgOriginal, max_width_height);
                    imgOriginal.Dispose();
                    hinhanh_thumb.Save(server_path_thumb);
                    hinhanh_thumb.Dispose();
                    hinhanh.duongdan_thumb = "_thumb_" + hpf.FileName;
                //add to re
                    re.Add(hinhanh);
                Debug.WriteLine("uploaded: " + server_path);
                Debug.WriteLine("uploaded: " + server_path_thumb);
            }
            return re;
        }
    }
}
