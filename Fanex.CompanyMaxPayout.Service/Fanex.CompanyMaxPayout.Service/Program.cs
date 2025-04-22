using Fanex.CompanyMaxPayout.Service;
using Fanex.CompanyMaxPayout.Service.Services;
using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddConfiguration();
services.AddFanexSql(configuration);
services.AddFanexLogSentry(configuration);
services.AddOneBookUmForService(configuration);
services.AddInternalService();
services.AddRemotingRef();

services.AddHostedService<MainBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

await app.RunAsync();