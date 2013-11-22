namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nothing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NhanViens", "bad", c => c.Boolean(nullable: false));
            AddColumn("dbo.KhachHangs", "bad", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "bad");
            DropColumn("dbo.NhanViens", "bad");
        }
    }
}
