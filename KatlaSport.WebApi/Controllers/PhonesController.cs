using KatlaSport.Services.PhoneManagement;
using KatlaSport.WebApi.CustomFilters;
using Microsoft.Web.Http;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KatlaSport.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/phones")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [CustomExceptionFilter]
    [SwaggerResponseRemoveDefaults]
    public class PhonesController : ApiController
    {
        private readonly IPhoneService _phoneService;
        private readonly IPhoneModelService _phoneModelService;

        public PhonesController(IPhoneService phoneService, IPhoneModelService phoneModelService)
        {
            _phoneService = phoneService ?? throw new ArgumentNullException(nameof(phoneService));
            _phoneModelService = phoneModelService ?? throw new ArgumentNullException(nameof(phoneModelService));
        }

        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of phones.", Type = typeof(UpdatePhoneRequest[]))]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetPhones()
        {
            var phones = await _phoneService.GetPhonesAsync();
            return Ok(phones);
        }

        [HttpGet]
        [Route("{phoneId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a phone.", Type = typeof(StorePhone))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetPhone(int phoneId)
        {
            var phone = await _phoneService.GetPhoneAsync(phoneId);
            return Ok(phone);
        }

        [HttpGet]
        [Route("{phoneId:int:min(1)}/models")]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Returns a list of phone sections for specified phone.", Type = typeof(StorePhoneModel))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> GetPhoneModels(int phoneId)
        {
            var models = await _phoneModelService.GetModelsAsync(phoneId);
            return Ok(models);
        }

        

        [HttpPost]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> AddPhone([FromBody] UpdatePhoneRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var phone = await _phoneService.CreatePhoneAsync(createRequest);
            var location = string.Format($"/api/phones/{phone.Id}");
            return Created<StorePhone>(location, phone);
        }

        [HttpPut]
        [Route("{phoneId:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> UpdatePhone([FromUri] int phoneId, [FromBody] UpdatePhoneRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _phoneService.UpdatePhoneAsync(phoneId, createRequest);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        [HttpDelete]
        [Route("{id:int:min(1)}")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> DeletePhone([FromUri] int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            await _phoneService.DeletePhoneAsync(id);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }
    }
}