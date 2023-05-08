using UpBlazor.Application;
using UpBlazor.Infrastructure;
using UpBlazor.WebApi;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddWebApi(builder.Configuration);
services.AddApplication();
services.AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseForwardedHeaders();

app.UseCors(options =>
{
    options.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(builder.Configuration["UiUri"] ?? throw new InvalidOperationException());
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();