using AutoMapper;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;
using ContactHubApi.Repositories.Contacts;

namespace ContactHubApi.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository repository, IMapper mapper)
        {
            _contactRepository = repository;
            _mapper = mapper;
        }

        public async Task<Contact> CreateContact(ContactCreationDto contact)
        {
            var contactModel = _mapper.Map<Contact>(contact);
            contactModel.Id = await _contactRepository.CreateContact(contactModel);

            return contactModel;
        }

        public async Task<bool> DeleteContact(Guid id)
        {
            return await _contactRepository.DeleteContact(id);
        }

        public async Task<IReadOnlyCollection<ContactDto>> GetAllContacts(Guid userId)
        {
            var contactModels = await _contactRepository.GetAllContacts(userId);
            return _mapper.Map<IReadOnlyCollection<ContactDto>>(contactModels);
        }

        public async Task<ContactAddressDto?> GetContactById(Guid id)
        {
            var contactModel = await _contactRepository.GetContactById(id);
            if (contactModel == null) return null;

            return _mapper.Map<ContactAddressDto>(contactModel);
        }

        public async Task<ContactDto> UpdateContact(Guid id, ContactCreationDto contact)
        {
            var contactModel = _mapper.Map<Contact>(contact);
            contactModel.Id = id;

            await _contactRepository.UpdateContact(contactModel);

            var contactDto = _mapper.Map<ContactDto>(contactModel);

            return contactDto;
        }
    }
}
