using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NorthWind.Membership.Backend.AspNetIdentity.Options;
using NorthWind.Membership.Backend.Core.Options;
using NorthWind.Sales.Backend.SmtpGateways.Options;

internal static class Startup
{
    public static WebApplication CreateWebApplication(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenBearer();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(config =>
            {
                config.AllowAnyMethod();
                config.AllowAnyHeader();
                config.AllowAnyOrigin();
            });
        });

        builder.Services.AddNorthWindSalesServices(
            dbOptions =>
                builder.Configuration.GetSection(DBOptions.SectionKey)
                    .Bind(dbOptions),
            smtpOptions =>
                builder.Configuration.GetSection(SmtpOptions.SectionKey)
                    .Bind(smtpOptions),
            membershipDBOptions =>
                builder.Configuration.GetSection(MembershipDBOptions.SectionKey)
                    .Bind(membershipDBOptions),
            jwtOptions =>
                builder.Configuration.GetSection(JwtOptions.SectionKey)
                    .Bind(jwtOptions));

        // Configurar autenticación JWT Bearer
        builder.Services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                builder.Configuration.GetSection(JwtOptions.SectionKey)
                    .Bind(options.TokenValidationParameters);

                string SecurityKey = builder.Configuration
                    .GetSection(JwtOptions.SectionKey)
                    [nameof(JwtOptions.SecurityKey)];

                byte[] SecurityKeyBytes = Encoding.UTF8.GetBytes(SecurityKey);

                options.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(SecurityKeyBytes);
            });

        builder.Services.AddAuthorization();

        return builder.Build();
    }

    public static WebApplication ConfigureWebApplication(
        this WebApplication app)
    {
        app.UseExceptionHandler(builder => { });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();

        app.UseAuthentication();   // ← Antes de Authorization
        app.UseAuthorization();

        app.MapNorthwindSalesEndpoints();

        return app;
    }
}