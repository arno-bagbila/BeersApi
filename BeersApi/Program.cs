using Autofac;
using Autofac.Extensions.DependencyInjection;
using BeersApi;
using BeersApi.Infrastructure.Middlewares.CustomClaims;
using BeersApi.Infrastructure.Middlewares.CustomExceptionMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

// Using a custom DI container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(startup.ConfigureContainer);

var app = builder.Build();

startup.Configure(app, app.Environment);

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "BEERS API V1");
   c.RoutePrefix = string.Empty;
});

app.ConfigureCustomExceptionMiddleware();

app.UseRouting();

app.UseCors("_myAllowSpecificOrigins");

app.UseAuthentication();

app.AddCustomClaims();

app.UseAuthorization();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
