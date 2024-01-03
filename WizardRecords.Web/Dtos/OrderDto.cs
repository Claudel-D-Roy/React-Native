namespace WizardRecords.Dtos {
    public class OrderDto {
        public Guid userId { get; set; }
        public float TotalAvTaxes { get; set; }
        public float Taxes { get; set; }
        public float TotalApTaxes { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String address { get; set; }
        public String city { get; set; }
        public String country { get; set; }
        public String province { get; set; }
        public String zipCode { get; set; }
    }
}
