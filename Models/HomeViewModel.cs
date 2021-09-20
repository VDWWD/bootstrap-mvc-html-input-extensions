using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace DemoWebsite.Models
{
    public class HomeViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Your name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Your name is too short.")]
        public string Name { get; set; }


        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Your email address is required.")]
        [EmailAddress(ErrorMessage = "The email address is incorrect.")]
        [StringLength(100)]
        public string Email { get; set; }


        [Display(Name = "Birthdate")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "Incorrect date.")]
        public string Birthdate { get; set; }


        [Display(Name = "Telephone number")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Incorrect phone number.")]
        public string Telephone { get; set; }


        [Display(Name = "Password")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Your password is too short.")]
        public string Password { get; set; }


        [Display(Name = "Gender")]
        public GenderEnum Gender { get; set; }


        [Display(Name = "Rating")]
        [Required(ErrorMessage = "Please give me a rating.")]
        [Range(9, 10, ErrorMessage = "Your {0} must be between {1} and {2}.")]
        public int Rating { get; set; }


        [Display(Name = "Select a file...")]
        [FileType("bmp,gif,jpg,jpeg,png")]
        public HttpPostedFileBase Image { get; set; }


        [Display(Name = "Send me a message")]
        [Required(ErrorMessage = "A message is required.")]
        [AllowHtml]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "The message is too short.")]
        public string Message { get; set; }


        [Display(Name = "Want to enable this Switch?")]
        public bool Switch { get; set; }


        [Display(Name = "Disabled Switch without label")]
        public bool SwitchNoLabel { get; set; }


        [Display(Name = "Do you agree with the Terms and Conditions?")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the Terms and Conditions.")]
        public bool TermsAndConditions { get; set; }


        public bool Result { get; set; }
        public string ResultMessage { get; set; }
        public string ImageBase64 { get; set; }


        public enum GenderEnum
        {
            Unknown,
            Male,
            Female,
            Its_Complicated,
            Apache_Helicopter
        }
    }
}