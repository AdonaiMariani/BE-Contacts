using AutoMapper;
using BE_Contacts.Models.DTO;
using BE_Contacts.Models.Entities;

namespace BE_Contacts.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
