using qdtest._Library;
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
    public class AdminNhomSanPhamController : AdminController
    {
        //
        // GET: /AdminNhomSanPham/
        public ActionResult Index()
        {
            NhomSanPham root = this._db.ds_nhomsanpham.Where(x => x.id == 1).FirstOrDefault();
            ViewBag.NhomSanPham_List = this._db.ds_nhomsanpham.ToList();
            ViewBag.Title += " - View";
            return View();
        }
        [HttpPost]
        public ActionResult Submit()
        {
            //get search
            return View();
        }
        [HttpPost]
        public ActionResult Add()
        {
            //get data
            int p_id = TextLibrary.ToInt(Request["cat_parent_id"]);
            //nên lấy instance của chính Controller đó để dùng sẽ không bị lỗi track changes
            NhomSanPhamController controller = new NhomSanPhamController();

            NhomSanPham obj = new NhomSanPham();
            obj.ten = Request["cat_name"];
            if (obj.ten.Equals(""))
            {
                //lỗi
                Debug.WriteLine("Lỗi cat_name= rỗng");
                return RedirectToAction("Index", "AdminNhomSanPham");
            }
            NhomSanPham p_obj = controller._db.ds_nhomsanpham.Where(x => x.id == p_id).FirstOrDefault();
            if (p_obj == null)
            {
                //lỗi
                Debug.WriteLine("Lỗi cat_parent_obj= null (id=)"+p_id);
                return RedirectToAction("Index","AdminNhomSanPham");
            }
            obj.nhomcha = p_obj;
            //call add
            controller.add(obj);
            Debug.WriteLine("Thêm Thành Công Nhóm Sản Phẩm");
            return RedirectToAction("Index","AdminNhomSanPham");
        }
        public ActionResult Delete(int id=0)
        {
            NhomSanPhamController controller = new NhomSanPhamController();
            if (!controller.is_exist(id))
            {
                Debug.WriteLine("Delete Nhóm sản phẩm id không tồn tại");
                return RedirectToAction("Index", "AdminNhomSanPham");
            }
            controller.delete(id);
            Debug.WriteLine("Delete Nhóm sản phẩm thành công (xóa luôn con)");
            return RedirectToAction("Index", "AdminNhomSanPham");
        }

    }
}
