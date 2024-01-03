
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WizardRecords.Api.Domain.Entities;
using WizardRecords.Dtos;

namespace WizardRecords.Api.Data.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public OrderState State { get; set; }
        public float TotalAvTaxes { get; set; }
        public float Taxes { get; set; }
        public float TotalApTaxes { get; set; }
        public String? UserName { get; set; }
        public String? UserEmail { get; set; }
        public String? UserPhone { get; set; }
        public String? Adress { get; set; }
        public String? City { get; set; }
        public String? Country { get; set; } = "Canada";
        public String? Province { get; set; }
        public String? ZipCode { get; set; } 

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public Order() {}

         public Order(Cart cart) {
            CartItems.AddRange(cart.CartItems);
            TotalApTaxes = cart.TotalApTaxes;
            TotalAvTaxes = cart.TotalAvTaxes;
            Taxes = cart.Taxes;
            UserId = cart.UserId;
            State = OrderState.Confirmée;
         }

         public void UpdateFromDto(OrderDto dto){
            UserName = dto.firstName + ' ' + dto.lastName;
            UserEmail = dto.email;
            UserPhone = dto.phone;
            Adress = dto.address;
            City = dto.city;
            Country = dto.country;
            Province = dto.province;
            ZipCode =  dto.zipCode;
         }
    }

   public enum OrderState{
      Confirmée,
      Payée,
      Annulée,
      EnPrep,
      EnLivraison,
      Livrée,
      Retournée
   }
}

