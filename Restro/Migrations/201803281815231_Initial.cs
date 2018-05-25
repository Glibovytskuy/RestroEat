namespace Restro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DayModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        From = c.Time(precision: 7),
                        To = c.Time(precision: 7),
                        IsWeeknd = c.Boolean(),
                        IsRoundClock = c.Boolean(),
                        PlaceModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlaceModels", t => t.PlaceModelId, cascadeDelete: true)
                .Index(t => t.PlaceModelId);
            
            CreateTable(
                "dbo.PlaceModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Type = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Contacts = c.String(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommentModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        UserId = c.String(),
                        PlaceId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        PlaceModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlaceModels", t => t.PlaceModel_Id)
                .Index(t => t.PlaceModel_Id);
            
            CreateTable(
                "dbo.FeatureModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KitchenModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RatingModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Mark = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FeatureModelPlaceModels",
                c => new
                    {
                        FeatureModel_Id = c.Int(nullable: false),
                        PlaceModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FeatureModel_Id, t.PlaceModel_Id })
                .ForeignKey("dbo.FeatureModels", t => t.FeatureModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.PlaceModels", t => t.PlaceModel_Id, cascadeDelete: true)
                .Index(t => t.FeatureModel_Id)
                .Index(t => t.PlaceModel_Id);
            
            CreateTable(
                "dbo.KitchenModelPlaceModels",
                c => new
                    {
                        KitchenModel_Id = c.Int(nullable: false),
                        PlaceModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.KitchenModel_Id, t.PlaceModel_Id })
                .ForeignKey("dbo.KitchenModels", t => t.KitchenModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.PlaceModels", t => t.PlaceModel_Id, cascadeDelete: true)
                .Index(t => t.KitchenModel_Id)
                .Index(t => t.PlaceModel_Id);
            
            CreateTable(
                "dbo.RatingModelPlaceModels",
                c => new
                    {
                        RatingModel_Id = c.Int(nullable: false),
                        PlaceModel_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RatingModel_Id, t.PlaceModel_Id })
                .ForeignKey("dbo.RatingModels", t => t.RatingModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.PlaceModels", t => t.PlaceModel_Id, cascadeDelete: true)
                .Index(t => t.RatingModel_Id)
                .Index(t => t.PlaceModel_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RatingModelPlaceModels", "PlaceModel_Id", "dbo.PlaceModels");
            DropForeignKey("dbo.RatingModelPlaceModels", "RatingModel_Id", "dbo.RatingModels");
            DropForeignKey("dbo.KitchenModelPlaceModels", "PlaceModel_Id", "dbo.PlaceModels");
            DropForeignKey("dbo.KitchenModelPlaceModels", "KitchenModel_Id", "dbo.KitchenModels");
            DropForeignKey("dbo.FeatureModelPlaceModels", "PlaceModel_Id", "dbo.PlaceModels");
            DropForeignKey("dbo.FeatureModelPlaceModels", "FeatureModel_Id", "dbo.FeatureModels");
            DropForeignKey("dbo.DayModels", "PlaceModelId", "dbo.PlaceModels");
            DropForeignKey("dbo.CommentModels", "PlaceModel_Id", "dbo.PlaceModels");
            DropIndex("dbo.RatingModelPlaceModels", new[] { "PlaceModel_Id" });
            DropIndex("dbo.RatingModelPlaceModels", new[] { "RatingModel_Id" });
            DropIndex("dbo.KitchenModelPlaceModels", new[] { "PlaceModel_Id" });
            DropIndex("dbo.KitchenModelPlaceModels", new[] { "KitchenModel_Id" });
            DropIndex("dbo.FeatureModelPlaceModels", new[] { "PlaceModel_Id" });
            DropIndex("dbo.FeatureModelPlaceModels", new[] { "FeatureModel_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.CommentModels", new[] { "PlaceModel_Id" });
            DropIndex("dbo.DayModels", new[] { "PlaceModelId" });
            DropTable("dbo.RatingModelPlaceModels");
            DropTable("dbo.KitchenModelPlaceModels");
            DropTable("dbo.FeatureModelPlaceModels");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RatingModels");
            DropTable("dbo.KitchenModels");
            DropTable("dbo.FeatureModels");
            DropTable("dbo.CommentModels");
            DropTable("dbo.PlaceModels");
            DropTable("dbo.DayModels");
        }
    }
}
