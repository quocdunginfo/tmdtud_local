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
        public ActionResult Index()
        {
            ViewBag.id = 5;
            PhanHoi obj = new PhanHoi();
            if(this._nhanvien!=null)
            {
                obj.nguoigui_ten = _nhanvien.tendaydu;
                obj.nguoigui_email = _nhanvien.email;
                obj.nguoigui_sdt = "";
            }
            else if(this._khachhang != null)
            {
                obj.khachhang = this._khachhang;
            }
            ViewBag.State = TempData["state"] == null ? new List<string>() : (List<string>)TempData["state"];
            ViewBag.phanhoi = obj;
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
             Session["frontcontact_captcha"] = a + b;//string

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
             PhanHoiController ctr = new PhanHoiController();
             PhanHoi obj = new PhanHoi();
            //get post value     
                string tendaydu = TextLibrary.ToString(Request["front_contact_author"]);
                string sdt = TextLibrary.ToString(Request["front_contact_phone"]);
                string email = TextLibrary.ToString(Request["front_contact_email"]);
                string tieude = TextLibrary.ToString(Request["front_contact_tieude"]);
                string noidung = TextLibrary.ToString(Request["front_contact_text"]);
                string captcha = TextLibrary.ToString(Request["front_contact_captcha"]);
            //pass to obj
                if (this._khachhang != null)
                {
                    obj.khachhang = ctr._db.ds_khachhang.Where(x => x.id == this._khachhang.id).FirstOrDefault();
                }
                else
                {
                    obj.nguoigui_email = email;
                    obj.nguoigui_sdt = sdt;
                    obj.nguoigui_ten = tendaydu;
                }
                obj.tieude = tieude;
                obj.noidung = noidung;
             //validate
                List<string> validate = ctr.validate(obj);
                //xét captcha
                if (!this.get_captcha_string().ToLower().Equals(captcha.ToLower()))
                {
                    validate.Add("captcha_fail");
                }
            //Check ok
                if (validate.Count == 0)
                {
                    ctr.add(obj);
                    validate.Add("success");
                }

                ViewBag.State = validate;
                ViewBag.phanhoi = obj;
                return View("Index");
         }
         [NonAction]
         protected string get_captcha_string()
         {
             if (Session["frontcontact_captcha"] != null)
             {
                 return Session["frontcontact_captcha"].ToString();
             }
             return "QWERDF32423434GHYUSDVBXYUI12389gsdf7723jbcxv09892gvvzvxgt23";
         }

    }
}
