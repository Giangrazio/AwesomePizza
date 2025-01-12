using System.ComponentModel.DataAnnotations;

namespace AwesomePizzaBLL.Models
{
    public class AccountRegisterModel
    {
        [Required]
        public string NickName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string CodeValue { get; set; }
        public string? Denomination { get; set; }
    }
}
