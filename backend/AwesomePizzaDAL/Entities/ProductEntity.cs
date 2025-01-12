using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUnitOfWork.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomePizzaDAL.Entities
{
    [Table("Product", Schema = "Master")]
    public class ProductEntity : BaseAuditEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string CodeValue { get; set; }

        [Required]
        public TypeOfProduct Type { get; set;}
        public string Category { get; set; }

        public string? ImageUrl { get; set; }

        [Precision(18,5)]
        public decimal? Price { get; set; }

        public string? Description { get; set; }

        // Navigation property for related OrderProducts
        public ICollection<OrderProductEntity> OrderProducts { get; set; }
    }

    public enum TypeOfProduct
    {
        Pizza,
    }
}
