using System.ComponentModel.DataAnnotations.Schema;

namespace Techa.DocumentGenerator.Domain.Entities.AAA
{
    //[Table(nameof(UserRole))]
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}
