namespace GoBroBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("GoBroBackend.Entries", "Challenge_Id", "GoBroBackend.Challenges");
            DropIndex("GoBroBackend.Entries", new[] { "Challenge_Id" });
            AddColumn("GoBroBackend.Entries", "ContainerName", c => c.String());
            AddColumn("GoBroBackend.Entries", "ResourceName", c => c.String());
            AddColumn("GoBroBackend.Entries", "SasQueryString", c => c.String());
            AddColumn("GoBroBackend.Entries", "ImageUri", c => c.String());
            AlterColumn("GoBroBackend.Entries", "Challenge_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("GoBroBackend.Entries", "Challenge_Id");
            AddForeignKey("GoBroBackend.Entries", "Challenge_Id", "GoBroBackend.Challenges", "Id", cascadeDelete: true);
            DropColumn("GoBroBackend.Entries", "ImageId");
        }
        
        public override void Down()
        {
            AddColumn("GoBroBackend.Entries", "ImageId", c => c.String());
            DropForeignKey("GoBroBackend.Entries", "Challenge_Id", "GoBroBackend.Challenges");
            DropIndex("GoBroBackend.Entries", new[] { "Challenge_Id" });
            AlterColumn("GoBroBackend.Entries", "Challenge_Id", c => c.String(maxLength: 128));
            DropColumn("GoBroBackend.Entries", "ImageUri");
            DropColumn("GoBroBackend.Entries", "SasQueryString");
            DropColumn("GoBroBackend.Entries", "ResourceName");
            DropColumn("GoBroBackend.Entries", "ContainerName");
            CreateIndex("GoBroBackend.Entries", "Challenge_Id");
            AddForeignKey("GoBroBackend.Entries", "Challenge_Id", "GoBroBackend.Challenges", "Id");
        }
    }
}
