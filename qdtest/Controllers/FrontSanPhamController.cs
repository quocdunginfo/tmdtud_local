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
    public class FrontSanPhamController : FrontController
    {
        public SanPhamController ctr = new SanPhamController();
        protected HttpCookie front_sanpham=null;
        public ActionResult Index(int page = 1,int id_loaisp=0,int level_loaisp=0)
        {
            ViewBag.id = 2;
            NhomSanPhamController ctr2 = new NhomSanPhamController(ctr._db);
            List<NhomSanPham> loaisp_list = new List<NhomSanPham>();
            NhomSanPham2 loaisp = new NhomSanPham2();
            List<List<SanPham>> splist = new List<List<SanPham>>();
            if (id_loaisp == 0 || level_loaisp == 0)
            {
                NhomSanPham tmp;
                if (id_loaisp != 0)
                {
                    tmp = ctr2.get_by_id(id_loaisp);
                    loaisp.Load_From(tmp);
                    loaisp.level = level_loaisp;
                    foreach (NhomSanPham item in tmp.ds_nhomcon)
                    {
                        loaisp_list.Add(item);
                        splist.Add(ctr.timkiem_dequy(item, "1", 0, 3));
                    }
                }
                else
                {
                    tmp = null; loaisp = null;
                    List<NhomSanPham2> loaisp_l = ctr2.get_tree(tmp, 0);
                    foreach (NhomSanPham2 item in loaisp_l)
                    {
                       // if ((loaisp != null && loaisp.id != item.id && item.level == 1) || (loaisp == null && item.level == 0))
                        if(item.level==0&&item.active==true)
                        {
                            NhomSanPham a = ctr2.get_by_id(item.id);
                            loaisp_list.Add(a);
                            splist.Add(ctr.timkiem_dequy(a, "1", 0, 3));
                        }
                        
                    }

                   
                }
                ViewBag.splist = splist;
                ViewBag.loaisp = loaisp;
                ViewBag.loaisp_list = loaisp_list;
            }
            else
            {
                String orderby = TextLibrary.ToString(front_sanpham["front_orderby"]);
                Boolean desc = TextLibrary.ToBoolean(front_sanpham["front_desc"]);  
                NhomSanPham tmp = ctr2.get_by_id(id_loaisp);
                loaisp.Load_From(tmp);
                loaisp.level = level_loaisp;
                foreach(var item in ctr2.get_tree2(tmp))
                {
                    if (item.active == true)
                        loaisp_list.Add(item);
                }
                //calculate offset
                if (TextLibrary.ToString(Request["front_current_page"]) != "") page = int.Parse(TextLibrary.ToString(Request["front_current_page"]));
                int max_item_per_page = TextLibrary.ToInt(this.front_sanpham["front_max_item_per_page"]);
                int start_point = (page - 1) * max_item_per_page;
                if (start_point <= 0) start_point = 0;
                //get list
                List<SanPham> listnew = ctr.timkiem("", "", "", "", -1, -1, null, loaisp_list, "1", orderby, desc, start_point, max_item_per_page);
                ViewBag.SanPham_List = listnew;
                ViewBag.front_sanpham = this.front_sanpham;
                ViewBag.loaisp = loaisp;
                //pagination
                int Current_Page = page;
                //   int Result_Count = listnew.Count;
                int Result_Count = ctr.timkiem_count("", "", "", "", -1, -1, null, loaisp_list, "1");
                int Total_Page = (int)(Math.Ceiling((double)Result_Count / max_item_per_page));
                Boolean CanNextPage = Current_Page >= Total_Page ? false : true;
                Boolean CanPrevPage = Current_Page == 1 ? false : true;
                ViewBag.Result_Count = Result_Count;
                ViewBag.CanNextPage = CanNextPage;
                ViewBag.CanPrevPage = CanPrevPage;
                ViewBag.Current_Page = Current_Page;
                ViewBag.Total_page = Total_Page;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Submit(int id,int level)
        {
            //get search value
            if (!TextLibrary.ToString(Request["front_submit_reset"]).Equals(""))
            {
                //reset button click
                this.khoitao_cookie();
            }
            else
            {
                //search button click
                //this.front_sanpham["front_nhomsanpham_id"] = TextLibrary.ToString(Request["front_sanpham_nhomsanpham_id"]);
                this.front_sanpham["front_max_item_per_page"] = TextLibrary.ToString(Request["front_sanpham_max_item_per_page"]);
                this.front_sanpham["front_orderby"] = TextLibrary.ToString(Request["front_sanpham_orderby"]);
                this.front_sanpham["front_desc"] = TextLibrary.ToString(Request["front_sanpham_desc"]); ;
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.front_sanpham));
            //redirect
            return RedirectToAction("Index", "FrontSanPham", new {id_loaisp=id,level_loaisp=level });
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
            if (Request.Cookies.Get("front_sanpham") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                this.luu_cookie();
            }
            else
            {
                try
                {
                    this.front_sanpham = CookieLibrary.Base64Decode(Request.Cookies.Get("front_sanpham"));
                }
                catch (Exception ex)
                {
                    this.khoitao_cookie();
                    this.luu_cookie();
                }
            }
            ViewBag.front_sanpham = this.front_sanpham;
        }
        [NonAction]
        protected void luu_cookie()
        {
            if (this.front_sanpham != null)
            {
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.front_sanpham));
            }
        }
        [NonAction]
        protected void khoitao_cookie()
        {
            front_sanpham = new HttpCookie("front_sanpham");
            this.front_sanpham["front_ten"] = "";
            this.front_sanpham["front_masp"] = "";
            this.front_sanpham["front_mota"] = "";
            this.front_sanpham["front_gia_from"] = "-1";
            this.front_sanpham["front_gia_to"] = "-1";
            this.front_sanpham["front_hangsx_ten"] = "";
            this.front_sanpham["front_nhomsanpham_id"] = "";
            this.front_sanpham["front_max_item_per_page"] = "6";
            this.front_sanpham["front_orderby"] = "id";
            this.front_sanpham["front_desc"] = "1";
        }

        }

    
}