using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static WizardRecords.Api.Data.Constants;

namespace WizardRecords.Api.Domain.Entities
{
    public class User : IdentityUser<Guid> {
        public User(string userName) : base(userName) { }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public int AddressNum { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public Province Province { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public string Country { get; set; } = "Canada";
        [Required]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
    }
}
