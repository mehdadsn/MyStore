using Store.Domain.Entities.Commons;

namespace Store.Domain.Entities.Users
{
    public class UserRole : BaseEntity
    { 
        public int Id { get;set; } 
        public User User { get; set; }
        public long UserId { get; set; }
        public Role Role { get; set; }
        public long RoleId { get; set; }
    }
}
