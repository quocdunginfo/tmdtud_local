using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest.Controllers.ModelController;
using qdtest.Models;

namespace CuaHangBanGiay.Controllers.View_Controller
{
    public class FrontController : Controller
    {
        //
        // GET: /Layout/

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NhomSanPhamController ctr = new NhomSanPhamController();
            ViewBag.NhomSanPham2_List_All = ctr.timkiem("", "", "", "");
           // var list = ctr._db.Database.SqlQuery<int>("drop table table2 select ChiTietSPs.sanpham_id,sum(ChiTiet_DonHang.soluong) as sl into table2 from ChiTiet_DonHang inner join ChiTietSPs on ChiTiet_DonHang.chitietsp_id=ChiTietSPs.id group by ChiTietSPs.sanpham_id order by sl DESC select sanpham_id from table2 order by sl  DESC").ToList();
            var list = (from p in ctr._db.ds_sanpham
                       join c in
                           ((from ChiTiet_DonHang in ctr._db.ds_chitiet_donhang
                             group new { ChiTiet_DonHang.chitietsp, ChiTiet_DonHang } by new
                             {
                                 Sanpham_id = (int?)ChiTiet_DonHang.chitietsp.sanpham.id,
                             } into g
                             select new
                             {
                                 Sanpham_id = (int?)g.Key.Sanpham_id,
                                 sl = (int?)g.Sum(p => p.ChiTiet_DonHang.soluong),
                             }).OrderByDescending(x => x.sl)) on p.id equals c.Sanpham_id
                       select new
                       {
                           sp=p,
                           sl=c.sl
                       }).OrderByDescending(x=>x.sl);
            foreach (var item in list)
            {
                Debug.WriteLine("Kim =" + item.sp.id + "//loai="+item.sp.nhomsanpham.ten+"//sl="+item.sl);
            }
        }

    }
}
