namespace WizardRecords.Dtos {
    public record UserDto(
        Guid UserId,
        string FirstName,
        string LastName
    ) {
        public string FullName => $"{FirstName} {LastName}";
    };
}
