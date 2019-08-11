using System.Collections.Generic;
using System.Threading.Tasks;

namespace KatlaSport.Services.PhoneManagement
{
    public interface IPhoneService
    {
        Task<List<UpdatePhoneRequest>> GetPhonesAsync();

        Task<UpdatePhoneRequest> GetPhoneAsync(int phoneId);

        Task<StorePhone> CreatePhoneAsync(UpdatePhoneRequest createRequest);

        Task<StorePhone> UpdatePhoneAsync(int phoneId, UpdatePhoneRequest updateRequest);

        Task DeletePhoneAsync(int phoneId);
    }
}
