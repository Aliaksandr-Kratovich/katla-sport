
namespace KatlaSport.Services.PhoneManagement
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using KatlaSport.DataAccess;
    using KatlaSport.DataAccess.PhoneStore;

    public class PhoneModelService : IPhoneModelService
    {
        private readonly IProductStorePhoneContext _context;

        public PhoneModelService(IProductStorePhoneContext context, IUserContext userContext)
        {
            _context = context;
        }

        public async Task<StorePhoneModel> CreateModelsAsync(UpdatePhoneModelRequest createRequest, int phoneId)
        {
            var dbModels = await _context.Models.Where(h => h.Code == createRequest.Code).ToArrayAsync();
            if (dbModels.Length > 0)
            {
                throw new RequestedResourceHasConflictException("code");
            }

            StorePhoneModel dbModel = Mapper.Map<UpdatePhoneModelRequest, StorePhoneModel>(createRequest);
            dbModel.StorePhoneId = phoneId;
            _context.Models.Add(dbModel);

            await _context.SaveChangesAsync();
            return Mapper.Map<StorePhoneModel>(dbModel);
        }

        public async Task DeleteModelAsync(int modelId)
        {
            var dbModels = await _context.Models.Where(p => p.Id == modelId).ToArrayAsync();
            if (dbModels.Length == 0)
            {
                throw new RequestedResourceNotFoundException();
            }

            var dbModel = dbModels[0];

            _context.Models.Remove(dbModel);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UpdatePhoneModelRequest>> GetModelsAsync()
        {
            var dbModels = await _context.Models.OrderBy(s => s.Id).ToArrayAsync();
            var models = dbModels.Select(s => Mapper.Map<UpdatePhoneModelRequest>(s)).ToList();
            return models;
        }

        public async Task<UpdatePhoneModelRequest> GetModelAsync(int modelId)
        {
            var dbModels = await _context.Models.Where(s => s.StorePhoneId == modelId).ToArrayAsync();
            if (dbModels.Length == 0)
            {
                throw new RequestedResourceNotFoundException();
            }

            return Mapper.Map<StorePhoneModel, UpdatePhoneModelRequest>(dbModels[0]);
        }

        public async Task<List<UpdatePhoneModelRequest>> GetModelsAsync(int phoneId)
        {
            var dbModels = await _context.Models.Where(s => s.StorePhoneId == phoneId).OrderBy(s => s.Id).ToArrayAsync();
            var models = dbModels.Select(s => Mapper.Map<UpdatePhoneModelRequest>(s)).ToList();
            return models;
        }
    }
}
