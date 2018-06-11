using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Murtain.Extensions.AutoMapper;
using Murtain.Extensions.Domain;
using Murtain.OAuth2.Domain.Aggregates.User;
using Murtain.OAuth2.Domain.Enumerations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Murtain.OAuth2.Domain.Aggregates.User
{
    public class ApplicationUser : IdentityUser<long>, IAggregateRoot
    {

        public virtual Address Address { get; set; }
        public virtual IdentityUserProperty UserProperty { get; set; }

    }
}
