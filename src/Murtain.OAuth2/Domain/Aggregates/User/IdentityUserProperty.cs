using Murtain.Extensions.Domain.Entities;
using Murtain.OAuth2.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Domain.Aggregates.User
{
    [Table("identity_user_property")]
    public class IdentityUserProperty : Entity
    {
        [MaxLength(50)]
        public virtual string NickName { get; set; }
        [MaxLength(50)]
        public string Birthday { get; set; }
        public int Age { get; set; }
        [MaxLength(50)]
        public virtual Gender Gender { get; set; }
        [MaxLength(2000)]
        public virtual string Avatar { get; set; }
    }
}
