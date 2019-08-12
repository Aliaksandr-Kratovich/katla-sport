
namespace KatlaSport.Services.PhoneManagement
{
    public class UpdatePhoneModelRequest
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Code { get; set; }

        public string UrlPhoto { get; set; }

        public int StorePhoneId { get; set; }
    }
}
