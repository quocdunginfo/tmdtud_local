﻿using qdtest._Library;
using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace qdtest.Controllers.ModelController
{
    public class SanPhamController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public SanPhamController()
        {

        }
        public SanPhamController(BanGiayDBContext db)
        {
            this._db = db;
        }
        public List<SanPham> get_bestseller(int limit)
        {
            // var list = ctr._db.Database.SqlQuery<int>("drop table table2 select ChiTietSPs.sanpham_id,sum(ChiTiet_DonHang.soluong) as sl into table2 from ChiTiet_DonHang inner join ChiTietSPs on ChiTiet_DonHang.chitietsp_id=ChiTietSPs.id group by ChiTietSPs.sanpham_id order by sl DESC select sanpham_id from table2 order by sl  DESC").ToList();
            var list = (from p in _db.ds_sanpham
                        join c in
                            ((from ChiTiet_DonHang in _db.ds_chitiet_donhang
                              where !ChiTiet_DonHang.donhang.trangthai.Equals("dabihuy")
                              group new { ChiTiet_DonHang.chitietsp, ChiTiet_DonHang } by new
                              {
                                  Sanpham_id = (int?)ChiTiet_DonHang.chitietsp.sanpham.id,
                              } into g
                              select new
                              {
                                  Sanpham_id = (int?)g.Key.Sanpham_id,
                                  sl = (int?)g.Sum(p => p.ChiTiet_DonHang.soluong),
                              }).OrderByDescending(x => x.sl)) on p.id equals c.Sanpham_id
                        select new
                        {
                            sp = p,
                            sl = c.sl
                        }).OrderByDescending(x => x.sl);
            List<SanPham> listbest = new List<SanPham>();
            int i=1;
         foreach(var item in list)
         {
             if (i == limit) break;
                Debug.WriteLine("Kim =" + item.sp.id + "//tên="+item.sp.ten+"//loai=" + item.sp.nhomsanpham.ten + "//sl=" + item.sl);
                if (item.sp.active == true)
                {
                    listbest.Add(item.sp);
                    i++;
                }
         }
            return listbest;
        }
        public SanPham get_by_id(int obj_id)
        {
            return _db.ds_sanpham.FirstOrDefault(x => x.id == obj_id);
        }
        public SanPham get_by_masp(String masp)
        {
            return _db.ds_sanpham.FirstOrDefault(x => x.masp.ToUpper().Equals(masp.ToUpper()));
        }

        public Boolean is_exist(int obj_id)
        {
            SanPham obj = this.get_by_id(obj_id);
            return obj == null ? false : true;
        }
        public Boolean is_exist_masp(String masp)
        {
            SanPham obj = this.get_by_masp(masp);
            return obj == null ? false : true;
        }
        public int add(SanPham obj)
        {
            //call add
            this._db.ds_sanpham.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_sanpham.Max(x => x.id);
        }
        public Boolean delete(int obj_id)
        {
            //get entity
            SanPham obj = this.get_by_id(obj_id);
            //check null
            if (obj == null) return false;
            //remove
            this._db.ds_sanpham.Remove(obj);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String masp = "", String ten = "", String mota = "", int gia_from = 0, int gia_to = 0, List<HangSX> hangsx_list = null, List<NhomSanPham> nhomsanpham_list = null, String active = "")
        {
            return timkiem(id, masp, ten, mota, gia_from, gia_to, hangsx_list, nhomsanpham_list,active).Count;
        }
        public List<SanPham> timkiem(String id = "", String masp = "", String ten = "", String mota = "", int gia_from = 0, int gia_to = 0, List<HangSX> hangsx_list = null, List<NhomSanPham> nhomsanpham_list = null, String active = "", String order_by = "id", Boolean order_desc = true, int start_point = 0, int count = -1)
        {
            List<SanPham> obj_list = new List<SanPham>();
            //find by LIKE element
            obj_list = this._db.ds_sanpham.Where(x =>
                x.masp.Contains(masp)
                && x.ten.Contains(ten)
                && x.mota.Contains(mota)
                ).ToList();

            //filter by id
            if (!id.Equals(""))
            {
                int id_i = TextLibrary.ToInt(id);
                obj_list = obj_list.Where(x => x.id == id_i).ToList();
            }
            //Filter by gia
            if (gia_from>0 || gia_to>0)
            {
                obj_list = obj_list.Where(x => x.gia >= gia_from && x.gia<=gia_to).ToList();
            }
            //filter by HangSX List
            if (hangsx_list != null && hangsx_list.Count>0)
            {
                //build list id
                List<int> hangsx_id_list = new List<int>();
                hangsx_id_list= hangsx_list.Select(x => x.id).ToList();
                //query
                obj_list = obj_list.Where(x => hangsx_id_list.Contains(x.hangsx.id)).ToList();
            }
            //filter by NhomSanPham List
            if (nhomsanpham_list != null && nhomsanpham_list.Count > 0)
            {
                //build list id
                List<int> nhomsanpham_id_list = new List<int>();
                nhomsanpham_id_list = nhomsanpham_list.Select(x => x.id).ToList();
                //query
                obj_list = obj_list.Where(x => nhomsanpham_id_list.Contains(x.nhomsanpham.id)).ToList();
            }

            //Filter again by by active
            if (!active.Equals(""))
            {
                Boolean active_b = TextLibrary.ToBoolean(active);
                obj_list = obj_list.Where(x => x.active == active_b).ToList();
            }

            //order
            if (order_by.Equals("id"))
            {
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.id).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.id).ToList();
                }
            }
            else if (order_by.Equals("masp"))
            {
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.masp).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.masp).ToList();
                }
            }
            else if (order_by.Equals("ten"))
            {
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.ten).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.ten).ToList();
                }
            }
            else if (order_by.Equals("gia"))
            {
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.gia).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.gia).ToList();
                }
            }
            else if (order_by.Equals("nhomsanpham"))
            {
                if (order_desc)
                {
                    List<SanPham> nhom_notnull= obj_list.Where(x=>x.nhomsanpham!=null).OrderByDescending(x=>x.nhomsanpham.ten).ToList();
                    List<SanPham> nhom_null = obj_list.Where(x => x.nhomsanpham == null).OrderByDescending(x => x.id).ToList();
                    obj_list = nhom_notnull.Concat(nhom_null).ToList();
                }
                else
                {
                    List<SanPham> nhom_notnull = obj_list.Where(x => x.nhomsanpham != null).OrderBy(x => x.nhomsanpham.ten).ToList();
                    List<SanPham> nhom_null = obj_list.Where(x => x.nhomsanpham == null).OrderBy(x => x.id).ToList();
                    obj_list = nhom_notnull.Concat(nhom_null).ToList();
                }
            }
            else if (order_by.Equals("hangsx"))
            {
                if (order_desc)
                {
                    List<SanPham> hsx_notnull = obj_list.Where(x => x.hangsx != null).OrderByDescending(x => x.hangsx.ten).ToList();
                    List<SanPham> hsx_null = obj_list.Where(x => x.hangsx == null).OrderByDescending(x => x.id).ToList();
                    obj_list = hsx_notnull.Concat(hsx_null).ToList();
                }
                else
                {
                    List<SanPham> hsx_notnull = obj_list.Where(x => x.hangsx != null).OrderBy(x => x.hangsx.ten).ToList();
                    List<SanPham> hsx_null = obj_list.Where(x => x.hangsx == null).OrderBy(x => x.id).ToList();
                    obj_list = hsx_notnull.Concat(hsx_null).ToList();
                }
            }
            else if (order_by.Equals("active"))
            {
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.active).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.active).ToList();
                }
            }

            //limit
            if (count >= 0)
            {
                obj_list = obj_list.Skip(start_point).Take(count).ToList();
            }
            //FINAL return
            return obj_list;
        }
        public Boolean can_use_masp(int obj_id, String masp)
        {
            SanPham u = (from obj in _db.ds_sanpham
                          where obj.masp.ToUpper().Equals(masp.ToUpper())
                          && obj.id != obj_id
                          select obj).FirstOrDefault();
            return u == null ? true : false;
        }
        public List<String> validate(SanPham obj)
        {
            //
            List<String> re = new List<string>();
            //check
            if (!this.can_use_masp(obj.id, obj.masp))
            {
                re.Add("masp_exist_fail");
            }
            if (obj.masp.Equals(""))
            {
                re.Add("masp_fail");
            }
            if (obj.ten.Equals(""))
            {
                re.Add("ten_fail");
            }
            if (obj.gia<0 || obj.gia>999999999)
            {
                re.Add("gia_fail");
            }
            return re;
        }
        public List<SanPham> timkiem_dequy(NhomSanPham root = null, String active = "", int start_point = 0, int count = -1)
        {
            List<SanPham> re = new List<SanPham>();
            NhomSanPhamController ctr=new NhomSanPhamController();
            List<NhomSanPham> list = ctr.get_tree2(root);
            re.AddRange(this.timkiem("","","","",0,0,null,list,active,"id",true,start_point,count));
            return re;
        }
    }
}