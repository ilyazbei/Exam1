using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam1.Models
{
    public class Users : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string description { get; set; }


        [InverseProperty("UserFollowed")]
        public List<Connections> Followers { get; set; }
 
        [InverseProperty("Follower")]
        public List<Connections> UsersFollowed { get; set; }
         

        public Users() : base() 
        {
           Followers = new List<Connections>();
           UsersFollowed = new List<Connections>();
        }
    }
}