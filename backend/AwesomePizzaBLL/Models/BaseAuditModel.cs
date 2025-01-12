using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePizzaBLL.Models
{
    public class BaseAuditModel
    {
        public long Id { get; set; }
        public DateTime CreatedOnDate { get; internal set; }
        public DateTime? ModifiedDate { get; internal set; }

        public string? CreatedByUserId { get; internal set; }
        public string? MembershipName { get; internal set; }
        public string? MembershipEmail { get; internal set; }
        public string? CreatedByIpAddress { get; internal set; }
        public string? LastTokenId { get; internal set; }
    }
}
