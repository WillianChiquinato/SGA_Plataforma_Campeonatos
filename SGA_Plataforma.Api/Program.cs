using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SGA_Plataforma.Infrastructure.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Amazon.S3;
using SGA_Plataforma.Api.DependecyInjection;
using System.Threading.RateLimiting;
using Microsoft.OpenApi.Models;
using Hangfire.PostgreSql;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Filters;
using SGA_Plataforma.Api.Utils;
using System.Text;
using SGA_Plataforma.Api.Jobs;
using SGA_Plataforma.Api.Hubs;
using StackExchange.Redis;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.WebHost.UseUrls("http://0.0.0.0:8080");
}

var enableHttpsRedirection = builder.Configuration.GetValue<bool?>("EnableHttpsRedirection")
    ?? builder.Environment.IsDevelopment();

var allowedCorsOrigins = ResolveAllowedCorsOrigins(builder.Configuration);
var redisSettings = ResolveRedisSettings(builder.Configuration);

var connectionString =
    $"Host={Environment.GetEnvironmentVariable("DB_SERVER")};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
    $"Ssl Mode={Environment.GetEnvironmentVariable("DB_SSL")};";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins(allowedCorsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddControllers();
ConfigureRedis(builder.Services, redisSettings);
var signalRBuilder = builder.Services.AddSignalR();
if (redisSettings.Enabled)
{
    signalRBuilder.AddStackExchangeRedis(options =>
    {
        options.Configuration.ChannelPrefix = RedisChannel.Literal(redisSettings.ChannelPrefix);
        options.ConnectionFactory = async writer =>
            await ConnectionMultiplexer.ConnectAsync(redisSettings.Configuration, writer);
    });
}
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddScoped<TokenService>();
// builder.Services.AddScoped<IEmailService, SendGridEmailService>();

// Hangfire
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString));

builder.Services.AddHangfireServer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<CrudEntityRequestOperationFilter>();

    c.TagActionsBy(api =>
    {
        if (!string.IsNullOrWhiteSpace(api.GroupName))
            return new[] { api.GroupName };

        return new[] { api.ActionDescriptor.RouteValues["controller"] };
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });

    c.OrderActionsBy(apiDesc => apiDesc.GroupName);
});

builder.Services.AddRateLimiter(options =>
{
    var isDevelopment = builder.Environment.IsDevelopment();

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "rate_limit_exceeded",
            message = "Muitas requisicoes em pouco tempo. Aguarde antes de tentar novamente."
        }, cancellationToken);
    };

    // [SEC] global default policy: disabled in development, enabled in shared environments
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ResolveRateLimitKey(httpContext, isDevelopment),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = isDevelopment ? 1000 : 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    // [SEC] login policy: relaxed in development to avoid local lockouts
    options.AddPolicy("loginPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ResolveRateLimitKey(httpContext, isDevelopment),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = isDevelopment ? 30 : 5,
                Window = isDevelopment ? TimeSpan.FromMinutes(1) : TimeSpan.FromSeconds(30),
                QueueLimit = 0
            }));
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddProjectDependencies();
builder.Services.AddHttpClient();
// builder.Services.AddSingleton<IAmazonS3>(_ =>
// {
//     var accountId = Environment.GetEnvironmentVariable("CLOUDFLARE_ACCOUNT_ID")
//                     ?? throw new InvalidOperationException("Variável CLOUDFLARE_ACCOUNT_ID não configurada.");
//     var accessKeyId = Environment.GetEnvironmentVariable("CLOUDFLARE_ACCESS_KEY_ID")
//                       ?? throw new InvalidOperationException("Variável CLOUDFLARE_ACCESS_KEY_ID não configurada.");
//     var secretAccessKey = Environment.GetEnvironmentVariable("CLOUDFLARE_SECRET_ACCESS_KEY")
//                           ?? throw new InvalidOperationException("Variável CLOUDFLARE_SECRET_ACCESS_KEY não configurada.");

//     var s3Config = new AmazonS3Config
//     {
//         ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
//         ForcePathStyle = true,
//         AuthenticationRegion = "auto"
//     };

