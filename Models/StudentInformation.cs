using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.Models
{
    public class StudentInformation
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string? MobileNo { get; set; }

        [Required(ErrorMessage = "Please select a language.")]
        public string? Lang { get; set; }

        [Required(ErrorMessage = "Please select a gender.")]
        public string? Gender { get; set; }

        
        public string? Languages { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please select a languages.")]
        public List<string> lstLanguages { get; set; } = new List<string>();

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        public int Id { get; set; }
     
        [RegularExpression(@"^.*\.(jpg|jpeg|png|gif|bmp)$", ErrorMessage = "Please provide a valid photo path (jpg, jpeg, png, gif, bmp).")]
        public string? PhotoPath { get; set; }

        [RegularExpression(@"^.*\.(pdf)$", ErrorMessage = "Please provide a valid document path (PDF only).")]
        public string? DocumentPath { get; set; }
        [Required(ErrorMessage = "Photo  is required.")]
        [NotMapped]
        public IFormFile? PhotoFile { get; set; }

        [Required(ErrorMessage = "Document  is required.")]
        [NotMapped]
        public IFormFile? DocumentFile { get; set; }
    }
}
