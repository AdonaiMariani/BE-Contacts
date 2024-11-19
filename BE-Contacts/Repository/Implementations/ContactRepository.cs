using AutoMapper;
using BE_Contacts.Models;
using BE_Contacts.Models.DTO;
using BE_Contacts.Models.Entities;
using BE_Contacts.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_Contacts.Repository.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly AplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContactRepository(AplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ContactDTO>> GetAllContactsAsync(int userId)
        {
            return await _context.Contacts
                          .Where(contact => contact.UserId == userId)
                          .Select(contact => _mapper.Map<ContactDTO>(contact))
                          .ToListAsync();
        }

        public async Task<ContactDTO> GetContactByIdAsync(int id, int userId)
        {
            var contact = await _context.Contacts
                                        .FirstOrDefaultAsync(contact => contact.Id == id && contact.UserId == userId);

            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found or does not belong to the user.");
            }

            return _mapper.Map<ContactDTO>(contact);
        }

        public async Task<ContactDTO> AddContactAsync(ContactDTO contactDto, int userId)
        {
            var contact = _mapper.Map<Contact>(contactDto);
            contact.UserId = userId; // Asociar el contacto al usuario

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return _mapper.Map<ContactDTO>(contact);
        }

        public async Task UpdateContactAsync(ContactDTO contactDto, int userId)
        {
            var existingContact = await _context.Contacts
                                                .FirstOrDefaultAsync(contact => contact.Id == contactDto.Id && contact.UserId == userId);

            if (existingContact != null)
            {
                existingContact.Name = contactDto.Name;
                existingContact.Phone = contactDto.Phone;
                existingContact.Email = contactDto.Email;
                existingContact.Address = contactDto.Address;
                existingContact.IsFavorite = contactDto.IsFavorite;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Contact with ID {contactDto.Id} not found or does not belong to the user.");
            }
        }

        public async Task ToggleFavoriteAsync(int id, int userId, bool isFavorite)
        {
            var contact = await _context.Contacts
                                        .FirstOrDefaultAsync(contact => contact.Id == id && contact.UserId == userId);
            if (contact != null)
            {
                contact.IsFavorite = isFavorite;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found or does not belong to the user.");
            }
        }

        public async Task DeleteContactAsync(int id, int userId)
        {
            var contact = await _context.Contacts
                                        .FirstOrDefaultAsync(contact => contact.Id == id && contact.UserId == userId);
            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found or does not belong to the user.");
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }
    }
}


//using AutoMapper;
//using BE_Contacts.Models;
//using BE_Contacts.Models.DTO;
//using BE_Contacts.Models.Entities;
//using BE_Contacts.Repository.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace BE_Contacts.Repository.Implementations
//{
//    public class ContactRepository: IContactRepository
//    {
//        private readonly AplicationDbContext _context;
//        private readonly IMapper _mapper;

//        public ContactRepository(AplicationDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;

//        }



//        public async Task<List<ContactDTO>> GetAllContacts()
//        {
//            return await _context.Contacts
//                          .Select(contact => _mapper.Map<ContactDTO>(contact))
//                          .ToListAsync();
//        }

//        public async Task<ContactDTO> GetContactByIdAsync(int id)
//        {
//           var contact = await _context.Contacts.FindAsync(id);

//            if (contact == null)
//            {
//                throw new KeyNotFoundException($"Contact with ID {id} not found.");
//            }

//            return _mapper.Map<ContactDTO>(contact);
//        }

//        public async Task<ContactDTO> AddContactAsync(ContactDTO contactDto)
//        {
//            var contact = _mapper.Map<Contact>(contactDto);
//            await _context.Contacts.AddAsync(contact);
//            await _context.SaveChangesAsync();
//            return _mapper.Map<ContactDTO>(contact);
//        }


//        public async Task UpdateContactAsync(ContactDTO contactDto)
//        {
//            var existingContact = await _context.Contacts.FirstOrDefaultAsync(contact => contact.Id == contactDto.Id);

//            if (existingContact != null)
//            {
//                existingContact.Name = contactDto.Name;
//                existingContact.Phone = contactDto.Phone;
//                existingContact.Email = contactDto.Email;
//                existingContact.Address = contactDto.Address;
//                existingContact.IsFavorite = contactDto.IsFavorite;

//                await _context.SaveChangesAsync();
//            }


//        }

//        public async Task ToggleFavoriteAsync(int id, bool isFavorite)
//        {
//            var contact = await _context.Contacts.FirstOrDefaultAsync(contact => contact.Id == id);
//            if (contact != null)
//            {
//                contact.IsFavorite = isFavorite;
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task DeleteContactAsync(int id)
//        {
//            var contact = await _context.Contacts.FindAsync(id);
//            if (contact == null) return;

//            _context.Contacts.Remove(contact);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
