namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sua_tenhsx_to_ten : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HangSXes", "ten", c => c.String());
            DropColumn("dbo.HangSXes", "tenhsx");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HangSXes", "tenhsx", c => c.String());
            DropColumn("dbo.HangSXes", "ten");
        }
    }
}
