using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopMng.Model
{
    [Table("UserInfo")]
    public class UsersInformation
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PhoneNo { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
