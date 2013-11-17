using CuaHangBanGiay.Controllers;
using qdtest._Library;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qdtest.Controllers
{
    public class FrontCartController : FrontController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.giohang = this._giohang;
            return View();
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Remove(int chitietsp_id)
        {
            ChiTiet_DonHang obj= this._giohang.ds_chitiet_donhang.Where(x => x.chitietsp.id == chitietsp_id).FirstOrDefault();
            if (obj != null)
            {
                this._giohang.ds_chitiet_donhang.Remove(obj);
                this._save_cart_to_session();
            }
            return RedirectToAction("Index", "FrontCart");
        }
        [HttpPost]
        public ActionResult Add_Or_Update()
        {
            ChiTietSPController ctr= new ChiTietSPController();
            Boolean update_from_cart = TextLibrary.ToBoolean(Request["giohang_update_from_cart"]);
            int chitietsp_id = TextLibrary.ToInt(Request["giohang_chitietsp_id"]);
            //validate id
            if (!ctr.is_exist(chitietsp_id))
            {
                return RedirectToAction("Index","FrontHome");
            }
            //get soluong
            int soluong = TextLibrary.ToInt(Request["giohang_soluong"]);
            Debug.WriteLine("soluong:" + soluong);
            //get obj
            ChiTiet_DonHang obj = this._giohang.ds_chitiet_donhang.Where(x => x.chitietsp.id == chitietsp_id).FirstOrDefault();
            
            //add or update
            if (obj == null)
            {
                //chưa có trong cart => thêm vào
                ChiTiet_DonHang tmp = new ChiTiet_DonHang();
                tmp.chitietsp = new ChiTietSPController().get_by_id(chitietsp_id);
                tmp.dongia = tmp.chitietsp.sanpham.gia;
                tmp.soluong = soluong;
                //filter
                    if (tmp.soluong > 3)
                    {
                        tmp.soluong = 3;
                    }
                    if (tmp.soluong <= 0)
                    {
                        tmp.soluong = 1;
                    }
                //add to giohang
                this._giohang.ds_chitiet_donhang.Add(tmp);
            }
            else
            {
                //đã có trong cart
                if (update_from_cart)
                {
                    //cập nhật từ giỏ hàng
                    obj.soluong = soluong;
                }
                else
                {
                    //thêm từ nơi khác
                    obj.soluong += soluong;
                }
                //filter
                    if (obj.soluong > 3)
                    {
                        obj.soluong = 3;
                    }
                    if (obj.soluong <= 0)
                    {
                        obj.soluong = 1;
                    }
            }
            //filter soluong
            
            //finally call save
            this._save_cart_to_session();
            return RedirectToAction("Index","FrontCart");
        }

    }
}
