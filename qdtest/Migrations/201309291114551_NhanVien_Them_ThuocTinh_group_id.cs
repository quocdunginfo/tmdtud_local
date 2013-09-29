namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NhanVien_Them_ThuocTinh_group_id : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NhanViens", "group_id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NhanViens", "group_id");
        }
    }
}
