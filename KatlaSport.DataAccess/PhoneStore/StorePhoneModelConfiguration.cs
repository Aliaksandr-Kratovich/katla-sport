using KatlaSport.Services.PhoneManagement;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KatlaSport.DataAccess.PhoneStore
{
    internal sealed class StorePhoneModelConfiguration : EntityTypeConfiguration<StorePhoneModel>
    {
        public StorePhoneModelConfiguration()
        {
            ToTable("product_phone_models");
            HasKey(i => i.Id);
            HasRequired(i => i.StorePhone).WithMany(i => i.Models).HasForeignKey(i => i.StorePhoneId);
            Property(i => i.Id).HasColumnName("product_phone_model_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(i => i.Name).HasColumnName("product_phone_model").HasMaxLength(20).IsRequired();
            Property(i => i.Code).HasColumnName("product_phone_model_code").HasMaxLength(20).IsRequired();
        }
    }
}
