using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Markdig;

namespace PMS.Web;
public static class Extensions
{
    // -------------------------- VIEW Authorisation Helper -------------------------//
    // ClaimsPrincipal - HasOneOfRoles extension method to check if a user has any of the roles in a comma separated string
    public static bool HasOneOfRoles(this ClaimsPrincipal claims, string rolesString)
    {
        // split string into an array of roles
        var roles = rolesString.Split(",");

        // linq query to check that ClaimsPrincipal has one of these roles
        return roles.FirstOrDefault(role => claims.IsInRole(role)) != null;
    }

    // --------------------------- AUTHENTICATION Helper ----------------------------//
    // IServiceCollection extension method adding cookie authentication 
    public static void AddCookieAuthentication(this IServiceCollection services, 
                                                    string notAuthorised = "/User/ErrorNotAuthorised", 
                                                    string notAuthenticated= "/User/ErrorNotAuthenticated")
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.AccessDeniedPath = notAuthorised;
                    options.LoginPath = notAuthenticated;
                });
    }

    // --------------------------- AUTHORISATION Helper ----------------------------//
    public static void AddPolicyAuthorisation(this IServiceCollection services)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies

        services.AddAuthorization( options => {
            // add policies here
            options.AddPolicy("RolePolicy", policy => 
                policy.RequireRole("admin","manager")
            ); 
        
            options.AddPolicy("IsManagerRoleOrIsGuestEmail", policy => 
                policy.RequireAssertion(context => 
                    context.User.HasOneOfRoles("manager") || 
                    context.User.Claims
                            .FirstOrDefault( c => c.Type == ClaimTypes.Email).Value == "guest@mail.com"
                ) 
            );  
            // for more sophisticated policies see resource based policies
            // https://learn.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased  
            
        });
    }

    // --------------------------- AUTHENTICATION Helper ----------------------------//
    // ClaimsPrincipal extension method to extract user id (sid) from claims
    public static int GetSignedInUserId(this ClaimsPrincipal user)
    {
        if (user != null && user.Identity != null && user.Identity.IsAuthenticated) {
            // id stored as a string in the Sid claim - convert to an int and return
            Claim sid = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid) ?? throw new KeyNotFoundException("Sid Claim is not found in the identity");
            try
            {
                return Int32.Parse(sid.Value);  
            } catch (FormatException) {
                throw new KeyNotFoundException("Sid Claim value is invalid - not an integer");  
            }
        }
        return 0;
    }

    // =========================== markdig extension ======================================
    public static string ParseAsMarkdown(this string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .DisableHtml() // stops use of script tags or other potential xss issues
            .Build();
        return Markdown.ToHtml(markdown, pipeline);
    }
    
}

