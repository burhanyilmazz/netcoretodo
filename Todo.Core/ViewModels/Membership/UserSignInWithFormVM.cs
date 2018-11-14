using System.ComponentModel.DataAnnotations;

namespace Todo.Core.ViewModels.Membership
{
    public class UserSignInWithFormVM
    {
        [StringLength(254)]
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}
