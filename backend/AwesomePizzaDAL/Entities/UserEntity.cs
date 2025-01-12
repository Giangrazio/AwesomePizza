using System.ComponentModel.DataAnnotations;
using GenericUnitOfWork.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomePizzaDAL.Entities
{
    [Table("User", Schema = "Master")]
    public class UserEntity : BaseEntity
    {
        [Required]
        public string CodeValue { get; set; }
        public string? Denomination { get; set; }
        
        public string? RoleName { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
