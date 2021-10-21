using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models
{
    // View'a göre model oluşturduk...
    public class AccountUserVM
    {
        [Required]
        [Display(Name = "Adı SoyAd")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Email Adresiniz")]
        [EmailAddress(ErrorMessage = "Geçersiz Email Formatı...")] // Email formatında olmalıdır..
        public string Email { get; set; }

        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Şifre Tekrarınız")]
        [Compare("Password", ErrorMessage = "Şifreler Aynı Değil")] // Compare bir validationdır. Bu property'nin değerini belirttiğiniz diğer property değeri ile karşılaştırır..
        public string Password2 { get; set; }
    }
}
