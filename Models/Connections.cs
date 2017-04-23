using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class Connections : BaseEntity
    {
        [Key]
        public int ConnectionId { get; set; }
        
        public int status { get; set; }
 
        public int FollowerId { get; set; }
        public Users Follower { get; set; }
 
        public int UserFollowedId { get; set; }
        public Users UserFollowed { get; set; }

    }
    
}