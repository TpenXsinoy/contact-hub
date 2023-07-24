using AutoMapper;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;
using ContactHubApi.Repositories.Contacts;
using ContactHubApi.Services.Contacts;
using Moq;

namespace ContactHubApiTests.Services
{
    public class IContactServiceTests
    {
        private readonly IContactService _contactService;
        private readonly Mock<IContactRepository> _fakeContactRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public IContactServiceTests()
        {
            _fakeContactRepository = new Mock<IContactRepository>();
            _fakeMapper = new Mock<IMapper>();
            _contactService = new ContactService(_fakeContactRepository.Object, _fakeMapper.Object);
        }

        // CreateContact Tests
        [Fact]
        public async Task CreateContact_ContactCreated_ReturnsContact()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserId = Guid.NewGuid()
            };

            var contactModel = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = contactCreationDto.FirstName,
                LastName = contactCreationDto.LastName,
                Addresses = new List<Address>(),
                UserId = contactCreationDto.UserId
            };

            _fakeMapper.Setup(m => m.Map<Contact>(contactCreationDto))
                        .Returns(contactModel);

            _fakeContactRepository.Setup(repo => repo.CreateContact(contactModel))
                                    .ReturnsAsync(contactModel.Id);

            // Act
            var result = await _contactService.CreateContact(contactCreationDto);

            // Assert
            Assert.Equal(contactModel, result);
            Assert.NotNull(result);
            Assert.IsType<Contact>(result);
        }

        [Fact]
        public async Task CreateContact_ConnectionError_ThrowsException()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserId = Guid.NewGuid()
            };

            var contactModel = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = contactCreationDto.FirstName,
                LastName = contactCreationDto.LastName,
                Addresses = new List<Address>(),
                UserId = contactCreationDto.UserId
            };

            _fakeMapper.Setup(m => m.Map<Contact>(contactCreationDto))
                        .Returns(contactModel);

            _fakeContactRepository.Setup(repo => repo.CreateContact(contactModel))
                                    .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _contactService.CreateContact(contactCreationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // DeleteContact Tests
        [Fact]
        public async Task DeleteContact_ContactDeleted_ReturnsTrue()
        {
            // Arrange
            var contactId = It.IsAny<Guid>();

            _fakeContactRepository.Setup(repo => repo.DeleteContact(contactId))
                                    .ReturnsAsync(true);

            // Act
            var result = await _contactService.DeleteContact(contactId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteContact_ContactNotDeleted_ReturnsFalse()
        {
            // Arrange
            var contactId = It.IsAny<Guid>();

            _fakeContactRepository.Setup(repo => repo.DeleteContact(contactId))
                                    .ReturnsAsync(false);

            // Act
            var result = await _contactService.DeleteContact(contactId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteContact_ConnectionError_ThrowsExceptiom()
        {
            // Arrange
            var contactId = It.IsAny<Guid>();

            _fakeContactRepository.Setup(repo => repo.DeleteContact(contactId))
                                   .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _contactService.DeleteContact(contactId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // GetAllContacts Tests
        [Fact]
        public async void GetAllContacts_HasContacts_ReturnsContacts()
        {
            // Arrange
            var userId = It.IsAny<Guid>();
            var contactId = It.IsAny<Guid>();

            var contactsModel = new List<Contact>
            {
                new Contact
                {
                    Id = contactId,
                    FirstName = "John",
                    LastName =  "Doe",
                    Addresses = new List<Address>(),
                    UserId = userId
                }
            };

            var contactsDto = new List<ContactDto>
            {
                new ContactDto
                {
                    Id = contactId,
                    FirstName = "John",
                    LastName =  "Doe"
                }
            };

            _fakeContactRepository.Setup(repo => repo.GetAllContacts(userId))
                                    .ReturnsAsync(contactsModel);

            _fakeMapper.Setup(m => m.Map<IReadOnlyCollection<ContactDto>>(contactsModel))
                        .Returns(contactsDto);

            // Act
            var result = await _contactService.GetAllContacts(userId);

            // Assert
            Assert.NotEmpty(result);
            Assert.NotNull(result);
            Assert.IsType<List<ContactDto>>(result);
            Assert.Equal(contactsModel.Count, result.Count);
        }

        [Fact]
        public async void GetAllContacts_HasNoContacts_ReturnsEmpty()
        {
            // Arrange
            var userId = It.IsAny<Guid>();
            var contactsModel = new List<Contact>();
            var contactsDto = new List<ContactDto>();

            _fakeContactRepository.Setup(repo => repo.GetAllContacts(userId))
                                    .ReturnsAsync(contactsModel);

            _fakeMapper.Setup(m => m.Map<IReadOnlyCollection<ContactDto>>(contactsModel))
                        .Returns(contactsDto);

            // Act
            var result = await _contactService.GetAllContacts(userId);

            // Assert
            Assert.Empty(result);
            Assert.NotNull(result);
            Assert.IsType<List<ContactDto>>(result);
            Assert.Equal(contactsModel.Count, result.Count);
        }

        [Fact]
        public async void GetAllContacts_ConnectionError_ThrowsException()
        {
            // Arrange
            var userId = It.IsAny<Guid>();

            _fakeContactRepository.Setup(repo => repo.GetAllContacts(userId))
                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _contactService.GetAllContacts(userId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // GetContactById Tests
        [Fact]
        public async void GetContactById_HasContact_ReturnsContact()
        {
            // Arrange
            var contactId = It.IsAny<Guid>();

            var contactModel = new Contact
            {
                Id = contactId,
                FirstName = "John",
                LastName = "Doe",
                Addresses = new List<Address>()
                {
                    new Address
                    {
                        Id = Guid.NewGuid(),
                        AddressType = "Home",
                        Street = "123 Main St",
                        City = "City",
                        State = "State",
                        PostalCode = "12345",
                        ContactId = contactId
                    }
                },
                UserId = Guid.NewGuid()
            };

            var contactAddressDto = new ContactAddressDto
            {
                Id = contactModel.Id,
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName,
                Addresses = contactModel.Addresses
            };

            _fakeContactRepository.Setup(repo => repo.GetContactById(contactId))
                                    .ReturnsAsync(contactModel);

            _fakeMapper.Setup(m => m.Map<ContactAddressDto>(contactModel))
                        .Returns(contactAddressDto);

            // Act
            var result = await _contactService.GetContactById(contactId);

            // Assert
            Assert.Equal(contactAddressDto, result);
            Assert.NotNull(result);
            Assert.IsType<ContactAddressDto>(result);
        }

        [Fact]
        public async void GetContactById_ContactNotFound_ReturnsNull()
        {
            // Arrange
            var contactId = It.IsAny<Guid>();

            Contact? contactModel = null;
            ContactAddressDto? contactAddressDto = null;


            _fakeContactRepository.Setup(repo => repo.GetContactById(contactId))
                                    .ReturnsAsync(contactModel);

            _fakeMapper.Setup(m => m.Map<ContactAddressDto>(contactModel))
                        .Returns(contactAddressDto);

            // Act
            var result = await _contactService.GetContactById(contactId);

            // Assert
            Assert.Equal(contactAddressDto, result);
            Assert.Null(result);
        }

        [Fact]
        public async void GetContactById_ConnectionError_ThrowsException()
        {
            // Arrange
            var contactId = It.IsAny<Guid>();

            _fakeContactRepository.Setup(repo => repo.GetContactById(contactId))
                                    .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _contactService.GetContactById(contactId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // UpdateContact Tests
        [Fact]
        public async Task UpdateContact_ContactUpdated_ReturnsContactDto()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserId = Guid.NewGuid()
            };

            var contactModel = new Contact
            {
                Id = contactId,
                FirstName = contactCreationDto.FirstName,
                LastName = contactCreationDto.LastName,
                Addresses = new List<Address>(),
                UserId = contactCreationDto.UserId
            };

            var contactDto = new ContactDto
            {
                Id = contactModel.Id,
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName,
            };

            _fakeMapper.Setup(m => m.Map<Contact>(contactCreationDto))
                        .Returns(contactModel);

            _fakeContactRepository.Setup(repo => repo.UpdateContact(contactModel))
                                    .ReturnsAsync(true);

            _fakeMapper.Setup(m => m.Map<ContactDto>(contactModel))
                        .Returns(contactDto);

            // Act
            var result = await _contactService.UpdateContact(contactId, contactCreationDto);

            // Assert
            Assert.Equal(contactDto, result);
            Assert.NotNull(result);
            Assert.IsType<ContactDto>(result);
        }

        [Fact]
        public async Task UpdateContact_ContactNotUpdated_ReturnsEmptyContactDto()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();
            var contactCreationDto = new ContactCreationDto();
            var contactModel = new Contact();
            var contactDto = new ContactDto();

            _fakeMapper.Setup(m => m.Map<Contact>(contactCreationDto))
                        .Returns(contactModel);

            _fakeContactRepository.Setup(repo => repo.UpdateContact(contactModel))
                                    .ReturnsAsync(false);

            _fakeMapper.Setup(m => m.Map<ContactDto>(contactModel))
                        .Returns(contactDto);

            // Act
            var result = await _contactService.UpdateContact(contactId, contactCreationDto);

            // Assert
            Assert.Equal(contactDto, result);
            Assert.NotNull(result);
            Assert.IsType<ContactDto>(result);
        }

        [Fact]
        public async Task UpdateContact_ConnectionError_ThrowsException()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();
            var contactCreationDto = new ContactCreationDto();
            var contactModel = new Contact();

            _fakeMapper.Setup(m => m.Map<Contact>(contactCreationDto))
                        .Returns(contactModel);

            _fakeContactRepository.Setup(repo => repo.UpdateContact(contactModel))
                                    .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _contactService.UpdateContact(contactId, contactCreationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }
    }
}
