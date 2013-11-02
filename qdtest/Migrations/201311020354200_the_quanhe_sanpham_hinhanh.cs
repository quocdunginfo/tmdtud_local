namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class the_quanhe_sanpham_hinhanh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HinhAnhs", "SanPham_id", "dbo.SanPhams");
            DropIndex("dbo.HinhAnhs", new[] { "SanPham_id" });
            AddColumn("dbo.HinhAnhs", "duongdan_thumb", c => c.String());
            AddColumn("dbo.HinhAnhs", "macdinh", c => c.Boolean(nullable: false));
            AlterColumn("dbo.HinhAnhs", "sanpham_id", c => c.Int());
            AddForeignKey("dbo.HinhAnhs", "sanpham_id", "dbo.SanPhams", "id");
            CreateIndex("dbo.HinhAnhs", "sanpham_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.HinhAnhs", new[] { "sanpham_id" });
            DropForeignKey("dbo.HinhAnhs", "sanpham_id", "dbo.SanPhams");
            AlterColumn("dbo.HinhAnhs", "SanPham_id", c => c.Int());
            DropColumn("dbo.HinhAnhs", "macdinh");
            DropColumn("dbo.HinhAnhs", "duongdan_thumb");
            CreateIndex("dbo.HinhAnhs", "SanPham_id");
            AddForeignKey("dbo.HinhAnhs", "SanPham_id", "dbo.SanPhams", "id");
        }
    }
}
