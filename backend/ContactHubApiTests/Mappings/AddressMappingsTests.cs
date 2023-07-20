using AutoMapper;
using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Mappings;
using ContactHubApi.Models;
using Moq;

namespace ContactHubApiTests.Mappings
{
    public class AddressMappingsTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;

        public AddressMappingsTests()
        {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile<AddressMappings>());
            _mapper = _configuration.CreateMapper();
        }

        // AddressCreationDto to Address
        [Fact]
        public void Map_ValidAddressCreationDto_ReturnsAddress()
        {
            //Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = It.IsAny<Guid>(),
            };

            var address = new Address
            {
                Id = It.IsAny<Guid>(),
                AddressType = addressCreationDto.AddressType,
                Street = addressCreationDto.Street,
                City = addressCreationDto.City,
                State = addressCreationDto.State,
                PostalCode = addressCreationDto.PostalCode,
                ContactId = addressCreationDto.ContactId
            };

            //Act
            var result = _mapper.Map<Address>(addressCreationDto);

            //Assert
            Assert.Equal(address.Id, result.Id);
            Assert.Equal(address.AddressType, result.AddressType);
            Assert.Equal(address.Street, result.Street);
            Assert.Equal(address.City, result.City);
            Assert.Equal(address.PostalCode, result.PostalCode);
            Assert.Equal(address.ContactId, result.ContactId);

        }

        // Address to AddressDto
        [Fact]
        public void Map_ValidAddress_ReturnsAddressDto()
        {
            //Arrange
            var address = new Address
            {
                Id = It.IsAny<Guid>(),
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = It.IsAny<Guid>(),
            };

            var addressDto = new AddressDto
            {
                Id = address.Id,
                AddressType = address.AddressType,
                Street = address.Street,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode
            };

            //Act
            var result = _mapper.Map<AddressDto>(address);

            //Assert
            Assert.Equal(addressDto.Id, result.Id);
            Assert.Equal(addressDto.AddressType, result.AddressType);
            Assert.Equal(addressDto.Street, result.Street);
            Assert.Equal(addressDto.City, result.City);
            Assert.Equal(addressDto.PostalCode, result.PostalCode);
        }
    }
}
