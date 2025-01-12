using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericUnitOfWork.Entities
{
    public class BaseAuditEntity: BaseEntity
    {
        public DateTime CreatedOnDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [StringLength(255)]
        public string? CreatedByUserId { get; set; }
        [StringLength(255)]
        public string? MembershipName { get; set; }
        [StringLength(255)]
        public string? MembershipEmail { get; set; }
        [StringLength(255)]
        public string? CreatedByIpAddress { get; set; }
        //public long UserId { get; set; }
        //public string UserName { get; set; }

        public string? LastTokenId { get; set; }
    }
}
