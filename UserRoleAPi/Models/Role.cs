namespace UserRoleAPi.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<RoleUser> RoleUsers { get; set; } = new List<RoleUser>();

    }
}
