using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using KatlaSport.DataAccess;
using KatlaSport.DataAccess.PhoneStore;
using Microsoft.WindowsAzure.Storage.Blob;

namespace KatlaSport.Services.PhoneManagement
{
    public class PhoneModelService : IPhoneModelService
    {
        private readonly IProductStorePhoneContext _context;

        public PhoneModelService(IProductStorePhoneContext context, IUserContext userContext)
        {
            _context = context;
        }

        public async Task<StorePhoneModel> CreateModelsAsync(UpdatePhoneModelRequest createRequest, int phoneId, CloudBlobContainer blobContainer, HttpPostedFile photo)
        {
            var dbModels = await _context.Models.Where(h => h.Code == createRequest.Code).ToArrayAsync();
            if (dbModels.Length > 0)
            {
                throw new RequestedResourceHasConflictException("code");
            }

            StorePhoneModel dbModel = Mapper.Map<UpdatePhoneModelRequest, StorePhoneModel>(createRequest);
            dbModel.StorePhoneId = phoneId;

            var photoName = GetRandomBlobName(photo.FileName);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(photoName);
            using (var fileStream = photo.InputStream)
            {
                await blob.UploadFromStreamAsync(fileStream);
            }

            dbModel.UrlPhoto = blob.Uri.ToString();

            _context.Models.Add(dbModel);

            await _context.SaveChangesAsync();
            return Mapper.Map<StorePhoneModel>(dbModel);
        }

        public async Task DeleteModelAsync(int modelId, CloudBlobContainer blobContainer)
        {
            var dbModels = await _context.Models.Where(p => p.Id == modelId).ToArrayAsync();
            if (dbModels.Length == 0)
            {
                throw new RequestedResourceNotFoundException();
            }

            var dbModel = dbModels[0];
            string photpName = Path.GetFileName(dbModel.UrlPhoto);
            var blobDelete = blobContainer.GetBlockBlobReference(photpName);
            await blobDelete.DeleteIfExistsAsync();

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
            var dbModels = await _context.Models.Where(s => s.Id == modelId).ToArrayAsync();
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

        public async Task<StorePhoneModel> UpdatePhoneModelAsync(UpdatePhoneModelRequest updateRequest, CloudBlobContainer blobContainer, HttpPostedFile photo)
        {
            var dbModels = _context.Models.Where(p => p.Id != updateRequest.Id).ToArray();
            if (dbModels.Length > 0)
            {
                throw new RequestedResourceHasConflictException("code");
            }

            dbModels = await _context.Models.Where(p => p.Id == updateRequest.Id).ToArrayAsync();
            if (dbModels.Length == 0)
            {
                throw new RequestedResourceNotFoundException();
            }

            var dbModel = dbModels[0];

            string filename = Path.GetFileName(dbModel.UrlPhoto);

            var blobDelete = blobContainer.GetBlockBlobReference(filename);
            await blobDelete.DeleteIfExistsAsync();

            var photoName = GetRandomBlobName(photo.FileName);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(photoName);
            using (var fileStream = photo.InputStream)
            {
                await blob.UploadFromStreamAsync(fileStream);
            }

            updateRequest.UrlPhoto = blob.Uri.ToString();
            Mapper.Map(updateRequest, dbModel);

            await _context.SaveChangesAsync();

            return Mapper.Map<StorePhoneModel>(dbModel);
        }

        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
       }
    }
}
