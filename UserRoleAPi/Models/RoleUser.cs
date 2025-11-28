using System.ComponentModel.DataAnnotations;

namespace UserRoleAPi.Models
{
    public class RoleUser
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }


   
    }
}
