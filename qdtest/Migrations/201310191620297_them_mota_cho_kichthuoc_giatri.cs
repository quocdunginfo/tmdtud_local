namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class them_mota_cho_kichthuoc_giatri : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KichThuocs", "mota", c => c.String());
            AddColumn("dbo.MauSacs", "mota", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MauSacs", "mota");
            DropColumn("dbo.KichThuocs", "mota");
        }
    }
}
