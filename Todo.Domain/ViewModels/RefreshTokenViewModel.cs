using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Domain.ViewModels
{
    public class RefreshTokenViewModel
    {
        [StringLength(254)]
        [Required(ErrorMessage = "Required")]
        public string Token { get; set; }
    }
}
