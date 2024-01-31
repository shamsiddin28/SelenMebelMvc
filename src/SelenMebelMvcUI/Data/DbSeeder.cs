using Microsoft.AspNetCore.Identity;
using SelenMebelMvcUI.Constants;

namespace SelenMebelMvcUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            // adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // create admin

            var admin = new IdentityUser
            {
                UserName = "shamsiddinumarov0013@gmail.com",
                Email = "shamsiddinumarov0013@gmail.com",
                EmailConfirmed = true,
            };

            var isUserExists = await userMgr.FindByEmailAsync(admin.Email);
            if (isUserExists is null)
            {
                await userMgr.CreateAsync(admin, "EUFd5MiR+jaP%,v");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());

            }
        }
    }
}
