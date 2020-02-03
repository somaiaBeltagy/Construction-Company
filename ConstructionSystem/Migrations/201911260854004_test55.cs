namespace ConstructionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test55 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employee", "Phone", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employee", "Phone", c => c.String(maxLength: 15));
        }
    }
}
