
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WizardRecords.Api.Domain.Entities;

namespace WizardRecords.Api.Data.Entities
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; } // créé un token ?pour les non connecter a voir 
        //S'assurer de géré la bd pour savoir si l'article est disponible ou pas et la quantity qu'on peux prendre
        public float TotalAvTaxes { get; set; }
        public float Taxes { get; set; }
        public float TotalApTaxes { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        internal Cart() { }

        public Cart(Guid cartId, Guid user)
        {
            CartId = cartId;
            UserId = user;
        }
    }

    
    public class CartItem
    {
        [Key]
        public Guid AlbumId { get; set; }
        [ForeignKey("CartId")]
        public Guid? CartId { get; set; }
        [ForeignKey("OrderId")]
        public Guid? OrderId { get; set; }
        public Album? Album { get; set; }
        public int Quantity { get; set; }
    }
}

