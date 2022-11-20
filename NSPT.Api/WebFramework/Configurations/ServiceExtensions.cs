namespace Server.WebFramework.Configurations
{
    public static class ServiceExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection serviceCollection, IConfigurationRoot Configuration)
        {
            serviceCollection.AddDbContext<ApplicationContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseNpgsql(Configuration.GetConnectionString("Postgres"), b => b.MigrationsAssembly("Repos"))
                    .UseProjectAssembly(typeof(ApplicationUser).Assembly);
                options.EnableServiceProviderCaching(false);
            }, ServiceLifetime.Scoped);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public static void AddCustomCores(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod()
                        .SetPreflightMaxAge(TimeSpan.FromHours(24))
                        .WithExposedHeaders("Access-control-allow-origins")
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();

                }));
        }
        public static void AddCustomController(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers(options => { options.Filters.Add(new AuthorizeFilter()); })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
                );
        }

        public static void AddCustomMvc(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }
        public static void AddJwtCustomAuthentication(this IServiceCollection serviceCollection, JwtSettings jwtSettings)
        {
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                    
                })
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, options =>
            {

            })
            .AddJwtBearer(options =>
                {
                    var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                    var validateParammetrs = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        RequireSignedTokens = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer

                    };
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = validateParammetrs;
                    options.IncludeErrorDetails = true;

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                                context.Token = accessToken.ToString();
                            var videoStorageOrigin = context.Request.Headers["X-Original-URI"].ToString();
                            var videoToken = videoStorageOrigin.Split("?access_token=").Last();
                            if (!string.IsNullOrEmpty(videoToken))
                                context.Token = videoToken;
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }
                    };
                });

        }

        public static void AddCustomIdentity(this IServiceCollection serviceCollection)
        {
            

            serviceCollection.AddIdentityCore<ApplicationUser>(options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = false;
                })
                .AddRoles<ApplicationRole>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();

        }

        public static void AddCustomApiVersioning(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
        }
    }
}
