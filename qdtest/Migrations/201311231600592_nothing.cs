namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nothing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UuDais", "ngaybatdau", c => c.DateTime(nullable: false));
            AddColumn("dbo.UuDais", "ngayketthuc", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UuDais", "ngayketthuc");
            DropColumn("dbo.UuDais", "ngaybatdau");
        }
    }
}
