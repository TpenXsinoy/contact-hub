using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;
using ContactHubApi.Repositories.Users;

namespace ContactHubApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUser(UserCreationDto user)
        {
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userModel = _mapper.Map<User>(user);

            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            userModel.Id = await _userRepository.CreateUser(userModel);

            var userDto = _mapper.Map<UserDto>(userModel);

            return userDto;
        }

        public async Task<UserUIDetailsDto?> GetUserById(Guid id)
        {
            var userModel = await _userRepository.GetUserById(id);
            if (userModel == null)
            {
                return null;
            }

            return _mapper.Map<UserUIDetailsDto>(userModel);
        }

        public async Task<UserUIDetailsDto?> GetUserByUsername(string username)
        {
            var userModel = await _userRepository.GetUserByUsername(username);
            if (userModel == null)
            {
                return null;
            }

            return _mapper.Map<UserUIDetailsDto>(userModel);
        }

        public async Task<UserTokenDto?> GetUserByUsernameWithToken(string username)
        {
            var userModel = await _userRepository.GetUserByUsername(username);
            if (userModel == null)
            {
                return null;
            }

            return _mapper.Map<UserTokenDto>(userModel);
        }

        public async Task<bool> IsUserEmailExist(string email)
        {
            return await _userRepository.IsUserEmailExist(email);
        }

        public async Task<bool> IsUsernameExist(string username)
        {
            return await _userRepository.IsUsernameExist(username);
        }

        public async Task<UserUIDetailsDto> UpdateUser(Guid id, UserCreationDto user)
        {
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userModel = _mapper.Map<User>(user);

            userModel.Id = id;
            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            await _userRepository.UpdateUser(userModel);

            var userDto = _mapper.Map<UserUIDetailsDto>(userModel);

            return userDto;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public UserTokenDto? GetCurrentUser(HttpContext identity)
        {
            if (identity != null && identity.User.Identity is ClaimsIdentity claimsIdentity)
            {
                var userClaims = claimsIdentity.Claims;

                return new UserTokenDto
                {
                    FirstName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    LastName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                };
            }
            return null;
        }
    }
}
