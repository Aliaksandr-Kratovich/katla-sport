namespace KatlaSport.Services.PhoneManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using KatlaSport.DataAccess;
    using KatlaSport.DataAccess.PhoneStore;

    public class PhoneService : IPhoneService
    {
        private readonly IProductStorePhoneContext _context;

        public PhoneService(IProductStorePhoneContext context, IUserContext userContext)
        {
            _context = context;
        }

        public async Task<StorePhone> CreatePhoneAsync(UpdatePhoneRequest createRequest)
        {
            var dbPhones = await _context.Phones.Where(p => (p.Mark == createRequest.Mark)).ToArrayAsync();
            if (dbPhones.Length > 0)
            {
                throw new RequestedResourceHasConflictException("code");
            }

            var phone = Mapper.Map<UpdatePhoneRequest, StorePhone>(createRequest);
            _context.Phones.Add(phone);

            await _context.SaveChangesAsync();

            return phone;
        }

        public async Task DeletePhoneAsync(int phoneId)
        {
            var dbPhones = await _context.Phones.Where(p => p.Id == phoneId).ToArrayAsync();
            if (dbPhones.Length == 0)
            {
                throw new RequestedResourceHasConflictException("code");
            }

            var dbPhone = dbPhones[0];

            _context.Phones.Remove(dbPhone);
            await _context.SaveChangesAsync();
        }

        public async Task<UpdatePhoneRequest> GetPhoneAsync(int phoneId)
        {
            var dbPhones = await _context.Phones.Where(p => p.Id == phoneId).ToArrayAsync();
            if (dbPhones.Length == 0)
            {
                throw new RequestedResourceNotFoundException();
            }

            return Mapper.Map<StorePhone, UpdatePhoneRequest >(dbPhones[0]);
        }

        public async Task<List<UpdatePhoneRequest>> GetPhonesAsync()
        {
            var dbPhones = await _context.Phones.OrderBy(h => h.Id).ToArrayAsync();
            var phones = dbPhones.Select(h => Mapper.Map<UpdatePhoneRequest>(h)).ToList();

            return phones;
        }

        public async Task<StorePhone> UpdatePhoneAsync(int phoneId, UpdatePhoneRequest updateRequest)
        {
            var dbPhones = await _context.Phones.Where(p => p.Id == phoneId).ToArrayAsync();
            if (dbPhones.Length == 0)
            {
                throw new RequestedResourceNotFoundException();
            }

            var dbPhone = dbPhones[0];

            Mapper.Map(updateRequest, dbPhone);

            await _context.SaveChangesAsync();

            return Mapper.Map<StorePhone>(dbPhone);
        }
    }
}