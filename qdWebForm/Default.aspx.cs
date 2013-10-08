using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using qdtest.Models;
using qdtest.Controllers.ModelController;

namespace qdWebForm
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //load data
            BanGiayDBContext db = new BanGiayDBContext();
            NhanVienController ctr=new NhanVienController();
            List<NhanVien> list;// = db.ds_nhanvien.ToList();
            list = ctr.timkiem();
            if (list != null)
            {
                this.ListBox_nhanvien.Items.Clear();
                foreach(NhanVien item in list)
                {
                    this.ListBox_nhanvien.Items.Add(new ListItem(item.tendangnhap, item.id+""));
                }
                

            }
        }
    }
}