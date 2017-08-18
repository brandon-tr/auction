using System.ComponentModel.DataAnnotations;
namespace auction.Models
{
    public class UserReg
    {
        [Key]
        public int userId{get; set;}

        [Key]
        public double money{get; set;}

        [Required(ErrorMessage = "Must have a first name")]
        [MinLength(2, ErrorMessage = "Minimum Length is 2 for your first name")]
        public string firstName {get; set;}


        [Required(ErrorMessage = "Must have a last name")]
        [MinLength(2, ErrorMessage = "Minimum Length is 2 for your last name")]
        public string lastName {get; set;}


        [Required(ErrorMessage = "Must have a email")]
        [MinLength(5, ErrorMessage = "Minimum Length is 5 for your email")]
        public string email {get; set;}


        [Required(ErrorMessage = "Must have a password")]
        [MinLength(8, ErrorMessage = "Minimum Length is 8 for your password")]
        public string password {get; set;}

        [Required(ErrorMessage = "passwords must be the same")]
        [Compare("password", ErrorMessage = "passwords must be the same")]
        public string confPassword {get; set;}
    }
}