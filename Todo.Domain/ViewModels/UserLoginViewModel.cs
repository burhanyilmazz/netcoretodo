using System.ComponentModel.DataAnnotations;

namespace Todo.Domain.ViewModels
{
    public class UserLoginViewModel
    {
        [StringLength(254)]
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}
