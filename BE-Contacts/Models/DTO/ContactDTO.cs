﻿using BE_Contacts.Models.Entities;

namespace BE_Contacts.Models.DTO
{
    public class ContactDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool IsFavorite { get; set; }

        public int UserId { get; set; }
    }
}