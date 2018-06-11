using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Murtain.Extensions.Domain;
using Murtain.OAuth2.Domain.Aggregates.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Murtain.OAuth2.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, long, IdentityUserClaim, IdentityUserRole, IdentityUserLogin, IdentityRoleClaim, IdentityUserToken>, IUnitOfWork
    {
        private readonly IMediator mediator;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        //{
        //    this.mediator = mediator;
        //}

        public virtual DbSet<IdentityUserProperty> IdentityUserProperty { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("identity_user");
            builder.Entity<IdentityUserLogin>().ToTable("identity_user_login");
            builder.Entity<IdentityUserClaim>().ToTable("identity_user_claim");
            builder.Entity<IdentityUserRole>().ToTable("identity_user_role");
            builder.Entity<IdentityUserToken>().ToTable("identity_user_token");
            builder.Entity<IdentityRole>().ToTable("identity_role");
            builder.Entity<IdentityRoleClaim>().ToTable("identity_roleclaim");

        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            //await mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed throught the DbContext will be commited
            //var result = await base.SaveChangesAsync();

            return true;
        }
    }
}
