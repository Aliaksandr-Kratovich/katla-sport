using System.Data.Entity.Migrations;

namespace KatlaSport.DataAccess.Migrations
{
    public partial class MigratePhone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.product_phone_models",
                c => new
                    {
                        product_phone_model_id = c.Int(nullable: false, identity: true),
                        product_phone_model = c.String(nullable: false, maxLength: 20),
                        product_phone_model_code = c.String(nullable: false, maxLength: 20),
                        StorePhoneId = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.product_phone_model_id)
                .ForeignKey("dbo.product_phones", t => t.StorePhoneId, cascadeDelete: true)
                .Index(t => t.StorePhoneId);

            CreateTable(
                "dbo.product_phones",
                c => new
                    {
                        product_phone_id = c.Int(nullable: false, identity: true),
                        product_phone_mark = c.String(nullable: false, maxLength: 20),
                        product_phone_country = c.String(nullable: false, maxLength: 50),
                        updated_utc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.product_phone_id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.product_phone_models", "StorePhoneId", "dbo.product_phones");
            DropIndex("dbo.product_phone_models", new[] { "StorePhoneId" });
            DropTable("dbo.product_phones");
            DropTable("dbo.product_phone_models");
        }
    }
}
