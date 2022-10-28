using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace EntityLayer
{
    [Table("Users")]
    public class UserEntity
    {
        public UserEntity() { }

        public UserEntity(string fn, string ln, string add, string em, string ph, string user, string pw) {
            Username = user; 
            Password = pw;
            Firstname = fn;
            Lastname = ln;
            Address = add;
            Email = em;
            Phone = ph;
        }


        [Key]
        public string Username { get; set; }
        public string Password { get; set; }


        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int CompareTo(UserEntity other)
        {
            var compare1 = Username ?? string.Empty;
            var compare2 = other.Username ?? string.Empty;
            return compare1.CompareTo(compare2);
        }
    }
}
