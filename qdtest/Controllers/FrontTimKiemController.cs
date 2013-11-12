using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;

namespace qdtest.Controllers
{
    public class FrontTimKiemController : FrontController
    {
        public SanPhamController ctr = new SanPhamController();
        public ActionResult Index(int page=1)
        {
            List<NhomSanPham> nhomsp = null;
            List<HangSX> hangsx = null; 
            if(!front_timkiem_sanpham["front_nhomsanpham_ten"].Equals(""))
            {
                nhomsp = new List<NhomSanPham>();
                NhomSanPhamController ctr2 = new NhomSanPhamController(ctr._db);
                NhomSanPham tmp = ctr2.get_by_id(TextLibrary.ToInt(front_timkiem_sanpham["front_nhomsanpham_ten"]));
                nhomsp = ctr2.get_tree2(tmp);
                //List<NhomSanPham2> nhomsp2 = ctr2.timkiem("", TextLibrary.ToString(front_timkiem_sanpham["front_nhomsanpham_ten"]), "", "1");
                //if (nhomsp2 != null)
                //{
                //    foreach (NhomSanPham2 item in nhomsp2)
                //    {
                //        NhomSanPham a = ctr2.get_by_id(item.id);
                //        nhomsp.Add(a);
                //    }
                //}
               // nhomsp.Add(ctr2.get_by_id(front_timkiem_sanpham["front_nhomsanpham_ten"]
            }
            if(!front_timkiem_sanpham["front_hangsx_ten"].Equals(""))
            {
                hangsx = new List<HangSX>();
                HangSXController ctr3 = new HangSXController(ctr._db);
                hangsx=ctr3.timkiem("",TextLibrary.ToString(front_timkiem_sanpham["front_hangsx_ten"]),"1","id",false,0,-1);
            }
            //calculate offset
            if (TextLibrary.ToString(Request["front_current_page"]) != "") page = int.Parse(TextLibrary.ToString(Request["front_current_page"]));
            int max_item_per_page = TextLibrary.ToInt(this.front_timkiem_sanpham["front_max_item_per_page"]);
                int start_point = (page - 1) * max_item_per_page;
                if (start_point <= 0) start_point = 0;
            //get list
                List<SanPham> listnew = ctr.timkiem("", "", this.front_timkiem_sanpham["front_ten"], front_timkiem_sanpham["front_mota"],  TextLibrary.ToInt(front_timkiem_sanpham["front_gia_from"]),
                    TextLibrary.ToInt(front_timkiem_sanpham["front_gia_to"]), hangsx, nhomsp , "1", "id", true, start_point, max_item_per_page);
                ViewBag.SanPham_List = listnew;
                ViewBag.front_timkiem_sanpham = this.front_timkiem_sanpham;
            //pagination
                int Current_Page = page;
             //   int Result_Count = listnew.Count;
                int Result_Count = ctr.timkiem_count(
                    "",
                    front_timkiem_sanpham["front_masp"],
                    front_timkiem_sanpham["front_ten"],
                    front_timkiem_sanpham["front_mota"],
                    TextLibrary.ToInt(front_timkiem_sanpham["front_gia_from"]),
                    TextLibrary.ToInt(front_timkiem_sanpham["front_gia_to"]), hangsx, nhomsp, "1");
                int Total_Page = (int)(Math.Ceiling((double)Result_Count / max_item_per_page));
                Boolean CanNextPage = Current_Page >= Total_Page ? false : true;
                Boolean CanPrevPage = Current_Page == 1 ? false : true;
                ViewBag.Result_Count = Result_Count;
                ViewBag.CanNextPage = CanNextPage;
                ViewBag.CanPrevPage = CanPrevPage;
                ViewBag.Current_Page = Current_Page;
                ViewBag.Total_page = Total_Page;
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get search value
            if (!TextLibrary.ToString(Request["front_submit_reset"]).Equals(""))
            {
                //reset button click
                this._khoitao_cookie();
            }
            else
            {
                //search button click
                this.front_timkiem_sanpham["front_masp"] = TextLibrary.ToString(Request["front_sanpham_masp"]);
                this.front_timkiem_sanpham["front_ten"] = TextLibrary.ToString(Request["front_sanpham_ten"]);
                this.front_timkiem_sanpham["front_mota"] = TextLibrary.ToString(Request["front_sanpham_mota"]);
                this.front_timkiem_sanpham["front_gia_from"] = TextLibrary.ToString(Request["front_sanpham_gia_from"]);
                this.front_timkiem_sanpham["front_gia_to"] = TextLibrary.ToString(Request["front_sanpham_gia_to"]);
                this.front_timkiem_sanpham["front_hangsx_ten"] = TextLibrary.ToString(Request["front_sanpham_hangsx_ten"]);
                this.front_timkiem_sanpham["front_nhomsanpham_ten"] = TextLibrary.ToString(Request["front_sanpham_nhomsanpham_ten"]);
                this.front_timkiem_sanpham["front_max_item_per_page"] = TextLibrary.ToString(Request["front_sanpham_max_item_per_page"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.front_timkiem_sanpham));
            //redirect
            return RedirectToAction("Index", "FrontTimKiem");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NhomSanPhamController ctr = new NhomSanPhamController();
            List<NhomSanPham2> list1 = ctr.timkiem("", "", "", "1");
            ViewBag.loaisp = list1;
            HangSXController ctr2 = new HangSXController(ctr._db);
            List<HangSX> list2 = ctr2.timkiem("", "", "1", "id", false, 0, -1);
            ViewBag.hangsx = list2;
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Tìm kiếm sản phẩm";
        }

    }
}
