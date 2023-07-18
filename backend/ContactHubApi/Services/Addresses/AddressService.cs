using AutoMapper;
using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Models;
using ContactHubApi.Repositories.Addresses;

namespace ContactHubApi.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository repository, IMapper mapper)
        {
            _addressRepository = repository;
            _mapper = mapper;
        }
        public async Task<Address> CreateAddress(AddressCreationDto address)
        {
            var addressModel = _mapper.Map<Address>(address);
            addressModel.Id = await _addressRepository.CreateAddress(addressModel);

            return addressModel;
        }

        public async Task<bool> DeleteAddress(Guid id)
        {
            return await _addressRepository.DeleteAddress(id);
        }

        public async Task<AddressDto?> GetAddressById(Guid id)
        {
            var addressModel = await _addressRepository.GetAddressById(id);
            if (addressModel == null) return null;

            return _mapper.Map<AddressDto>(addressModel);
        }

        public async Task<AddressDto> UpdateAddress(Guid id, AddressCreationDto address)
        {
            var addressModel = _mapper.Map<Address>(address);
            addressModel.Id = id;

            await _addressRepository.UpdateAddress(addressModel);

            var addressDto = _mapper.Map<AddressDto>(addressModel);

            return addressDto;
        }
    }
}
