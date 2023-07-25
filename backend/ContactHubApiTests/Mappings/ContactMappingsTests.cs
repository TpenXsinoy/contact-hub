using AutoMapper;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Mappings;
using ContactHubApi.Models;
using Moq;

namespace ContactHubApiTests.Mappings
{
    public class ContactMappingsTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;

        public ContactMappingsTests()
        {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile<ContactMappings>());
            _mapper = _configuration.CreateMapper();
        }

        // ContactCreationDto to Contact
        [Fact]
        public void Map_ValidContactCreationDto_ReturnsContact()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                UserId = It.IsAny<Guid>(),
            };

            var contact = new Contact
            {
                Id = It.IsAny<Guid>(),
                FirstName = contactCreationDto.FirstName,
                LastName = contactCreationDto.LastName,
                PhoneNumber = contactCreationDto.PhoneNumber,
                Addresses = new List<Address>(),
                UserId = contactCreationDto.UserId
            };

            //Act
            var result = _mapper.Map<Contact>(contactCreationDto);

            //Assert
            Assert.Equal(contact.Id, result.Id);
            Assert.Equal(contact.FirstName, result.FirstName);
            Assert.Equal(contact.LastName, result.LastName);
            Assert.Equal(contact.Addresses, result.Addresses);
            Assert.Equal(contact.UserId, result.UserId);
        }

        // Contact to ContactDto
        [Fact]
        public void Map_ValidContact_ReturnsContactDto()
        {
            //Arrange
            var contact = new Contact
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Addresses = new List<Address>(),
                UserId = It.IsAny<Guid>()
            };

            var contactDto = new ContactDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber
            };

            //Act
            var result = _mapper.Map<ContactDto>(contact);

            //Assert
            Assert.Equal(contactDto.Id, result.Id);
            Assert.Equal(contactDto.FirstName, result.FirstName);
            Assert.Equal(contactDto.LastName, result.LastName);
        }
    }
}
