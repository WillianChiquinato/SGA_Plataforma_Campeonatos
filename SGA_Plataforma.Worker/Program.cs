using SGA_Plataforma.Worker;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

var redisSettings = ResolveRedisSettings(builder.Configuration);
if (redisSettings.Enabled)
{
	builder.Services.AddSingleton(redisSettings);
	builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisSettings.Configuration));
}

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

static RedisSettings ResolveRedisSettings(ConfigurationManager configuration)
{
	var endpoint = Environment.GetEnvironmentVariable("REDIS_ENDPOINT");
	var password = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
	var portValue = Environment.GetEnvironmentVariable("REDIS_PORT");

	if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(password))
	{
		return RedisSettings.Disabled();
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

	return RedisSettings.CreateEnabled(options);
}

static bool? ParseBoolean(string? value)
{
	return bool.TryParse(value, out var parsedValue) ? parsedValue : null;
}

sealed record RedisSettings(bool Enabled, ConfigurationOptions Configuration)
{
	public static RedisSettings Disabled()
		=> new(false, new ConfigurationOptions());

	public static RedisSettings CreateEnabled(ConfigurationOptions configuration)
		=> new(true, configuration);
}
