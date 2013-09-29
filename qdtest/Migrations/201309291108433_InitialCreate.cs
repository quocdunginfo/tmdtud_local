namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DatHangs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ngay = c.DateTime(nullable: false),
                        tongtien = c.Int(nullable: false),
                        dathanhtoan = c.Boolean(nullable: false),
                        dagiaohang = c.Boolean(nullable: false),
                        diachi_nguoinhan = c.String(),
                        ten_nguoinhan = c.String(),
                        sdt_nguoinhan = c.String(),
                        SanPham_id = c.Int(),
                        nguoidung_id = c.Int(),
                        khachhang_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.SanPhams", t => t.SanPham_id)
                .ForeignKey("dbo.NhanViens", t => t.nguoidung_id)
                .ForeignKey("dbo.KhachHangs", t => t.khachhang_id)
                .Index(t => t.SanPham_id)
                .Index(t => t.nguoidung_id)
                .Index(t => t.khachhang_id);
            
            CreateTable(
                "dbo.ChiTiet_DatHang",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        soluong = c.Int(nullable: false),
                        dongia = c.Int(nullable: false),
                        sanpham_tag_id = c.Int(),
                        DatHang_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.SanPham_Tag", t => t.sanpham_tag_id)
                .ForeignKey("dbo.DatHangs", t => t.DatHang_id)
                .Index(t => t.sanpham_tag_id)
                .Index(t => t.DatHang_id);
            
            CreateTable(
                "dbo.SanPham_Tag",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        soluong = c.Int(nullable: false),
                        kichthuoc_id = c.Int(),
                        mausac_id = c.Int(),
                        sanpham_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.KichThuocs", t => t.kichthuoc_id)
                .ForeignKey("dbo.MauSacs", t => t.mausac_id)
                .ForeignKey("dbo.SanPhams", t => t.sanpham_id)
                .Index(t => t.kichthuoc_id)
                .Index(t => t.mausac_id)
                .Index(t => t.sanpham_id);
            
            CreateTable(
                "dbo.KichThuocs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        giatri = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.MauSacs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        giatri = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SanPhams",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        masp = c.String(),
                        ten = c.String(),
                        mota = c.String(),
                        gia = c.Int(nullable: false),
                        active = c.Boolean(nullable: false),
                        hangsx_id = c.Int(),
                        nhomsanpham_id = c.Int(),
                        nguoidung_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.HangSXes", t => t.hangsx_id)
                .ForeignKey("dbo.NhomSanPhams", t => t.nhomsanpham_id)
                .ForeignKey("dbo.NhanViens", t => t.nguoidung_id)
                .Index(t => t.hangsx_id)
                .Index(t => t.nhomsanpham_id)
                .Index(t => t.nguoidung_id);
            
            CreateTable(
                "dbo.HinhAnhs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        duongdan = c.String(),
                        SanPham_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.SanPhams", t => t.SanPham_id)
                .Index(t => t.SanPham_id);
            
            CreateTable(
                "dbo.HangSXes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ten = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.NhomSanPhams",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ten = c.String(),
                        nhomcha_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.NhomSanPhams", t => t.nhomcha_id)
                .Index(t => t.nhomcha_id);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        tendangnhap = c.String(),
                        tendaydu = c.String(),
                        email = c.String(),
                        matkhau = c.String(),
                        active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        tendangnhap = c.String(),
                        tendaydu = c.String(),
                        matkhau = c.String(),
                        diachi = c.String(),
                        email = c.String(),
                        sdt = c.String(),
                        active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.NhomSanPhams", new[] { "nhomcha_id" });
            DropIndex("dbo.HinhAnhs", new[] { "SanPham_id" });
            DropIndex("dbo.SanPhams", new[] { "nguoidung_id" });
            DropIndex("dbo.SanPhams", new[] { "nhomsanpham_id" });
            DropIndex("dbo.SanPhams", new[] { "hangsx_id" });
            DropIndex("dbo.SanPham_Tag", new[] { "sanpham_id" });
            DropIndex("dbo.SanPham_Tag", new[] { "mausac_id" });
            DropIndex("dbo.SanPham_Tag", new[] { "kichthuoc_id" });
            DropIndex("dbo.ChiTiet_DatHang", new[] { "DatHang_id" });
            DropIndex("dbo.ChiTiet_DatHang", new[] { "sanpham_tag_id" });
            DropIndex("dbo.DatHangs", new[] { "khachhang_id" });
            DropIndex("dbo.DatHangs", new[] { "nguoidung_id" });
            DropIndex("dbo.DatHangs", new[] { "SanPham_id" });
            DropForeignKey("dbo.NhomSanPhams", "nhomcha_id", "dbo.NhomSanPhams");
            DropForeignKey("dbo.HinhAnhs", "SanPham_id", "dbo.SanPhams");
            DropForeignKey("dbo.SanPhams", "nguoidung_id", "dbo.NhanViens");
            DropForeignKey("dbo.SanPhams", "nhomsanpham_id", "dbo.NhomSanPhams");
            DropForeignKey("dbo.SanPhams", "hangsx_id", "dbo.HangSXes");
            DropForeignKey("dbo.SanPham_Tag", "sanpham_id", "dbo.SanPhams");
            DropForeignKey("dbo.SanPham_Tag", "mausac_id", "dbo.MauSacs");
            DropForeignKey("dbo.SanPham_Tag", "kichthuoc_id", "dbo.KichThuocs");
            DropForeignKey("dbo.ChiTiet_DatHang", "DatHang_id", "dbo.DatHangs");
            DropForeignKey("dbo.ChiTiet_DatHang", "sanpham_tag_id", "dbo.SanPham_Tag");
            DropForeignKey("dbo.DatHangs", "khachhang_id", "dbo.KhachHangs");
            DropForeignKey("dbo.DatHangs", "nguoidung_id", "dbo.NhanViens");
            DropForeignKey("dbo.DatHangs", "SanPham_id", "dbo.SanPhams");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.NhanViens");
            DropTable("dbo.NhomSanPhams");
            DropTable("dbo.HangSXes");
            DropTable("dbo.HinhAnhs");
            DropTable("dbo.SanPhams");
            DropTable("dbo.MauSacs");
            DropTable("dbo.KichThuocs");
            DropTable("dbo.SanPham_Tag");
            DropTable("dbo.ChiTiet_DatHang");
            DropTable("dbo.DatHangs");
        }
    }
}
