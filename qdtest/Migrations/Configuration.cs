namespace qdtest.Migrations
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
        }
    }
}
