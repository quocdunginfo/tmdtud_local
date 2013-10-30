namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sua_SanPhamTag_ChiTietSP : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChiTiet_DonHang", "sanpham_tag_id", "dbo.SanPham_Tag");
            DropForeignKey("dbo.SanPham_Tag", "kichthuoc_id", "dbo.KichThuocs");
            DropForeignKey("dbo.SanPham_Tag", "mausac_id", "dbo.MauSacs");
            DropForeignKey("dbo.SanPham_Tag", "sanpham_id", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTiet_NhapHang", "sanpham_tag_id", "dbo.SanPham_Tag");
            DropForeignKey("dbo.TonKhoes", "sanpham_tag_id", "dbo.SanPham_Tag");
            DropIndex("dbo.ChiTiet_DonHang", new[] { "sanpham_tag_id" });
            DropIndex("dbo.SanPham_Tag", new[] { "kichthuoc_id" });
            DropIndex("dbo.SanPham_Tag", new[] { "mausac_id" });
            DropIndex("dbo.SanPham_Tag", new[] { "sanpham_id" });
            DropIndex("dbo.ChiTiet_NhapHang", new[] { "sanpham_tag_id" });
            DropIndex("dbo.TonKhoes", new[] { "sanpham_tag_id" });
            CreateTable(
                "dbo.ChiTietSPs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        soluong = c.Int(nullable: false),
                        active = c.Boolean(nullable: false),
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
            
            AddForeignKey("dbo.ChiTiet_DonHang", "sanpham_tag_id", "dbo.ChiTietSPs", "id");
            AddForeignKey("dbo.ChiTiet_NhapHang", "sanpham_tag_id", "dbo.ChiTietSPs", "id");
            AddForeignKey("dbo.TonKhoes", "sanpham_tag_id", "dbo.ChiTietSPs", "id");
            CreateIndex("dbo.ChiTiet_DonHang", "sanpham_tag_id");
            CreateIndex("dbo.ChiTiet_NhapHang", "sanpham_tag_id");
            CreateIndex("dbo.TonKhoes", "sanpham_tag_id");
            DropTable("dbo.SanPham_Tag");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SanPham_Tag",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        soluong = c.Int(nullable: false),
                        active = c.Boolean(nullable: false),
                        kichthuoc_id = c.Int(),
                        mausac_id = c.Int(),
                        sanpham_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            DropIndex("dbo.TonKhoes", new[] { "sanpham_tag_id" });
            DropIndex("dbo.ChiTiet_NhapHang", new[] { "sanpham_tag_id" });
            DropIndex("dbo.ChiTietSPs", new[] { "sanpham_id" });
            DropIndex("dbo.ChiTietSPs", new[] { "mausac_id" });
            DropIndex("dbo.ChiTietSPs", new[] { "kichthuoc_id" });
            DropIndex("dbo.ChiTiet_DonHang", new[] { "sanpham_tag_id" });
            DropForeignKey("dbo.TonKhoes", "sanpham_tag_id", "dbo.ChiTietSPs");
            DropForeignKey("dbo.ChiTiet_NhapHang", "sanpham_tag_id", "dbo.ChiTietSPs");
            DropForeignKey("dbo.ChiTietSPs", "sanpham_id", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTietSPs", "mausac_id", "dbo.MauSacs");
            DropForeignKey("dbo.ChiTietSPs", "kichthuoc_id", "dbo.KichThuocs");
            DropForeignKey("dbo.ChiTiet_DonHang", "sanpham_tag_id", "dbo.ChiTietSPs");
            DropTable("dbo.ChiTietSPs");
            CreateIndex("dbo.TonKhoes", "sanpham_tag_id");
            CreateIndex("dbo.ChiTiet_NhapHang", "sanpham_tag_id");
            CreateIndex("dbo.SanPham_Tag", "sanpham_id");
            CreateIndex("dbo.SanPham_Tag", "mausac_id");
            CreateIndex("dbo.SanPham_Tag", "kichthuoc_id");
            CreateIndex("dbo.ChiTiet_DonHang", "sanpham_tag_id");
            AddForeignKey("dbo.TonKhoes", "sanpham_tag_id", "dbo.SanPham_Tag", "id");
            AddForeignKey("dbo.ChiTiet_NhapHang", "sanpham_tag_id", "dbo.SanPham_Tag", "id");
            AddForeignKey("dbo.SanPham_Tag", "sanpham_id", "dbo.SanPhams", "id");
            AddForeignKey("dbo.SanPham_Tag", "mausac_id", "dbo.MauSacs", "id");
            AddForeignKey("dbo.SanPham_Tag", "kichthuoc_id", "dbo.KichThuocs", "id");
            AddForeignKey("dbo.ChiTiet_DonHang", "sanpham_tag_id", "dbo.SanPham_Tag", "id");
        }
    }
}
