﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Domain.Aggregates.User
{
    public class IdentityUserLogin : Microsoft.AspNetCore.Identity.IdentityUserLogin<long>
    {
    }
}
