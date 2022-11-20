using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Pluralize.NET;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Api.WebFramework.Configurations;

public static class SwaggerConfiguration
{
    public static void AddCustomSwagger(this IServiceCollection services, string baseUrl , SwaggerSetting swaggerSetting)
    {
        services.AddSwaggerGen(options =>
        {
            //var xmlDuc = Path.Combine(AppContext.BaseDirectory, "swaggerApi.xml");
            //options.IncludeXmlComments(xmlDuc,true);
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = swaggerSetting.ApiName,
                    Description = swaggerSetting.Description,
                    License = new OpenApiLicense { Name = swaggerSetting.LicenseName },
                    Contact = new OpenApiContact
                    {
                        Name = "Amir Hossein Khademi",
                        Email = "avvampier@gmail.com",
                        Url = new Uri("http://amir-khademi.ir")
                    }
                });
            options.EnableAnnotations();
            options.DescribeAllParametersInCamelCase();
            options.IgnoreObsoleteActions();

            #region Versioning

            // Remove version parameter from all Operations
            options.OperationFilter<RemoveVersionParameters>();

            //set version "api/v{version}/[controller]" from current swagger doc verion
            options.DocumentFilter<SetVersionInPaths>();

            //Seperate and categorize end-points by doc version
            options.DocInclusionPredicate((version, desc) =>
            {
                if (!desc.TryGetMethodInfo(out var methodInfo)) return false;
                var versions = methodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions)
                    .ToList();

                return versions.Any(v => $"v{v.ToString()}" == version);
            });

            #endregion

            #region Security

            var url = $"{baseUrl}/api/v1/Auth/LoginSwagger";
            
            options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "Bearer",
                Name = "Bearer",
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(url)
                    }
                }
            });

            options.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "Bearer");

            #endregion

            #region Customize

            options.OperationFilter<ApplySummariesOperationFilter>();

            #endregion
        });
    }

    public static void UseCustomSwagger(this IApplicationBuilder app, string baseUrl, IWebHostEnvironment env)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            var sidebar = Path.Combine(env.ContentRootPath, "wwwroot/assets/swagger-ui/customSidebar.html");
            options.HeadContent = File.ReadAllText(sidebar);
            options.InjectStylesheet("/assets/swagger-ui/x3/theme-flattop.css");
            options.DocExpansion(DocExpansion.None);
            // Display
            options.DefaultModelExpandDepth(2);
            options.DefaultModelRendering(ModelRendering.Example);
            options.DefaultModelsExpandDepth(-1);
            options.DisplayOperationId();
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.ShowExtensions();

            options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            options.SwaggerEndpoint($"{baseUrl}/swagger/v1/swagger.json", "V1 Docs");
        });
    }
}

public class RemoveVersionParameters : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Remove version parameter from all Operations
        var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
        if (versionParameter != null)
            operation.Parameters.Remove(versionParameter);
    }
}

public class SetVersionInPaths : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc == null)
            throw new ArgumentNullException(nameof(swaggerDoc));

        var replacements = new OpenApiPaths();

        foreach (var (key, value) in swaggerDoc.Paths)
            replacements.Add(key.Replace("v{version}", swaggerDoc.Info.Version, StringComparison.InvariantCulture),
                value);

        swaggerDoc.Paths = replacements;
    }
}

public class UnauthorizedResponsesOperationFilter : IOperationFilter
{
    private readonly bool includeUnauthorizedAndForbiddenResponses;
    private readonly string schemeName;

    public UnauthorizedResponsesOperationFilter(bool includeUnauthorizedAndForbiddenResponses,
        string schemeName = "Bearer")
    {
        this.includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
        this.schemeName = schemeName;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors;

        var hasAnynomousEndPoint = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(e =>
            e.GetType() == typeof(AllowAnonymousAttribute));

        //var hasAnonymous = filters.Any(p => p.Filter is AllowAnonymousFilter);
        if (hasAnynomousEndPoint)
            return;

        /*var hasAuthorize = filters.Any(p => p.Filter is AuthorizeFilter);
        if (!hasAuthorize)
            return;*/

        if (includeUnauthorizedAndForbiddenResponses)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
        }

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    }
}

public class ApplySummariesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor == null) return;

        var pluralizer = new Pluralizer();

        var actionName = controllerActionDescriptor.ActionName;
        var singularizeName = pluralizer.Singularize(controllerActionDescriptor.ControllerName);
        var pluralizeName = pluralizer.Pluralize(singularizeName);

        var parameterCount = operation.Parameters.Where(p => p.Name != "version" && p.Name != "api-version").Count();

        if (IsGetAllAction())
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Returns all {pluralizeName}";
        }
        else if (IsActionName("Post", "Create"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Creates a {singularizeName}";

            if (operation.Parameters.Count > 0 && !operation.Parameters[0].Description.HasValue())
                operation.Parameters[0].Description = $"A {singularizeName} representation";
        }
        else if (IsActionName("Read", "Get"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Retrieves a {singularizeName} by unique id";

            if (operation.Parameters.Count > 0 && !operation.Parameters[0].Description.HasValue())
                operation.Parameters[0].Description = $"a unique id for the {singularizeName}";
        }
        else if (IsActionName("Put", "Edit", "Update"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Updates a {singularizeName} by unique id";

            //if (!operation.Parameters[0].OrderDescription.HasValue())
            //    operation.Parameters[0].OrderDescription = $"A unique id for the {singularizeName}";

            if (operation.Parameters.Count > 0 && !operation.Parameters[0].Description.HasValue())
                operation.Parameters[0].Description = $"A {singularizeName} representation";
        }
        else if (IsActionName("Delete", "Remove"))
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"Deletes a {singularizeName} by unique id";

            if (operation.Parameters.Count > 0 && !operation.Parameters[0].Description.HasValue())
                operation.Parameters[0].Description = $"A unique id for the {singularizeName}";
        }
        else
        {
            if (!operation.Summary.HasValue())
                operation.Summary = $"{actionName} {pluralizeName}";
        }

        #region Local Functions

        bool IsGetAllAction()
        {
            foreach (var name in new[] { "Get", "Read", "Select" })
                if (actionName.Equals(name, StringComparison.OrdinalIgnoreCase) && parameterCount == 0 ||
                    actionName.Equals($"{name}All", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}{pluralizeName}", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}All{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Equals($"{name}All{pluralizeName}", StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        bool IsActionName(params string[] names)
        {
            foreach (var name in names)
                if (actionName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                    actionName.Contains($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Contains($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                    actionName.Contains($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        #endregion
    }
}