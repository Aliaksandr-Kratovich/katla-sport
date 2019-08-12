using AutoMapper;
using System;
using DataAccessPhone = KatlaSport.Services.PhoneManagement.StorePhone;
using DataAccessPhoneModel = KatlaSport.Services.PhoneManagement.StorePhoneModel;

namespace KatlaSport.Services.PhoneManagement
{
    public class PhoneManagementMappingProfile : Profile
    {
        public PhoneManagementMappingProfile()
        {
            CreateMap<UpdatePhoneRequest, DataAccessPhone>();
            CreateMap<DataAccessPhone, UpdatePhoneRequest>();
            CreateMap<DataAccessPhoneModel, UpdatePhoneModelRequest>();
            CreateMap<UpdatePhoneModelRequest, DataAccessPhoneModel>();
            CreateMap<UpdatePhoneRequest, DataAccessPhone>()
    .ForMember(r => r.LastUpdated, opt => opt.MapFrom(p => DateTime.UtcNow));
            CreateMap<UpdatePhoneModelRequest, DataAccessPhoneModel>()
    .ForMember(r => r.LastUpdated, opt => opt.MapFrom(p => DateTime.UtcNow));
        }
    }
}
