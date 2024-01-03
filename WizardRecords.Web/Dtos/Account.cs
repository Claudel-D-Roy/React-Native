using System.ComponentModel.DataAnnotations;

namespace WizardRecords.Dtos
{
    public class Account
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
