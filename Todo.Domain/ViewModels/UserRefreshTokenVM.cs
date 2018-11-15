using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Domain.ViewModels
{
    public class UserRefreshTokenVM
    {
        [StringLength(254)]
        [Required(ErrorMessage = "Required")]
        public string Token { get; set; }
    }
}
