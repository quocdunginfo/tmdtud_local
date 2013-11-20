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
    public class FrontContactController : FrontController
    {
        //
        // GET: /FrontContact/
     //   List<string> valid;
        public ActionResult Index()
        {
            if (this._is_logged_in())
            {
                ViewBag.kh_info = this._khachhang;
            }
            else ViewBag.kh_info = new KhachHang();
         //   ViewBag.State = this.valid;
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

                         gfx.DrawEllipse(pen, x - r, y - r, r, r);
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
         public ActionResult Submit()
         {
                 PhanHoi obj = new PhanHoi();
                 KhachHang kh_info=new KhachHang();
            //get post value     
                string tendaydu = TextLibrary.ToString(Request["front_contact_author"]);
                string sdt = TextLibrary.ToString(Request["front_contact_phone"]);
                string email = TextLibrary.ToString(Request["front_contact_email"]);
                string noidung = TextLibrary.ToString(Request["front_contact_text"]);
                string captcha = TextLibrary.ToString(Request["front_contact_captcha"]);
            //pass to obj
            //note: chưa pass to obj đc do trong obj đòi hỏi khachhang login mà p.hồi chưa chắc có login
            //validate
            List<string> validate = new List<string>();
                //xét captcha trước
                if (!this.get_captcha_string().ToLower().Equals(captcha.ToLower()))
                {
                    validate.Add("captcha_fail");
                }
                if (!ValidateLibrary.is_valid_email(email)||email.Equals(""))
                {
                    validate.Add("email_fail");
                }
                if (tendaydu.Equals(""))
                {
                    validate.Add("author_fail");
                }
                if (noidung.Equals(""))
                {
                    validate.Add("text_fail");
                }
                sdt=sdt.Trim();
                double num;
                if (!double.TryParse(sdt,out num))
                {
                    validate.Add("phone_fail");
                }
                if (validate.Count == 0)
                {
                    validate.Add("success");
                }
            //    valid = validate;
                ViewBag.State = validate;
                kh_info.tendaydu = tendaydu;
                kh_info.sdt = sdt;
                kh_info.email = email;
                ViewBag.kh_info = kh_info;
                ViewBag.text = noidung;
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
