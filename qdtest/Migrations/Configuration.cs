﻿namespace qdtest.Migrations
{
    using qdtest.Controllers.ModelController;
    using qdtest.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<qdtest.Models.BanGiayDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(qdtest.Models.BanGiayDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            // Update-Database
            
            //them quyen
            String[] quyens = { "home", "sanpham", "user", "khachhang", "nhomsanpham", "kichthuoc", "mausac", "hinhanh", "hangsx","nhacc","nhaphang","donhang","chitietsp","loainhanvien" };
                List<Quyen> ds = new List<Quyen>();
                foreach (String item in quyens)
                {
                    Quyen tmp = new Quyen();
                    tmp.ten = item + "_view";
                    ds.Add(tmp);

                    tmp = new Quyen();
                    tmp.ten = item + "_edit";
                    ds.Add(tmp);

                    tmp = new Quyen();
                    tmp.ten = item + "_delete";
                    ds.Add(tmp);

                    tmp = new Quyen();
                    tmp.ten = item + "_add";
                    ds.Add(tmp);
                }

                foreach (Quyen item in ds)
                {
                    context.ds_quyen.AddOrUpdate(
                    q => q.ten,
                    item
                    );
                }
                context.SaveChanges();
            //them loai nhan vien
                LoaiNhanVien lnv = new LoaiNhanVien();
                lnv.ten = "Admin";
                //lnv.ds_quyen.AddRange(context.ds_quyen.ToList());
                context.ds_loainhanvien.AddOrUpdate(
                    q => q.ten,
                    lnv
                    );
                context.SaveChanges();
                lnv = context.ds_loainhanvien.Where(x => x.ten.Equals("Admin")).FirstOrDefault();
                lnv.ds_quyen.Clear();
                lnv.ds_quyen.AddRange(context.ds_quyen.ToList());
            //them user
                NhanVien nv = new NhanVien();
                nv.tendangnhap = "admin";
                nv.tendaydu = "Quản trị viên";
                nv.email = "quocdunginfo@gmail.com";
                nv.active = true;
                nv.matkhau = "D033E22AE348AEB5660FC2140AEC35850C4DA997";//SHA1("admin")
                nv.loainhanvien = context.ds_loainhanvien.Where(x=>x.ten.Equals("Admin")).FirstOrDefault();
                context.ds_nhanvien.AddOrUpdate(
                    q => q.tendangnhap,
                    nv
                    );
                context.SaveChanges();
            //them tinh thanh pho
                String[] tinhtp_list ={
                            "TP HCM", "An Giang","Bà Rịa - Vũng Tàu","Bắc Giang","Bắc Kạn","Bạc Liêu","Bắc Ninh","Bến Tre","Bình Định","Bình Dương","Bình Phước","Bình Thuận","Cà Mau","Cao Bằng","Đắk Lắk","Đắk Nông","Điện Biên","Đồng Nai","Đồng Tháp","Gia Lai","Hà Giang","Hà Nam","Hà Tĩnh","Hải Dương","Hậu Giang","Hòa Bình","Hưng Yên","Khánh Hòa","Kiên Giang","Kon Tum","Lai Châu","Lâm Đồng","Lạng Sơn","Lào Cai","Long An","Nam Định","Nghệ An","Ninh Bình","Ninh Thuận","Phú Thọ","Quảng Bình","Quảng Nam","Quảng Ngãi","Quảng Ninh","Quảng Trị","Sóc Trăng","Sơn La","Tây Ninh","Thái Bình","Thái Nguyên","Thanh Hóa","Thừa Thiên Huế","Tiền Giang","Trà Vinh","Tuyên Quang","Vĩnh Long","Vĩnh Phúc","Yên Bái","Phú Yên","Cần Thơ","Đà Nẵng","Hải Phòng","Hà Nội"
                          };
                foreach (String tinhtp in tinhtp_list)
                {
                    TinhTP tmp = new TinhTP();
                    tmp.ten = tinhtp;
                    tmp.noithanh = false;
                    tmp.phivanchuyen = 100000;
                    if (tmp.ten.ToUpper().Contains("HCM"))
                    {
                        tmp.noithanh = true;
                        tmp.phivanchuyen = 20000;
                    }
                    context.ds_tinhtp.AddOrUpdate(
                        q => q.ten,
                        tmp
                        );
                }
                context.SaveChanges();
            //Thêm loại khach hang
            /*    
            string[] loaikh = { "" };
                foreach (string item in loaikh)
                {
                    LoaiKhachHang tmp = new LoaiKhachHang();
                    context.ds_loaikh.AddOrUpdate(
                                    q => q.ten,
                                    tmp
                                    );
                }
            */
        }
    }
}
