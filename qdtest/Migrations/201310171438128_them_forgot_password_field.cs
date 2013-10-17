namespace qdtest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class them_forgot_password_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NhanViens", "forgot_password_session", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NhanViens", "forgot_password_session");
        }
    }
}
