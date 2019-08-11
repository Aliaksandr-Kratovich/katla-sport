using KatlaSport.Services.PhoneManagement;

namespace KatlaSport.DataAccess.PhoneStore
{
    internal sealed class ProductStorePhoneContext : DomainContextBase<ApplicationDbContext>, IProductStorePhoneContext
    {
        public ProductStorePhoneContext(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEntitySet<StorePhone> Phones => GetDbSet<StorePhone>();

        public IEntitySet<StorePhoneModel> Models => GetDbSet<StorePhoneModel>();
    }
}
