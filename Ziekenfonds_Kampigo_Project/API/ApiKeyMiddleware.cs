using Microsoft.AspNetCore.Identity;
using Models;

namespace Ziekenfonds_Kampigo_Project.API
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "Kampigo-Mobile-API-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            // Beveilig alleen routes die beginnen met "/api"
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                // Controleer of de API-key is meegegeven in de headers
                if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("API sleutel niet meegegeven.");
                    return;
                }

                // Vergelijk de API-key met die in de configuratie
                var apiKey = configuration.GetValue<string>("ApiKey");
                if (!apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = 403; // Forbidden
                    await context.Response.WriteAsync("Geen toegang.");
                    return;
                }

                // Haal UserManager op via dependency injection
                var userManager = context.RequestServices.GetRequiredService<UserManager<CustomUser>>();

                // Controleer of de gebruiker is geauthenticeerd en de juiste rol heeft
                var user = context.User;
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Gebruiker is niet geauthenticeerd.");
                    return;
                }

                // Controleer of de gebruiker de rol 'Hoofdmonitor' heeft
                var isHoofdmonitor = await userManager.IsInRoleAsync(await userManager.GetUserAsync(user), "Hoofdmonitor");
                if (!isHoofdmonitor)
                {
                    context.Response.StatusCode = 403; // Forbidden
                    await context.Response.WriteAsync("Gebruiker is geen hoofdmonitor.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
