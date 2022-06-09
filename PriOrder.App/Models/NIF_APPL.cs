using System;
using System.ComponentModel.DataAnnotations;

namespace PriOrder.App.Models
{
    public class NIF_APPL
    {
        [Display(Name = "Your NID")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 20, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 5)]
        public string APPL_NID { get; set; }

        public string NID_IMG { get; set; }
        public string APPL_IMG { get; set; }

        [Display(Name = "Full Name(According to NID)")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string FULL_NAME { get; set; }

        [Display(Name = "Date of Birth(According to NID)")]
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Date)]
        [MiniAge(18,100)]
        public DateTime BIRTH_DATE { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 11, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        public string MOBILE_NUMBER { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        public string EMAIL_ADDRESS { get; set; }

        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string FATHER_NAME { get; set; }

        [Display(Name = "Mother Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string MOTHER_NAME { get; set; }
        
        [Display(Name = "Parents Mobile Number")]
        [StringLength(maximumLength: 11, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        public string PARENTS_MOBILE { get; set; }

        [Display(Name = "Spouse Name")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string SPOUSE_NAME { get; set; }

        [Display(Name = "Spouse Mobile Number")]
        [StringLength(maximumLength: 11, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 11)]
        public string SPOUSE_MOBILE { get; set; }




        [Display(Name = "House/Road")]
        [StringLength(maximumLength: 30, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 0)]
        public string HOUSE_ROAD { get; set; }

        [Display(Name = "Village Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 30, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string VILLAGE_NAME { get; set; }
        
        [Display(Name = "Union Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string UNION_NAME { get; set; }

        [Display(Name = "PS/Thana")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string POLICE_STATION { get; set; }

        [Display(Name = "District")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string DISTRICT { get; set; }


        
        [Display(Name = "House/Road")]
        [StringLength(maximumLength: 30, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 0)]
        public string HOUSE_ROAD2 { get; set; }

        [Display(Name = "Village Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 30, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string VILLAGE_NAME2 { get; set; }

        [Display(Name = "Union Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string UNION_NAME2 { get; set; }

        [Display(Name = "PS/Thana")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string POLICE_STATION2 { get; set; }

        [Display(Name = "District")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 25, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string DISTRICT2 { get; set; }

        [Display(Name = "As Present")]
        public int AS_PRESENT { get; set; }




        [Display(Name = "Account Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string ACCOUNT_NAME { get; set; }

        [Display(Name = "Account Number")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 30, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 5)]
        public string ACCOUNT_NO { get; set; }

        [Display(Name = "Bank Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string BANK_NAME { get; set; }

        [Display(Name = "Branch Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string BRANCH_NAME { get; set; }

        [Display(Name = "TIN Number")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string DIST_TIN { get; set; }

        [Display(Name = "BIN Number")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string DIST_BIN { get; set; }

        [Display(Name = "Trade Number")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0} length is between {2} and {1}", MinimumLength = 3)]
        public string DIST_TRADE { get; set; }
        
    }
}