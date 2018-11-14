using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Core.ViewModels.Membership
{
    public class UserRefreshTokenVM
    {
        [StringLength(254)]
        [Required(ErrorMessage = "Required")]
        public string Token { get; set; }
    }
}
