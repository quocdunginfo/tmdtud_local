namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class them_class_quyen_han : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoaiNhanViens",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ten = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Quyens",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ten = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.QuyenLoaiNhanViens",
                c => new
                    {
                        Quyen_id = c.Int(nullable: false),
                        LoaiNhanVien_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Quyen_id, t.LoaiNhanVien_id })
                .ForeignKey("dbo.Quyens", t => t.Quyen_id, cascadeDelete: true)
                .ForeignKey("dbo.LoaiNhanViens", t => t.LoaiNhanVien_id, cascadeDelete: true)
                .Index(t => t.Quyen_id)
                .Index(t => t.LoaiNhanVien_id);
            
            AddColumn("dbo.NhanViens", "loainhanvien_id", c => c.Int());
            AddForeignKey("dbo.NhanViens", "loainhanvien_id", "dbo.LoaiNhanViens", "id");
            CreateIndex("dbo.NhanViens", "loainhanvien_id");
            DropColumn("dbo.NhanViens", "group_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NhanViens", "group_id", c => c.Int(nullable: false));
            DropIndex("dbo.QuyenLoaiNhanViens", new[] { "LoaiNhanVien_id" });
            DropIndex("dbo.QuyenLoaiNhanViens", new[] { "Quyen_id" });
            DropIndex("dbo.NhanViens", new[] { "loainhanvien_id" });
            DropForeignKey("dbo.QuyenLoaiNhanViens", "LoaiNhanVien_id", "dbo.LoaiNhanViens");
            DropForeignKey("dbo.QuyenLoaiNhanViens", "Quyen_id", "dbo.Quyens");
            DropForeignKey("dbo.NhanViens", "loainhanvien_id", "dbo.LoaiNhanViens");
            DropColumn("dbo.NhanViens", "loainhanvien_id");
            DropTable("dbo.QuyenLoaiNhanViens");
            DropTable("dbo.Quyens");
            DropTable("dbo.LoaiNhanViens");
        }
    }
}
