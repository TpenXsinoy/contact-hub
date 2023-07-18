using AutoMapper;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;

namespace ContactHubApi.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserCreationDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserUIDetailsDto>();
            CreateMap<User, UserTokenDto>();
        }
    }
}
