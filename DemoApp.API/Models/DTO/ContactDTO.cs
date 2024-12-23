using System.ComponentModel.DataAnnotations;
 namespace DemoApp.API.Models.DTO
{
    public class ContactDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "The name must contain only alphabetic characters.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "The name must contain only alphabetic characters.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^[0-9]{10,15}$",
        ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone Number cannot exceed 15 characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [RegularExpression(@"^[a-zA-Z0-9,/\s]+$",
        ErrorMessage = "Address should only contain letters, numbers, commas, slashes, and spaces.")]
     
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [StringLength(10, ErrorMessage = "Postal Code cannot exceed 10 characters")]
        public string PostalCode { get; set; }
    }
}
