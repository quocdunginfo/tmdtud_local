namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cap_nhat_tpic_phanhoi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NhomSanPhams", "mota", c => c.String());
            AddColumn("dbo.NhomSanPhams", "active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NhomSanPhams", "active");
            DropColumn("dbo.NhomSanPhams", "mota");
        }
    }
}