//     return new AmazonS3Client(accessKeyId, secretAccessKey, s3Config);
// });

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key não configurado. Adicione 'Jwt:Key' em appsettings.json ou defina a variável de ambiente JWT_KEY.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer não configurado. Adicione 'Jwt:Issuer' em appsettings.json ou defina a variável de ambiente JWT_ISSUER.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT Audience não configurado. Adicione 'Jwt:Audience' em appsettings.json ou defina a variável de ambiente JWT_AUDIENCE.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"].ToString();
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrWhiteSpace(accessToken)
                    && (path.StartsWithSegments("/signalR") || path.StartsWithSegments("/hubs/player")))
                {
                    context.Token = accessToken;
                    return Task.CompletedTask;
                }

                if (!string.IsNullOrWhiteSpace(context.Token))
                    return Task.CompletedTask;

                if (context.Request.Cookies.TryGetValue(TokenService.AuthCookieName, out var cookieToken))
                {
                    context.Token = cookieToken;
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

app.UseForwardedHeaders();
app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseRateLimiter();
if (enableHttpsRedirection)
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

// [SEC] Hangfire dashboard requires admin authentication
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.MapControllers();
app.MapHub<OverwolfEventsHub>("/signalR");
app.MapHub<OverwolfEventsHub>("/hubs/player");

static IResult ApiHealthResponse() => Results.Ok("API Running");

app.MapGet("/", ApiHealthResponse);

static string[] ResolveAllowedCorsOrigins(ConfigurationManager configuration)
{
    var configuredOrigins = configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? Array.Empty<string>();

    var legacyOrigin = configuration["Cors:AllowedOrigin"];
    if (!string.IsNullOrWhiteSpace(legacyOrigin))
    {
        configuredOrigins = configuredOrigins.Append(legacyOrigin).ToArray();
    }

    var normalizedOrigins = configuredOrigins
        .Where(origin => !string.IsNullOrWhiteSpace(origin))
        .Select(origin => origin.Trim().TrimEnd('/'))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

    return normalizedOrigins.Length > 0
        ? normalizedOrigins
        : ["http://localhost:3000"];
}
app.Run();

static string ResolveRateLimitKey(HttpContext httpContext, bool isDevelopment)
{
    var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].ToString();
    var forwardedIp = string.IsNullOrWhiteSpace(forwardedFor)
        ? null
        : forwardedFor.Split(',')[0].Trim();

    var remoteIp = forwardedIp
        ?? httpContext.Connection.RemoteIpAddress?.ToString()
        ?? "unknown";

    if (!isDevelopment)
        return remoteIp;

    var path = httpContext.Request.Path.Value ?? "unknown-path";
    return $"{remoteIp}:{path}";
}

static void ConfigureRedis(IServiceCollection services, RedisSettings redisSettings)
{
    services.AddSingleton(redisSettings);

    if (!redisSettings.Enabled)
    {
        services.AddDistributedMemoryCache();
        return;
    }

    services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisSettings.Configuration));
    services.AddStackExchangeRedisCache(options =>
    {
        options.ConfigurationOptions = redisSettings.Configuration;
        options.InstanceName = redisSettings.InstanceName;
    });
}

static RedisSettings ResolveRedisSettings(ConfigurationManager configuration)
{
    var endpoint = Environment.GetEnvironmentVariable("REDIS_ENDPOINT");
    var password = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
    var portValue = Environment.GetEnvironmentVariable("REDIS_PORT");
    var channelPrefix = Environment.GetEnvironmentVariable("REDIS_CHANNEL_PREFIX");
    var instanceName = Environment.GetEnvironmentVariable("REDIS_INSTANCE_NAME");

    if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(password))
    {
        return RedisSettings.Disabled(channelPrefix!, instanceName!);
    }

    var port = int.TryParse(portValue, out var parsedPort) ? parsedPort : 6379;
    var useSsl = ParseBoolean(Environment.GetEnvironmentVariable("REDIS_SSL")) ?? true;

    var options = new ConfigurationOptions
    {
        AbortOnConnectFail = false,
        Ssl = useSsl,
        Password = password,
        ConnectRetry = 3,
        ConnectTimeout = 10000,
        SyncTimeout = 10000,
        AsyncTimeout = 10000,
        KeepAlive = 60
    };

    options.EndPoints.Add(endpoint, port);

    return RedisSettings.CreateEnabled(options, channelPrefix!, instanceName!);
}

static bool? ParseBoolean(string? value)
{
    return bool.TryParse(value, out var parsedValue) ? parsedValue : null;
}

sealed record RedisSettings(
    bool Enabled,
    ConfigurationOptions Configuration,
    string ChannelPrefix,
    string InstanceName)
{
    public static RedisSettings Disabled(string channelPrefix, string instanceName)
        => new(false, new ConfigurationOptions(), channelPrefix, instanceName);

    public static RedisSettings CreateEnabled(ConfigurationOptions configuration, string channelPrefix, string instanceName)
        => new(true, configuration, channelPrefix, instanceName);
}