using AwesomePizzaDAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePizzaBLL.Models
{
    public class ProductModel : BaseAuditModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string CodeValue { get; set; }

        [Required]
        public TypeOfProduct Type { get; set; }
        public string Category { get; set; }

        public string? ImageUrl { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }
    }
}
