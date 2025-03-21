using Microsoft.EntityFrameworkCore;
using Techa.DocumentGenerator.Infrastructure.Data;
using Techa.DocumentGenerator.Application.Utilities;
using Techa.DocumentGenerator.Domain.Entities.AAA;

namespace Techa.DocumentGenerator.API.Utilities;

public class DataInitializer
{
    internal static void Initialize(ApplicationDbContext context)
    {
        context.Database.Migrate();
        InitData(context);
    }

    private static void InitData(ApplicationDbContext context)
    {
        if (!context.Set<User>().Any())
        {
            var adminRole = new Role { Title = "Admin" };
            var userRole = new Role { Title = "ProjectUser" };

            context.Set<Role>().AddRange(new List<Role>()
            {
                adminRole, 
                userRole
            });

            context.Set<User>().Add(new User()
            {
                FirstName = "Farzam",
                LastName = "Yamini",
                UserName = "admin",
                PasswordHash = "Aa@12345".CleanString().GetSha256Hash(),
                SecurityStamp = Guid.NewGuid(),
                UserRoles = new List<UserRole>() 
                { 
                    new UserRole() { Role = adminRole }, 
                    new UserRole() { Role = userRole } 
                }
            });

            context.SaveChanges();
        }
    }
}