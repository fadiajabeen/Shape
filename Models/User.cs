using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Shape.Models
{
    public partial class User
    {
        public User()
        {
            UserLogins = new HashSet<UserLoginHistory>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "First Name")]
        [RegularExpression(@"^\w+( +\w+)*$", ErrorMessage ="Please enter a valid name.")]
        public string Firstname { get; set; } = null!;

        [Required]
        [StringLength(16)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^\w+( +\w+)*$", ErrorMessage = "Please enter a valid name.")]
        public string Lastname { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Work Email Address")]
        [StringLength(320)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 256, MinimumLength = 8)]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "Password must contain a digit, 1 uppercase letter, 1 lowercaser letter and a special character.")] //(?=^.{8,15}$)
        public string Password { get; set; } = null!;
        
        public bool PasswordUpdateRequired { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<UserLoginHistory> UserLogins { get; set; }

        /// <summary>
        /// Convert the plain Password to its hash in same property i.e., Password
        /// Call this before saving user in database
        /// </summary>
        public User SetPasswordHash()
        {
            Password = new PasswordHasher<User>().HashPassword(this, Password);
            return this;
        }
        
        public User ApplyTextualFormatting()
        {
            TextInfo textInfo =  new CultureInfo("en-us", false).TextInfo;
            Firstname = textInfo.ToTitleCase(Firstname.ToLower());
            Lastname = textInfo.ToTitleCase(Lastname.ToLower());
            Email = Email.ToLower();
            return this;
        }

        /// <summary>
        /// Verify the user input with password hash in the database
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        public bool VerifyPassword(string plainPassword)
        {
            PasswordVerificationResult verificationResult = new PasswordHasher<User>().VerifyHashedPassword(this, this.Password, plainPassword);
            switch (verificationResult)
            {
                case PasswordVerificationResult.Failed:
                    return true;

                case PasswordVerificationResult.Success:
                    return false;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    this.PasswordUpdateRequired = true;
                    return true;

                default:
                    return false;
            }
        }

    }

    [NotMapped]
    public class UserRegistration: User
    {
        public UserRegistration(): base()
        {
            CreatedDate = UpdatedDate = DateTime.UtcNow;
            PasswordUpdateRequired = false;
        }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm Password should match the Password.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
