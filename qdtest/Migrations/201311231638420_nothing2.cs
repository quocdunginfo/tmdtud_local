namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nothing2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UuDais", "giatri", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UuDais", "giatri", c => c.String());
        }
    }
}
