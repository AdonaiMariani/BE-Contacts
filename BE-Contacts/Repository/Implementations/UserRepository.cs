using AutoMapper;
using BE_Contacts.Models.DTO;
using BE_Contacts.Models.Entities;
using BE_Contacts.Models;
using BE_Contacts.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_Contacts.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDTO>(user);
        }


        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUserAsync(UserDTO userDto)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id);
            if (existingUser != null)
            {
                existingUser.Name = userDto.Name;
                existingUser.LastName = userDto.LastName;
                existingUser.UserName = userDto.UserName;
                existingUser.Email = userDto.Email;
                existingUser.Password = userDto.Password;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
