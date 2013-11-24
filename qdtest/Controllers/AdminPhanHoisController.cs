using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class AdminPhanHoisController : AdminController
    {
        private HttpCookie timkiem_phanhoi;
        public AdminPhanHoisController()
        {
            this.khoitao_cookie();
        }
        [NonAction]
        private void khoitao_cookie()
        {
            timkiem_phanhoi = new HttpCookie("timkiem_phanhoi");
            timkiem_phanhoi.Expires = DateTime.Now.AddDays(1);
            this.timkiem_phanhoi["id"] = "";
            this.timkiem_phanhoi["tieude"] = "";
            this.timkiem_phanhoi["noidung"] = "";
            this.timkiem_phanhoi["nguoigui_ten"] = "";
            this.timkiem_phanhoi["nguoigui_email"] = "";
            this.timkiem_phanhoi["max_item_per_page"] = "5";
        }
        [HttpGet]
        public ActionResult Index(int page=1, int phanhoi_id=0)
        {
            //check
            if (!this._nhanvien_permission.Contains("phanhoi_view"))
            {
                return this._fail_permission("phanhoi_view");
            }
            PhanHoiController ctr = new PhanHoiController();
            //pagination
                int max_item_per_page = TextLibrary.ToInt(this.timkiem_phanhoi["max_item_per_page"]);
                Pagination pg = new Pagination();
                pg.set_current_page(page);
                pg.set_max_item_per_page(max_item_per_page);
                pg.set_total_item(
                    ctr.timkiem_count(
                        timkiem_phanhoi["id"],
                        timkiem_phanhoi["tieude"],
                        timkiem_phanhoi["noidung"],
                        timkiem_phanhoi["nguoigui_ten"],
                        timkiem_phanhoi["nguoigui_email"]
                        )
                    );
                pg.update();
            //Chọn danh sách để hiển thị theo cookies tìm kiếm
            
            ViewBag.PhanHoi_List = ctr.timkiem(
                timkiem_phanhoi["id"],
                timkiem_phanhoi["tieude"],
                timkiem_phanhoi["noidung"],
                timkiem_phanhoi["nguoigui_ten"],
                timkiem_phanhoi["nguoigui_email"],
                "", "id", true, pg.start_point, pg.max_item_per_page
                );
            //set search cookies
            ViewBag.timkiem_phanhoi = this.timkiem_phanhoi;
            ViewBag.phanhoi  = ctr.is_exist(phanhoi_id) ? ctr.get_by_id(phanhoi_id) : new PhanHoi();
            
            ViewBag.Title += " - Quản lý";
            ViewBag.State = TempData["state"] == null ? new List<string>() : (List<string>)TempData["state"];
            ViewBag.pagination = pg;
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id=0)
        {
            //check
            if (!this._nhanvien_permission.Contains("phanhoi_delete"))
            {
                return this._fail_permission("phanhoi_delete");
            }
            PhanHoiController ctr = new PhanHoiController();
            if (!ctr.is_exist(id))
            {
                return RedirectToAction("Index", "AdminPhanHois");
            }
            //delete
            ctr.delete(id);
            return RedirectToAction("Index","AdminPhanHois");
        }
        [HttpPost]
        public ActionResult Submit()
        {
            
            //get search value
            if (!TextLibrary.ToString(Request["submit_reset"]).Equals(""))
            {
                //reset button click
                this.khoitao_cookie();
            }
            else
            {
                //search button click
                this.timkiem_phanhoi["id"] = TextLibrary.ToString(Request["phanhoi_id"]);
                this.timkiem_phanhoi["tieude"] = TextLibrary.ToString(Request["phanhoi_tieude"]);
                this.timkiem_phanhoi["noidung"] = TextLibrary.ToString(Request["phanhoi_noidung"]);
                this.timkiem_phanhoi["nguoigui_ten"] = TextLibrary.ToString(Request["phanhoi_nguoigui_ten"]);
                this.timkiem_phanhoi["nguoigui_email"] = TextLibrary.ToString(Request["phanhoi_nguoigui_email"]);
                this.timkiem_phanhoi["max_item_per_page"] = TextLibrary.ToString(Request["phanhoi_max_item_per_page"]);
            }
            //Save respone cookies
            Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_phanhoi));
            //redirect
            return RedirectToAction("Index", "AdminPhanHois");
        }
        [HttpGet]
        public ActionResult Reply(int id=0)
        {
            //check
            if (!this._nhanvien_permission.Contains("phanhoi_edit"))
            {
                return this._fail_permission("phanhoi_edit");
            }
            return RedirectToAction("Index", "AdminPhanHois", new { phanhoi_id = id });
        }
        [HttpPost]
        public ActionResult Reply_Submit()
        {
            //check
            if (!this._nhanvien_permission.Contains("phanhoi_edit"))
            {
                return this._fail_permission("phanhoi_edit");
            }
            //set nguoi xu ly phan hoi
                int phanhoi_id = TextLibrary.ToInt(Request["phanhoi_id"]);
                //
                PhanHoiController ctr = new PhanHoiController();
                PhanHoi obj = ctr.get_by_id(phanhoi_id);
                if (obj != null)
                {
                    obj.nhanvien = ctr._db.ds_nhanvien.Where(x => x.id == this._nhanvien.id).FirstOrDefault();
                    //lưu
                    ctr._db.SaveChanges();
                }

            //get value
            string email = TextLibrary.ToString(Request["email"]);
            string tieude = TextLibrary.ToString(Request["tieude"]);
            string noidung = TextLibrary.ToString(Request.Unvalidated["noidung"]);
            //send email
            GMailLibrary gmail = new GMailLibrary();
            gmail.receive_email = email;
            gmail.receive_title = tieude;
            gmail.receive_html = noidung;
            try
            {
                gmail.Send();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "AdminPhanHois");
            }
            TempData["state"] = (new string[] { "send_ok" }).ToList<string>();
            return RedirectToAction("Index", "AdminPhanHois");
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.Title += " - Phản hồi";

            //build timkiem
            if (Request.Cookies.Get("timkiem_phanhoi") == null)
            {
                //chưa set cookies trước => tiến hành set cookies
                this.khoitao_cookie();
                Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_phanhoi));
            }
            else
            {
                try
                {
                    this.timkiem_phanhoi = CookieLibrary.Base64Decode(Request.Cookies.Get("timkiem_phanhoi"));
                }
                catch (Exception)
                {
                    this.khoitao_cookie();
                    Response.Cookies.Add(CookieLibrary.Base64Encode(this.timkiem_phanhoi));
                }
            }

            //set active tab
            this._set_activetab(new String[] { "QuanLyKhachHang", "PhanHoi" });
        }
    }
}
