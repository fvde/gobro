namespace GoBroBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("GoBroBackend.Challenges", "IsUserGenerated", c => c.Boolean(nullable: false));
            AddColumn("GoBroBackend.Users", "DisplayName", c => c.String());
            AddColumn("GoBroBackend.Users", "Email", c => c.String());
            DropColumn("GoBroBackend.Users", "Username");
        }
        
        public override void Down()
        {
            AddColumn("GoBroBackend.Users", "Username", c => c.String());
            DropColumn("GoBroBackend.Users", "Email");
            DropColumn("GoBroBackend.Users", "DisplayName");
            DropColumn("GoBroBackend.Challenges", "IsUserGenerated");
        }
    }
}
