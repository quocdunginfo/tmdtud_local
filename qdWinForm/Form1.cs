using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using qdtest.Models;
using qdtest.Controllers.ModelController;

namespace qdWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BanGiayDBContext db = new BanGiayDBContext();
            NhanVienController ctr=new NhanVienController();
            List<NhanVien> list = ctr.timkiem();
            if(list!=null)
            {
                this.listBox1.Items.Clear();
                foreach (NhanVien item in list)
                {
                    this.listBox1.Items.Add(item.tendangnhap);
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String username = this.listBox1.SelectedItem.ToString();
            NhanVienController ctr = new NhanVienController();
            ctr._db.ds_nhanvien.Remove(ctr._db.ds_nhanvien.Where(x => x.tendangnhap == username).FirstOrDefault());
            ctr._db.SaveChanges();
            this.button1_Click(sender, e);
        }
    }
}
