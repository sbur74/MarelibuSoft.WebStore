using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie den angezeigten Captcha Code ein!")]
        [StringLength(4)]
        public string CaptchaCode { get; set; }

        public string ErrorString { get; set; }
    }
}
