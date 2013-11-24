using qdtest._Library;
using qdtest.Controllers.ModelController;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class FrontRegisterController : FrontController
    {
        [HttpGet]
        public ActionResult Index()
        {
            //xét nếu khách hàng đã đăng nhập thì đưa qua trang home
            if (this._is_logged_in())
            {
                return RedirectToAction("Index","FrontHome");
            }
            ViewBag.khachhang_register = new KhachHang();
            ViewBag.State = new List<string>();
            return View();
        }
        [HttpGet]
        public ActionResult CaptchaImage() 
        {
            bool noisy = true;

            var rand = new Random((int)DateTime.Now.Ticks); 
            //generate new question 
            int a = rand.Next(10, 90); 
            int b = rand.Next(0, 9); 
            var captcha = string.Format("{0} + {1} = ?", a, b); 
 
            //store answer 
            Session["frontregister_captcha"] = a + b;//string
 
            //image stream 
            FileContentResult img = null; 
 
            using (var mem = new MemoryStream()) 
            using (var bmp = new Bitmap(130, 30)) 
            using (var gfx = Graphics.FromImage((Image)bmp)) 
            { 
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit; 
                gfx.SmoothingMode = SmoothingMode.AntiAlias; 
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height)); 
 
                //add noise 
                if (noisy) 
                { 
                    int i, r, x, y; 
                    var pen = new Pen(Color.Yellow); 
                    for (i = 1; i < 10; i++) 
                    { 
                        pen.Color = Color.FromArgb( 
                        //(rand.Next(0, 255)), 
                        //(rand.Next(0, 255)), 
                        //(rand.Next(0, 255))
                        224,
                        224,
                        224
                        ); 
 
                        r = rand.Next(0, (130 / 3)); 
                        x = rand.Next(0, 130); 
                        y = rand.Next(0, 30); 
 
                        gfx.DrawEllipse(pen, x -r , y-r, r, r); 
                    } 
                } 
 
                //add question 
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Black, 2, 3); 
 
                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg); 
                img = this.File(mem.GetBuffer(), "image/Jpeg"); 
            } 
 
            return img; 
        }
        [HttpPost]
        public ActionResult Submit()
        {
            KhachHangController ctr = new KhachHangController();
            //get post value
            string tendangnhap = TextLibrary.ToString(Request["khachhang_tendangnhap"]);
            string tendaydu = TextLibrary.ToString(Request["khachhang_tendaydu"]);
            string matkhau = TextLibrary.ToString(Request["khachhang_matkhau"]);
            string matkhau2 = TextLibrary.ToString(Request["khachhang_matkhau2"]);
            string diachi = TextLibrary.ToString(Request["khachhang_diachi"]);
            string sdt = TextLibrary.ToString(Request["khachhang_sdt"]);
            string email = TextLibrary.ToString(Request["khachhang_email"]);
            string captcha = TextLibrary.ToString(Request["khachhang_captcha"]);
            //pass to obj
            KhachHang obj = new KhachHang();
            obj.diachi = diachi;
            obj.email = email;
            obj.matkhau = matkhau;
            obj.sdt = sdt;
            obj.tendangnhap = tendangnhap;
            obj.tendaydu = tendaydu;

            //validate
            List<string> validate = new List<string>();
            //xét captcha trước
            if (!this.get_captcha_string().ToLower().Equals(captcha.ToLower()))
            {
                validate.Add("captcha_fail");
            }
            //validate obj
            validate.AddRange(ctr.validate(obj, matkhau, matkhau2));
            //check
            if (validate.Count == 0)
            {
                //call update loaikh first
                obj._Update_LoaiKhachHang(ctr._db);
                //tiến hành thêm và gán session auto đăng nhập
                int max_id = ctr.add(obj);
                obj.id = max_id;
                //save to session
                Session["khachhang"] = ctr.get_by_id(max_id);
                //đăng ký thành công
                //nếu được dẫn link từ FrontCart.CheckOut thì quay lại checkOut
                if (Session["link_after_login"] != null)
                {
                    string url_to = (string)Session["link_after_login"];
                    Session["link_after_login"] = null;
                    return Redirect(url_to);
                }
                return RedirectToAction("Index", "FrontHome");
            }
            //add and redirect or return error
            
           //set tmp validate
            ViewBag.State = validate;
            ViewBag.khachhang_register = obj;
           return View("Index");

        }
        [NonAction]
        protected string get_captcha_string()
        {
            if (Session["frontregister_captcha"] != null)
            {
                return Session["frontregister_captcha"].ToString();
            }
            return "QWERDF32423434GHYUSDVBXYUI12389gsdf7723jbcxv09892gvvzvxgt23";
        }
    }
}
