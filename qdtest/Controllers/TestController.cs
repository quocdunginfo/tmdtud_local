using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            BanGiayDBContext db = new BanGiayDBContext();
            var re = from ChiTiet_DonHang in db.ds_chitiet_donhang
                     group new { ChiTiet_DonHang.chitietsp, ChiTiet_DonHang } by new
                     {
                         Sanpham_id = (int?)ChiTiet_DonHang.chitietsp.sanpham.id,
                         So_Luong = (int?)ChiTiet_DonHang.chitietsp.soluong
                     } into g
                     orderby
                       g.Key.So_Luong descending
                     select new
                     {
                         Sanpham_id = (int?)g.Key.Sanpham_id,
                         sl = (int?)g.Sum(p => p.ChiTiet_DonHang.soluong)
                     };
            foreach(var item in re)
            {
                Debug.WriteLine(item.Sanpham_id);
                Debug.WriteLine(item.sl);

            }
            return View();
        }

    }
}
