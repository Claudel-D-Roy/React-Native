using static WizardRecords.Api.Data.Constants;

namespace WizardRecords.Dtos {
    public record RegisterDto(
        string UserName,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber,
        int AddressNum,
        string StreetName,
        string City,
        string PostalCode,
        Province Province
    );
}
