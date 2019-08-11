using System;
using System.Collections.Generic;

namespace KatlaSport.Services.PhoneManagement
{
    public class StorePhone
    {
        public int Id { get; set; }

        public string Mark { get; set; }

        public string Country { get; set; }

        public DateTime LastUpdated { get; set; }

        public virtual ICollection<StorePhoneModel> Models { get; set; }
    }
}
