using System.Collections.Generic;
using System.Threading.Tasks;

namespace KatlaSport.Services.PhoneManagement
{
    public interface IPhoneModelService
    {
        Task<List<UpdatePhoneModelRequest>> GetModelsAsync();

        Task<List<UpdatePhoneModelRequest>> GetModelsAsync(int phoneId);

        Task<UpdatePhoneModelRequest> GetModelAsync(int modelId);

        Task<StorePhoneModel> CreateModelsAsync(UpdatePhoneModelRequest createRequest, int phoneId);

        Task DeleteModelAsync(int modelId);
    }
}
