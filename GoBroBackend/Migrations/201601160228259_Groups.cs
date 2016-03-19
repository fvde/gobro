namespace GoBroBackend.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Groups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "GoBroBackend.Groups",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                        DisplayName = c.String(),
                        LastChallengeChange = c.DateTimeOffset(nullable: false, precision: 7),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                        CurrentChallenge_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("GoBroBackend.Challenges", t => t.CurrentChallenge_Id)
                .Index(t => t.CreatedAt, clustered: true)
                .Index(t => t.CurrentChallenge_Id);
            
            AddColumn("GoBroBackend.Challenges", "Group_Id", c => c.String(maxLength: 128));
            AddColumn("GoBroBackend.Users", "Group_Id", c => c.String(maxLength: 128));
            CreateIndex("GoBroBackend.Challenges", "Group_Id");
            CreateIndex("GoBroBackend.Users", "Group_Id");
            AddForeignKey("GoBroBackend.Challenges", "Group_Id", "GoBroBackend.Groups", "Id");
            AddForeignKey("GoBroBackend.Users", "Group_Id", "GoBroBackend.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("GoBroBackend.Users", "Group_Id", "GoBroBackend.Groups");
            DropForeignKey("GoBroBackend.Challenges", "Group_Id", "GoBroBackend.Groups");
            DropForeignKey("GoBroBackend.Groups", "CurrentChallenge_Id", "GoBroBackend.Challenges");
            DropIndex("GoBroBackend.Groups", new[] { "CurrentChallenge_Id" });
            DropIndex("GoBroBackend.Groups", new[] { "CreatedAt" });
            DropIndex("GoBroBackend.Users", new[] { "Group_Id" });
            DropIndex("GoBroBackend.Challenges", new[] { "Group_Id" });
            DropColumn("GoBroBackend.Users", "Group_Id");
            DropColumn("GoBroBackend.Challenges", "Group_Id");
            DropTable("GoBroBackend.Groups",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
        }
    }
}
