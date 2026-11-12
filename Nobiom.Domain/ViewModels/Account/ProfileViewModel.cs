using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nobiom.Domain.ViewModels.Account
{
    public class ProfileViewModel
    {
        [DisplayName("نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; } = string.Empty;

        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; } = string.Empty;

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UserName { get; set; } = string.Empty;

        [DisplayName("کد ملی")]
        [MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? NationalCode { get; set; }

        [Display(Name = "سال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(1300, 1500, ErrorMessage = "بین ۱۳۰۰ تا ۱۵۰۰ باشد")]
        public int? Year { get; set; }

        [Display(Name = "ماه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(1, 12, ErrorMessage = "بین ۱ تا ۱۲ باشد")]
        public int? Month { get; set; }

        [Display(Name = "روز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(1, 31, ErrorMessage = "بین ۱ تا ۳۱ باشد")]
        public int? Day { get; set; }

        [DisplayName("تصویر پروفایل")]
        public string? Avatar { get; set; }

        [DisplayName("شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [RegularExpression(@"^09\d{9}$",ErrorMessage = "شماره موبایل باید ۱۱ رقم باشد (مثلاً ۰۹۱۲۳۴۵۶۷۸۹)")]
        public string Mobile { get; set; } = string.Empty;

        [DisplayName("ایمیل")]
        [EmailAddress(ErrorMessage = "لطفا یک ایمیل معتبر وارد کنید")]
        public string? Email { get; set; }
    }
}
