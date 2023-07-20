using ContactHubApi.Controllers;
using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;
using ContactHubApi.Services.Addresses;
using ContactHubApi.Services.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactHubApiTests.Controllers
{
    public class AddressesControllerTests
    {
        private readonly AddressesController _controller;
        private readonly Mock<IAddressService> _fakeAddressService;
        private readonly Mock<ILogger<AddressesController>> _fakeLogger;
        private readonly Mock<IContactService> _fakeContactService;

        public AddressesControllerTests()
        {
            _fakeAddressService = new Mock<IAddressService>();
            _fakeLogger = new Mock<ILogger<AddressesController>>();
            _fakeContactService = new Mock<IContactService>();
            _controller = new AddressesController(
                _fakeAddressService.Object,
                _fakeLogger.Object,
                _fakeContactService.Object);
        }

        // CreateAddress Tests
        [Fact]
        public async Task CreateAddress_AddressCreated_ReturnsCreated()
        {
            //Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ReturnsAsync(new ContactAddressDto());

            _fakeAddressService.Setup(service => service.CreateAddress(addressCreationDto))
                                .ReturnsAsync(new Address());

            // Act
            var result = await _controller.CreateAddress(addressCreationDto);

            //Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(201, createdAtRouteResult.StatusCode);
        }

        [Fact]
        public async Task CreateAddress_ContactDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ReturnsAsync((ContactAddressDto?)null);

            // Act
            var result = await _controller.CreateAddress(addressCreationDto);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateAddress_GetContactByIdServerError_ReturnsInternalServerError()
        {
            //Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateAddress(addressCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task CreateAddress_CreateServerError_ReturnsInternalServerError()
        {
            //Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ReturnsAsync(new ContactAddressDto());

            _fakeAddressService.Setup(service => service.CreateAddress(addressCreationDto))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateAddress(addressCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // GetUserById Tests
        [Fact]
        public async Task GetAddress_AddressExists_ReturnsOk()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                             .ReturnsAsync(new AddressDto());

            //Act
            var result = await _controller.GetAddress(addressId);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetAddress_AddressDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                             .ReturnsAsync((AddressDto?)null);

            //Act
            var result = await _controller.GetAddress(addressId);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetAddress_ServerError_ReturnsInternalServerError()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.GetAddress(addressId);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // UpdateAddress Tests
        [Fact]
        public async Task UpdateAddress_AddressUpdated_ReturnsOk()
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
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ReturnsAsync(new ContactAddressDto());

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ReturnsAsync(new AddressDto());

            _fakeAddressService.Setup(service => service.UpdateAddress(addressId, addressCreationDto))
                                .ReturnsAsync(new AddressDto());

            // Act
            var result = await _controller.UpdateAddress(addressId, addressCreationDto);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateAddress_ContactDoesNotExists_ReturnsNotFound()
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
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ReturnsAsync((ContactAddressDto?)null);

            // Act
            var result = await _controller.UpdateAddress(addressId, addressCreationDto);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateAddress_GetContactByIdServerError_ReturnsInternalServerError()
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
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateAddress(addressId, addressCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdateAddress_GetAddressByIdServerError_ReturnsInternalServerError()
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
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ReturnsAsync(new ContactAddressDto());

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateAddress(addressId, addressCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdateAddress_UpdateServerError_ReturnsInternalServerError()
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
                ContactId = It.IsAny<Guid>()
            };

            _fakeContactService.Setup(service => service.GetContactById(addressCreationDto.ContactId))
                               .ReturnsAsync(new ContactAddressDto());

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ReturnsAsync(new AddressDto());

            _fakeAddressService.Setup(service => service.UpdateAddress(addressId, addressCreationDto))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateAddress(addressId, addressCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // DeleteAddress Tests
        [Fact]
        public async Task DeleteAddress_AddressDeleted_ReturnsOk()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ReturnsAsync(new AddressDto());

            _fakeAddressService.Setup(service => service.DeleteAddress(addressId))
                                .ReturnsAsync(true);

            //Act
            var result = await _controller.DeleteAddress(addressId);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAddress_AddressDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ReturnsAsync((AddressDto?)null);

            //Act
            var result = await _controller.DeleteAddress(addressId);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteAddress_GetAddressByIdServerError_ReturnsInternalServerError()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.DeleteAddress(addressId);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task DeleteAddress_DeleteServerError_ReturnsInternalServerError()
        {
            //Arrange
            var addressId = It.IsAny<Guid>();

            _fakeAddressService.Setup(service => service.GetAddressById(addressId))
                                .ReturnsAsync(new AddressDto());

            _fakeAddressService.Setup(service => service.DeleteAddress(addressId))
                                .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.DeleteAddress(addressId);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}
