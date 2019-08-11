using KatlaSport.Services.PhoneManagement;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KatlaSport.DataAccess.PhoneStore
{
    internal sealed class StorePhoneConfiguration : EntityTypeConfiguration<StorePhone>
    {
        public StorePhoneConfiguration()
        {
            ToTable("product_phones");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("product_phone_id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(i => i.Mark).HasColumnName("product_phone_mark").HasMaxLength(20).IsRequired();
            Property(i => i.Country).HasColumnName("product_phone_country").HasMaxLength(50).IsRequired();
            Property(i => i.LastUpdated).HasColumnName("updated_utc").IsRequired();
        }
    }
}
