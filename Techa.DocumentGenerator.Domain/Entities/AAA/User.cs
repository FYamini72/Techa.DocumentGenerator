﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Techa.DocumentGenerator.Domain.Entities.AAA
{
    //[Table(nameof(User))]
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? NationalCode { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public AttachmentFile? Profile { get; set; }


        public Guid SecurityStamp { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}