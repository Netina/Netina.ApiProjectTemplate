using Api.WebFramework.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.UseSerilog();
LoggerConfig.ConfigureSerilog();

// Add services to the container.

var configuration = builder.Configuration;
var siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
builder.Services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));

builder.Services.AddControllers();
builder.Services.AddRazorPages(configure => configure.RootDirectory = "/Pages").AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddCustomDbContext(configuration);
builder.Services.AddCustomController();
builder.Services.AddCustomMvc();
builder.Services.AddCustomSwagger(siteSetting.BaseUrl, siteSetting.SwaggerSetting);
builder.Services.AddCustomCores();
builder.Services.AddJwtCustomAuthentication(siteSetting.JwtSettings);
builder.Services.AddCustomIdentity();
builder.Services.AddCustomApiVersioning();

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{

    var assembly = typeof(CoreConfig).Assembly;
    builder
        .RegisterAssemblyTypes(assembly)
        .AssignableTo<IScopedDependency>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

    var assemblyB = typeof(InfrastructureConfig).Assembly;
    builder.RegisterAssemblyTypes(assemblyB)
        .AssignableTo<IScopedDependency>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

    var assemblyC = typeof(ReposConfig).Assembly;
    builder.RegisterAssemblyTypes(assemblyC)
        .AssignableTo<IScopedDependency>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();


    var assemblyD = typeof(Program).Assembly;
    builder.RegisterAssemblyTypes(assemblyD)
        .AssignableTo<IScopedDependency>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger(siteSetting.BaseUrl, builder.Environment);
}

app.UseExceptionHandlerMiddleware();

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

//await app.ReposInit();

app.MapControllers();
app.MapRazorPages();
app.UseCors("CorsPolicy");

app.Run();
