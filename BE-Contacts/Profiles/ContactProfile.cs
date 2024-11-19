using AutoMapper;
using BE_Contacts.Models.DTO;
using BE_Contacts.Models.Entities;

namespace BE_Contacts.Profiles
{
    public class ContactProfile: Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDTO>();
            CreateMap<ContactDTO, Contact>();
        }
    }
}
