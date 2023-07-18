using AutoMapper;
using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Models;

namespace ContactHubApi.Mappings
{
    public class AddressMappings : Profile
    {
        public AddressMappings()
        {
            CreateMap<AddressCreationDto, Address>();
            CreateMap<Address, AddressDto>();
        }
    }
}
