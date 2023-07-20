using AutoMapper;
using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;
using ContactHubApi.Repositories.Addresses;
using ContactHubApi.Services.Addresses;
using Moq;

namespace ContactHubApiTests.Services
{
    public class IAddressServiceTests
    {
        private readonly IAddressService _addressService;
        private readonly Mock<IAddressRepository> _fakeAddressRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public IAddressServiceTests()
        {
            _fakeAddressRepository = new Mock<IAddressRepository>();
            _fakeMapper = new Mock<IMapper>();
            _addressService = new AddressService(_fakeAddressRepository.Object, _fakeMapper.Object);
        }

        [Fact]
        public async Task CreateAddress_AddressCreated_ReturnsCreatedAddress()
        {
            //Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = Guid.NewGuid()
            };

            var addressModel = new Address
            {
                Id = Guid.NewGuid(),
                AddressType = addressCreationDto.AddressType,
                Street = addressCreationDto.Street,
                City = addressCreationDto.City,
                State = addressCreationDto.State,
                PostalCode = addressCreationDto.PostalCode,
                ContactId = addressCreationDto.ContactId
            };

            _fakeMapper.Setup(m => m.Map<Address>(addressCreationDto))
               .Returns(addressModel);

            _fakeAddressRepository.Setup(repo => repo.CreateAddress(addressModel))
            .ReturnsAsync(addressModel.Id);

            // Act
            var result = await _addressService.CreateAddress(addressCreationDto);

            // Assert
            Assert.Equal(addressModel, result);
            Assert.NotNull(result);
            Assert.IsType<Address>(result);
        }

        [Fact]
        public async Task CreateAddress_ConnectionError_ThrowsException()
        {
            //Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = Guid.NewGuid()
            };

            var addressModel = new Address
            {
                Id = Guid.NewGuid(),
                AddressType = addressCreationDto.AddressType,
                Street = addressCreationDto.Street,
                City = addressCreationDto.City,
                State = addressCreationDto.State,
                PostalCode = addressCreationDto.PostalCode,
                ContactId = addressCreationDto.ContactId
            };

            _fakeMapper.Setup(m => m.Map<Address>(addressCreationDto))
                        .Returns(addressModel);

            _fakeAddressRepository.Setup(repo => repo.CreateAddress(addressModel))
                                    .Throws(new Exception("Database connection error"));

            //Act
            var result = await Assert.ThrowsAsync<Exception>(() => _addressService.CreateAddress(addressCreationDto));

            //Assert
            Assert.Equal("Database connection error", result.Message);
        }

        [Fact]
        public async Task DeleteAddress_AddressDeleted_ReturnsTrue()
        {
            // Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressRepository.Setup(repo => repo.DeleteAddress(addressId))
                                    .ReturnsAsync(true);

            // Act
            var result = await _addressService.DeleteAddress(addressId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAddress_AddressNotDeleted_ReturnsFalse()
        {
            // Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressRepository.Setup(repo => repo.DeleteAddress(addressId))
                                    .ReturnsAsync(false);

            // Act
            var result = await _addressService.DeleteAddress(addressId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAddress_ConnectionError_ThrowsExceptiom()
        {
            // Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressRepository.Setup(repo => repo.DeleteAddress(addressId))
                                   .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _addressService.DeleteAddress(addressId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        [Fact]
        public async Task GetAddressById_HasAddress_ReturnsAddress()
        {
            // Arrange
            var addressId = It.IsAny<Guid>();

            var addressDto = new AddressDto
            {
                Id = addressId,
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
            };

            var addressModel = new Address
            {
                Id = addressId,
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = It.IsAny<Guid>()
            };

            _fakeAddressRepository.Setup(repo => repo.GetAddressById(addressId))
                                    .ReturnsAsync(addressModel);

            _fakeMapper.Setup(m => m.Map<AddressDto>(addressModel))
                        .Returns(addressDto);

            // Act
            var result = await _addressService.GetAddressById(addressId);

            // Assert
            Assert.Equal(addressDto, result);
            Assert.NotNull(result);
            Assert.IsType<AddressDto>(result);
        }

        [Fact]
        public async Task GetAddressById_AddressNotFound_ReturnsNull()
        {
            // Arrange
            var addressId = It.IsAny<Guid>();

            AddressDto? addressDto = null;
            Address? addressModel = null;

            _fakeAddressRepository.Setup(repo => repo.GetAddressById(addressId))
                                    .ReturnsAsync(addressModel);

            _fakeMapper.Setup(m => m.Map<AddressDto>(addressModel))
                        .Returns(addressDto);

            // Act
            var result = await _addressService.GetAddressById(addressId);

            // Assert
            Assert.Equal(addressDto, result);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAddressById_ConnectionError_ThrowsException()
        {
            // Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressRepository.Setup(repo => repo.GetAddressById(addressId))
                                   .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _addressService.GetAddressById(addressId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        [Fact]
        public async Task UpdatedAddress_AddressUpdated_ReturnsAddressDto()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = Guid.NewGuid()
            };

            var addressModel = new Address
            {
                Id = addressId,
                AddressType = addressCreationDto.AddressType,
                Street = addressCreationDto.Street,
                City = addressCreationDto.City,
                State = addressCreationDto.State,
                PostalCode = addressCreationDto.PostalCode,
                ContactId = addressCreationDto.ContactId
            };

            var addressDto = new AddressDto
            {
                Id = addressModel.Id,
                AddressType = addressModel.AddressType,
                Street = addressModel.Street,
                City = addressModel.City,
                State = addressModel.State,
                PostalCode = addressModel.PostalCode
            };

            _fakeMapper.Setup(m => m.Map<Address>(addressCreationDto))
                        .Returns(addressModel);

            _fakeAddressRepository.Setup(repo => repo.UpdateAddress(addressModel))
                                    .ReturnsAsync(true);

            _fakeMapper.Setup(m => m.Map<AddressDto>(addressModel))
                        .Returns(addressDto);

            // Act
            var result = await _addressService.UpdateAddress(addressId, addressCreationDto);

            // Assert
            Assert.Equal(addressDto, result);
            Assert.NotNull(result);
            Assert.IsType<AddressDto>(result);
        }

        [Fact]
        public async Task UpdatedAddress_AddressNotUpdated_ReturnsEmptyAddressDto()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();
            var addressCreationDto = new AddressCreationDto();
            var addressModel = new Address();
            var addressDto = new AddressDto();

            _fakeMapper.Setup(m => m.Map<Address>(addressCreationDto))
                        .Returns(addressModel);

            _fakeAddressRepository.Setup(repo => repo.UpdateAddress(addressModel))
                                    .ReturnsAsync(false);

            _fakeMapper.Setup(m => m.Map<AddressDto>(addressModel))
                        .Returns(addressDto);

            // Act
            var result = await _addressService.UpdateAddress(addressId, addressCreationDto);

            // Assert
            Assert.Equal(addressDto, result);
            Assert.NotNull(result);
            Assert.IsType<AddressDto>(result);
        }

        [Fact]
        public async Task UpdatedAddress_ConnectionError_ThrowsException()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();
            var addressCreationDto = new AddressCreationDto();
            var addressModel = new Address();

            _fakeMapper.Setup(m => m.Map<Address>(addressCreationDto))
                        .Returns(addressModel);

            _fakeAddressRepository.Setup(repo => repo.UpdateAddress(addressModel))
                                    .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _addressService.UpdateAddress(addressId, addressCreationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }
    }
}
