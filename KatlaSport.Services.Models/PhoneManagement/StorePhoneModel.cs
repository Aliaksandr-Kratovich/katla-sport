using System;

namespace KatlaSport.Services.PhoneManagement
{
    public class StorePhoneModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public StorePhone StorePhone { get; set; }

        public int StorePhoneId { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
