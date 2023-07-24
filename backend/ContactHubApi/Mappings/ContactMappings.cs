using AutoMapper;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;

namespace ContactHubApi.Mappings
{
    public class ContactMappings : Profile
    {
        public ContactMappings()
        {
            CreateMap<ContactCreationDto, Contact>();
            CreateMap<Contact, ContactAddressDto>();
            CreateMap<Contact, ContactDto>();
        }
    }
}
