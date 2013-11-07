using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qdtest.Controllers.ModelController;
using qdtest.Models;
using qdtest.Controllers;
using qdtest._Library;
namespace qdtest.Controllers
{
    public class FrontHomeController : FrontController
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {
            ViewBag.SanPhamNew_List = spnew();
            return View();
        }
        public List<SanPham> spnew()
        {
            SanPhamController ctrl = new SanPhamController();
            List<SanPham> listnew = ctrl.timkiem("", "", "", "", -1, -1, null, null, "1", "id", true, 0, 6);
            if (listnew != null)
            {
                //foreach (var item in listnew)
                //{
                //    item.mota = sub(item.mota);    
                //}
                return listnew;
            }
            return new List<SanPham>();
              
        }
        private String sub(String s)
        {
            if (s.Equals("")) return s;
            //xóa khoảng trắng
              s = s.Trim();
            while (s.IndexOf("  ") >= 0)
               s= s.Replace("  "," ");
            int space = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i]==32) space++;
                if (i == s.Length - 1 && space < 15)
                {
                    if(!s.Substring(i-2,3).Equals("..."))
                    s += "...";
                    while (space != 15)
                    {
                        s += "      ";
                        space++;
                    }
                    return s;
                }
                if (space == 15) 
                { 
                    s = s.Substring(0,i); 
                    break; 
                }
            }
            if (!s.Substring(s.Length - 3, 3).Equals("..."))
                s += "...";
            return s;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //khi click lai trang chu thi tu dong xoa cookies
            this._khoitao_cookie();
            this._luu_cookie();

        }
    }
}
