using BE_Contacts.Models;
using BE_Contacts.Models.DTO;

namespace BE_Contacts.Repository.Interfaces
{
    public interface IContactRepository
    {
        Task<List<ContactDTO>> GetAllContactsAsync(int userId);
        Task<ContactDTO> GetContactByIdAsync(int id, int userId);
        Task<ContactDTO> AddContactAsync(ContactDTO contactDto, int userId);
        Task UpdateContactAsync(ContactDTO contactDto, int userId);
        Task ToggleFavoriteAsync(int id, int userId, bool isFavorite);
        Task DeleteContactAsync(int id, int userId);
    }
}


//using BE_Contacts.Models;
//using BE_Contacts.Models.DTO;

//namespace BE_Contacts.Repository.Interfaces
//{
//    public interface IContactRepository
//    {
//        Task<List<ContactDTO>> GetAllContacts();
//        Task<ContactDTO> GetContactByIdAsync(int id);
//        Task<ContactDTO> AddContactAsync(ContactDTO contactDto);
//        Task UpdateContactAsync(ContactDTO contactDto);
//        Task ToggleFavoriteAsync(int id, bool isFavorite);
//        Task DeleteContactAsync(int id);

//    }
//}
