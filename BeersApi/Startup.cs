using Autofac;
using Azure.Storage.Blobs;
using BeersApi.ActionFilters;
using BeersApi.Authorization;
using BeersApi.Infrastructure.Middlewares.CustomClaims;
using BeersApi.Infrastructure.Middlewares.CustomExceptionMiddleware;
using DataAccess;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace BeersApi
{
   public class Startup
   {
      public Startup(IConfiguration configuration/*, IWebHostEnvironment env*/)
      {
         _configuration = configuration;
         //_env = env;
      }

      readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

      private IConfiguration _configuration;
      //private IWebHostEnvironment _env;

      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddCors(options =>
         {
            options.AddPolicy(MyAllowSpecificOrigins,
               builder =>
               {
                  builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
               });
         });

         services.AddControllers()
             .AddNewtonsoftJson(options =>
                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
         services.AddFluentValidationAutoValidation()
             .AddFluentValidationClientsideAdapters();

         services.AddSingleton(x => new BlobServiceClient("UseDevelopmentStorage=true", 
             new BlobClientOptions(BlobClientOptions.ServiceVersion.V2019_07_07)));

         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
               options.Authority = "https://localhost:5000";

               options.TokenValidationParameters = new TokenValidationParameters
               {
                  ValidateAudience = false
               };
            });

         services.AddAuthorization(options =>
         {
            options.AddPolicy("BeersApiRole", policy =>
            {
               policy.RequireAuthenticatedUser();
               policy.RequireClaim("BeersApiRole", "BeersApiAdmin");
            });
            options.AddPolicy("ApiScope", policy =>
            {
               policy.RequireAuthenticatedUser();
               policy.RequireClaim("scope", "beersapi");
            });
            options.AddPolicy("ArnaudEmail", policy =>
            {
               policy.Requirements.Add(new CanDoEverythingRequirement("arnaudPolicy@test.com"));
            });

         });

         services.AddSingleton<IAuthorizationHandler, CanDoEverythingHandler>();

         services.AddScoped<ValidationFilterAttribute>();

         services.AddScoped<ValidatorActionFilter>();

         services.AddAutoMapper(typeof(Startup));

         services.AddDbContextPool<BeersApiContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("beersApi_db")));

         // Register the Swagger generator, defining 1 or more Swagger documents
         services.AddSwaggerGen(options =>
         {
            options.CustomSchemaIds(x => x.FullName);

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.EnableAnnotations();
         });

         services.AddFluentValidationRulesToSwagger();
        }

      public void ConfigureContainer(ContainerBuilder builder)
      {
         // Add any Autofac modules or registrations.
         // This is called AFTER ConfigureServices so things you
         // register here OVERRIDE things registered in ConfigureServices.
         //
         // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
         // when building the host or this won't be called.
         builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
         {
            var context = serviceScope.ServiceProvider.GetRequiredService<BeersApiContext>();
            context.Database.EnsureCreatedAsync();
         }

         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
      }

   }
}
