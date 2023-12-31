﻿using System.Security.Claims;
using ContactHubApi.Controllers;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;
using ContactHubApi.Services.Contacts;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactHubApiTests.Controllers
{
    public class ContactsControllerTests
    {
        private readonly ContactsController _controller;
        private readonly Mock<IContactService> _fakeContactService;
        private readonly Mock<ILogger<ContactsController>> _fakeLogger;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<HttpContext> _fakeContext;

        public ContactsControllerTests()
        {
            _fakeContactService = new Mock<IContactService>();
            _fakeLogger = new Mock<ILogger<ContactsController>>();
            _fakeUserService = new Mock<IUserService>();
            _fakeContext = new Mock<HttpContext>();
            _controller = new ContactsController(
                _fakeContactService.Object,
                _fakeLogger.Object,
                _fakeUserService.Object);
        }

        // CreateContact Tests
        [Fact]
        public async Task CreateContact_ContactCreated_ReturnsCreated()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };


            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ReturnsAsync(new UserUIDetailsDto());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier, contactCreationDto.UserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = contactCreationDto.UserId
                            });

            _fakeContactService.Setup(service => service.CreateContact(contactCreationDto))
                                .ReturnsAsync(new Contact());

            // Act
            var result = await _controller.CreateContact(contactCreationDto);

            //Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(201, createdAtRouteResult.StatusCode);
        }

        [Fact]
        public async Task CreateContact_UserDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ReturnsAsync((UserUIDetailsDto?)null);

            // Act
            var result = await _controller.CreateContact(contactCreationDto);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateContact_UserWithDifferentId_ReturnsUnauthorized()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = Guid.NewGuid()
            };

            var loggedInUserId = Guid.NewGuid();


            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ReturnsAsync(new UserUIDetailsDto());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,loggedInUserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = loggedInUserId
                            });

            // Act
            var result = await _controller.CreateContact(contactCreationDto);

            //Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateContact_UserNotAuthenticated_ReturnsUnauthorized()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = Guid.NewGuid()
            };

            var loggedInUserId = Guid.NewGuid();


            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ReturnsAsync(new UserUIDetailsDto());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,loggedInUserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns((UserTokenDto?)null);


            // Act
            var result = await _controller.CreateContact(contactCreationDto);

            //Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreateContact_GetUserByIdServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateContact(contactCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task CreateContact_CreateServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeContactService.Setup(service => service.CreateContact(contactCreationDto))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.CreateContact(contactCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // GetAllContacts Tests
        [Fact]
        public async Task GetAllContacts_HasContacts_ReturnsOk()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var username = "john.doe";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.Name, username)
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Username = username
                            });

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeContactService.Setup(service => service.GetAllContacts(userId))
                                .ReturnsAsync(new List<ContactDto>() { new ContactDto() });

            // Act
            var result = await _controller.GetAllContacts();

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var contacts = Assert.IsAssignableFrom<IEnumerable<ContactDto>>(okObjectResult.Value);
            Assert.Single(contacts);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetAllContacts_UserNotAuthenticated_ReturnsUnauthorized()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var username = "john.doe";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                new Claim(ClaimTypes.Name, username)
              }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns((UserTokenDto?)null);

            // Act
            var result = await _controller.GetAllContacts();

            //Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task GetAllContacts_HasNoContacts_ReturnsNoContent()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var username = "john.doe";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                new Claim(ClaimTypes.Name, username)
              }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Username = username
                            });

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeContactService.Setup(service => service.GetAllContacts(userId))
                                .ReturnsAsync(new List<ContactDto>());

            // Act
            var result = await _controller.GetAllContacts();

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetAllContacts_UserDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var username = "john.doe";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                new Claim(ClaimTypes.Name, username)
              }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Username = username
                            });

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                            .ReturnsAsync((UserUIDetailsDto?)null);

            // Act
            var result = await _controller.GetAllContacts();

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetAllContacts_GetUserByUsernameServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var username = "john.doe";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.Name, username)
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Username = username
                            });

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAllContacts();

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task GetAllContacts_GetAllServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var username = "john.doe";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
              {
                new Claim(ClaimTypes.Name, username)
              }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Username = username
                            });

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                           .ReturnsAsync(new UserUIDetailsDto());

            _fakeContactService.Setup(service => service.GetAllContacts(userId))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAllContacts();

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // GetContact Tests
        [Fact]
        public async Task GetContact_ContactExists_ReturnsOk()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();
            var userId = It.IsAny<Guid>();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync(new Contact());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString())
            }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = userId
                            });

            // Act
            var result = await _controller.GetContact(contacId);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetContact_ContactDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync((Contact?)null);

            // Act
            var result = await _controller.GetContact(contacId);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetContact_GettingOtherUserContact_ReturnsUnauthorized()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();
            var userId = Guid.NewGuid();
            var loggedInUserId = Guid.NewGuid();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync(new Contact()
                                {
                                    UserId = userId,
                                });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,loggedInUserId.ToString())
            }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = loggedInUserId
                            });

            // Act
            var result = await _controller.GetContact(contacId);

            //Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetContact_UserNotAuthenticated_ReturnsUnauthorized()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();
            var userId = Guid.NewGuid();
            var loggedInUserId = Guid.NewGuid();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync(new Contact());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,loggedInUserId.ToString())
            }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns((UserTokenDto?)null);

            // Act
            var result = await _controller.GetContact(contacId);

            //Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetContact_ServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetContact(contacId);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // UpdateContact Tests
        [Fact]
        public async Task UpdateContact_ContactUpdated_ReturnsOk()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ReturnsAsync(new UserUIDetailsDto()
                            {
                                Id = contactCreationDto.UserId
                            });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,contactCreationDto.UserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = contactCreationDto.UserId
                            });

            _fakeContactService.Setup(service => service.GetContactById(contactId))
                                .ReturnsAsync(new Contact());

            _fakeContactService.Setup(service => service.UpdateContact(contactId, contactCreationDto))
                                .ReturnsAsync(new ContactDto());

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_UserDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ReturnsAsync((UserUIDetailsDto?)null);

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_UserNotAuthenticated_ReturnsUnauthorized()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = contactCreationDto.UserId
                           });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,contactCreationDto.UserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns((UserTokenDto?)null);

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_UpdateOtherUserContact_ReturnsUnauthorized()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = Guid.NewGuid()
            };

            var loggedInUserId = Guid.NewGuid();

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = contactCreationDto.UserId
                           });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,contactCreationDto.UserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = loggedInUserId
                            });

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_ContactDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = contactCreationDto.UserId
                           });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,contactCreationDto.UserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = contactCreationDto.UserId
                            });

            _fakeContactService.Setup(service => service.GetContactById(contactId))
                                .ReturnsAsync((Contact?)null);

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_GetUserByIdServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_GetContactByIdServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = contactCreationDto.UserId
                           });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,contactCreationDto.UserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = contactCreationDto.UserId
                            });

            _fakeContactService.Setup(service => service.GetContactById(contactId))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdateContact_UpdateServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contactId = It.IsAny<Guid>();

            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                UserId = It.IsAny<Guid>()
            };

            _fakeUserService.Setup(service => service.GetUserById(contactCreationDto.UserId))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = contactCreationDto.UserId
                           });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.NameIdentifier,contactCreationDto.UserId.ToString())
             }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = contactCreationDto.UserId
                            });

            _fakeContactService.Setup(service => service.GetContactById(contactId))
                                .ReturnsAsync(new Contact());

            _fakeContactService.Setup(service => service.UpdateContact(contactId, contactCreationDto))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateContact(contactId, contactCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // DeleteContact Tests
        [Fact]
        public async Task DeleteContact_ContactDeleted_ReturnsOk()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();
            var userId = It.IsAny<Guid>();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync(new Contact()
                                {
                                    UserId = userId
                                });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString())
           }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = userId
                            });

            _fakeContactService.Setup(service => service.DeleteContact(contacId))
                                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteContact(contacId);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteContact_DeletingOtherUserContact_ReturnsUnauthorized()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();
            var userId = Guid.NewGuid();
            var loggedInUserId = Guid.NewGuid();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync(new Contact()
                                {
                                    UserId = userId,
                                });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,loggedInUserId.ToString())
            }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = loggedInUserId
                            });

            // Act
            var result = await _controller.DeleteContact(contacId);

            //Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteContact_UserNotAuthenticated_ReturnsUnauthorized()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();
            var userId = Guid.NewGuid();
            var loggedInUserId = Guid.NewGuid();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync(new Contact());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,loggedInUserId.ToString())
            }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns((UserTokenDto?)null);

            // Act
            var result = await _controller.DeleteContact(contacId);

            //Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteContact_ContactDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync((Contact?)null);

            // Act
            var result = await _controller.DeleteContact(contacId);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteContact_GetContactByIdServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.DeleteContact(contacId);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task DeleteContact_DeleteServerError_ReturnsInternalServerError()
        {
            //Arrange
            var contacId = It.IsAny<Guid>();
            var userId = It.IsAny<Guid>();

            _fakeContactService.Setup(service => service.GetContactById(contacId))
                                .ReturnsAsync(new Contact()
                                {
                                    UserId = userId
                                });

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString())
           }));

            _fakeContext.Setup(c => c.User).Returns(user);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _fakeContext.Object
            };

            _fakeUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsIdentity>()))
                            .Returns(new UserTokenDto()
                            {
                                Id = userId
                            });
            _fakeContactService.Setup(service => service.DeleteContact(contacId))
                                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.DeleteContact(contacId);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}
