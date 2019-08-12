using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace KatlaSport.Services.PhoneManagement
{
    public interface IPhoneModelService
    {
        Task<List<UpdatePhoneModelRequest>> GetModelsAsync();

        Task<List<UpdatePhoneModelRequest>> GetModelsAsync(int phoneId);

        Task<UpdatePhoneModelRequest> GetModelAsync(int modelId);

        Task<StorePhoneModel> CreateModelsAsync(UpdatePhoneModelRequest createRequest, int phoneId, CloudBlobContainer blobContainer, HttpPostedFile photo);

        Task DeleteModelAsync(int modelId, CloudBlobContainer blobContainer);

        Task<StorePhoneModel> UpdatePhoneModelAsync(UpdatePhoneModelRequest updateRequest, CloudBlobContainer blobContainer, HttpPostedFile photo);
    }
}
