using qdtest.Controllers.ModelController;
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
            List<NhomSanPham> list = new NhomSanPhamController().get_tree2(null);
            foreach (NhomSanPham item in list)
            {
                Debug.WriteLine(item.id);
                foreach (SanPham sp in item.ds_sanpham)
                {
                    Debug.Write(sp.id+""+ sp.ten+"-");
                }
                Debug.WriteLine("");
            }
            return View();
        }

    }
}
