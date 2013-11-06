using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest.Controllers;
using qdtest.Models;
using qdtest._Library;
using qdtest.Controllers.ModelController;

namespace qdtest.Controllers
{
    public class FrontSanPhamDetailController : FrontController
    {
        //
        // GET: /FrontSanPhamDetail/

        public ActionResult Index(int id)
        {
            SanPhamController ctr = new SanPhamController();
            SanPham sp = new SanPham();
            sp=ctr.get_by_id(id);
            if (sp != null)
            {
                ViewBag.Title = sp.ten;
                ViewBag.SanPhamDetail = sp;
                List<NhomSanPham> a = new List<NhomSanPham>();
                a.Add(sp.nhomsanpham);
                List<SanPham> SanPhamRelate = ctr.timkiem("", "", "", "", -1, -1, null, a, "1", "id", true, 0, 3);
                if (SanPhamRelate != null) ViewBag.SanPhamRelate = SanPhamRelate;
                else ViewBag.SanPhamRelate = new List<SanPham>();
                return View();
            }
            else return View("404");
        }

    }
}
