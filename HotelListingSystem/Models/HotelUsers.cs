using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace HotelListingSystem.Models
{
    //System users
    public class HotelUsers
    {
        [Column(Order = 1)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 10)]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "The FirstName field should consist of characters only")]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Column(Order = 11)]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage =  "The LastName field should consist of characters only")]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Column(Order = 12)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Column(Order = 13)]
        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Column(Order = 14)]
        public string Designation { get; set; }

        [Column(Order = 15)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Column(Order = 16)]
        [Display(Name = "Hotel User Type")]
        public string HotelUserType { get; set; }

        [Column(Order = 17)]
        [Display(Name = "Status")]
        public string StatusId { get; set; }

        [Column(Order = 18)]
        public bool IsPasswordReset { get; set; }

        [Column(Order = 22)]
        [Display(Name = "ID/ Passport No")]
        [MaxLength(100)]
        public string IdentificationNumber { get; set; }
        [Column(Order = 20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        [Column(Order = 201)]
        [Display(Name = "Created On")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "User")]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}