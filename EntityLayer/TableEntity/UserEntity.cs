using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace EntityLayer
{
    [Table("Users")]
    public class UserEntity
    {
        public UserEntity() { }

        public UserEntity(string user, string pw) { Username = user; Password = pw; }

        [Key]
        public string Username { get; set; }

        public string Password { get; set; }

        public int CompareTo(UserEntity other)
        {
            var compare1 = Username ?? string.Empty;
            var compare2 = other.Username ?? string.Empty;
            return compare1.CompareTo(compare2);
        }
    }
}
