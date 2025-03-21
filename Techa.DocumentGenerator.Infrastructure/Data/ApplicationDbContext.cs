using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Techa.DocumentGenerator.Domain.Entities;
using Techa.DocumentGenerator.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Techa.DocumentGenerator.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor = null) : base(options)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(BaseEntity).Assembly;

            modelBuilder.RegisterAllEntities<BaseEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddCurrentDateTimeForCreatedDateConvention();
            modelBuilder.AddPluralizingTableNameConvention();
        }


        public override int SaveChanges()
        {
            _cleanString();
            _setExtraInfos();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            _setExtraInfos();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            _setExtraInfos();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            _setExtraInfos();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (!string.IsNullOrEmpty(val))
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
        private void _setExtraInfos()
        {
            string? authenticatedUserId = null;

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
                if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
                    authenticatedUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && (p.Name is "ModifiedDate" or "ModifiedByUserId" or "CreatedByUserId"));

                foreach (var property in properties)
                {
                    var propName = property.Name;

                    if (item.State == EntityState.Modified)
                    {
                        if (propName == "ModifiedDate")
                        {
                            property.SetValue(item.Entity, DateTime.Now, null);
                        }

                        if (propName == "ModifiedByUserId")
                        {
                            if (!string.IsNullOrEmpty(authenticatedUserId) && int.TryParse(authenticatedUserId, out int userId))
                            {
                                property.SetValue(item.Entity, userId, null);
                            }
                        }
                    }
                    
                    if (item.State == EntityState.Added)
                    {
                        if (propName == "CreatedByUserId")
                        {
                            if (!string.IsNullOrEmpty(authenticatedUserId) && int.TryParse(authenticatedUserId, out int userId))
                            {
                                property.SetValue(item.Entity, userId, null);
                            }
                        }
                    }
                }
            }
        }
    }
}
