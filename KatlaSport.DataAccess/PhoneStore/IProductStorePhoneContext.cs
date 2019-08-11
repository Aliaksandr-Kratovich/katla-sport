using KatlaSport.Services.PhoneManagement;

namespace KatlaSport.DataAccess.PhoneStore
{
    public interface IProductStorePhoneContext : IAsyncEntityStorage
    {
        IEntitySet<StorePhone> Phones { get; }

        IEntitySet<StorePhoneModel> Models { get; }
    }
}
