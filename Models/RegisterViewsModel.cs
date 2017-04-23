using System.ComponentModel.DataAnnotations;

namespace Exam1.Models
{
    public class RegisterViewModel : BaseEntity
    {

        [Required(ErrorMessage="First Name is Required")]
        [RegularExpression(@"^[a-zA-Z]+\s[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters an only one space")]
        [MinLength(3, ErrorMessage="First Name must be 3 characters long")]
        public string userName { get; set; }
               
        [Required(ErrorMessage="Email is Required")]
        [RegularExpression(@"[\w+\-.]+@[a-z\d\-]+(\.[a-z\d\-]+)*\.[a-z]+", ErrorMessage = "Incorrect Email format")]
        public string email { get; set; }
        
        [Required(ErrorMessage="Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters long")]
        public string password { get; set; }

        // [Required(ErrorMessage="Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage="Password and confirmation must match.")]
        public string confirm_password { get; set; }

        [Required(ErrorMessage="Description is Required")]
        [MinLength(10, ErrorMessage="Description must be 10 characters long")]
        public string description { get; set; }

    }
}