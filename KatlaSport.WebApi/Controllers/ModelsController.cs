using KatlaSport.Services.PhoneManagement;
using KatlaSport.WebApi.CustomFilters;
using Microsoft.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KatlaSport.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/models")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomExceptionFilter]
    [SwaggerResponseRemoveDefaults]
    public class ModelsController : ApiController
    {
        private readonly IPhoneModelService _phoneModelService;
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        public ModelsController(IPhoneModelService phoneModelService)
        {
            _phoneModelService = phoneModelService ?? throw new ArgumentNullException(nameof(phoneModelService));
            var storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"].ToString();
            storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("webappstoragedotnet-imagecontainer");
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of models.", Type = typeof(StorePhoneModel[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetModelsAsync()
        {
            var models = await _phoneModelService.GetModelsAsync();
            return Ok(models);
        }

        [HttpGet]
        [Route("{modelId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a  model.", Type = typeof(StorePhoneModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetModelAsync(int modelId)
        {
            var model = await _phoneModelService.GetModelAsync(modelId);
            return Ok(model);
        }


        [HttpPost]
        [Route("{phoneId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> AddModel([FromUri] int phoneId)
        {
            var httprequest = HttpContext.Current.Request;
            var photo = httprequest.Files["Image"];
            var data = httprequest.Form["phoneModel"];
            var request = JsonConvert.DeserializeObject<UpdatePhoneModelRequest>(data);
           // var storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"].ToString();
           // CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
           // CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
           // CloudBlobContainer blobContainer = blobClient.GetContainerReference("webappstoragedotnet-imagecontainer");
            await blobContainer.CreateIfNotExistsAsync();
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });          

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var model = await _phoneModelService.CreateModelsAsync(request, phoneId, blobContainer, photo);
            var location = string.Format($"/api/models/{model.Id}");
            return Created(location, model);
        }

        [HttpPut]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> UpdateModel()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var httprequest = HttpContext.Current.Request;
            var photo = httprequest.Files["Image"];
            var data = httprequest.Form["phoneModel"];
            var request = JsonConvert.DeserializeObject<UpdatePhoneModelRequest>(data);
           // CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"].ToString());
           // CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
          //  CloudBlobContainer blobContainer = blobClient.GetContainerReference("webappstoragedotnet-imagecontainer");
            await blobContainer.CreateIfNotExistsAsync();
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            await _phoneModelService.UpdatePhoneModelAsync( request, blobContainer, photo);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> DeleteModel([FromUri] int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"].ToString());

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("webappstoragedotnet-imagecontainer");
            await blobContainer.CreateIfNotExistsAsync();
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            await _phoneModelService.DeleteModelAsync(id, blobContainer);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));

        }
    }
}