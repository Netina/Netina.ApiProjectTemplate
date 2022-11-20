namespace Repos.Services;


public class DbInitializerService : IDbInitializerService
{

    private readonly ApplicationContext _context;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptionsSnapshot<SiteSettings> _adminUserSeedOptions;
    private readonly ILogger<DbInitializerService> _logger;

    public DbInitializerService(
        ApplicationContext context,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IOptionsSnapshot<SiteSettings> adminUserSeedOptions,
        ILogger<DbInitializerService> logger)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _adminUserSeedOptions = adminUserSeedOptions;
        _logger = logger;
    }
    public void Initialize()
    {
        try
        {
            _context.Database.Migrate();
            _logger.LogInformation("Migration SUCCESS !!!!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public async Task SeedDate(bool force = false)
    {
        try
        {

            await SeedRoles();

            var seedAdmin = _adminUserSeedOptions.Value.UserSetting;

            var user = await _userManager.FindByNameAsync(seedAdmin.Username);
            if (user == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = seedAdmin.Username,
                    Email = seedAdmin.Email,
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    FirstName = seedAdmin.FirstName,
                    LastName = seedAdmin.LastName,
                    Gender = Gender.Mail,
                    PhoneNumberConfirmed = true,
                    PhoneNumber = seedAdmin.Phone,
                    BirthDate = DateTime.Now.AddYears(-23)
                };
                var adminUserResult = await _userManager.CreateAsync(adminUser, seedAdmin.Password);
                if (adminUserResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, seedAdmin.RoleName);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SeedRoles()
    {
        var seedAdmin = _adminUserSeedOptions.Value.UserSetting;
        var managerRole = await _roleManager.FindByNameAsync(seedAdmin.RoleName);
        if (managerRole == null)
        {
            managerRole = new ApplicationRole()
            {
                Name = seedAdmin.RoleName,
                Description = "root admin role"
            };

            await _roleManager.CreateAsync(managerRole);
        }

    }
}
