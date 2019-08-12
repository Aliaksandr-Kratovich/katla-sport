namespace KatlaSport.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UrlPhotoMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.product_phone_models", "product_phone_model_url_photo", c => c.String(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.product_phone_models", "product_phone_model_url_photo");
        }
    }
}
