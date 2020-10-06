using System;
using System.IO;
using System.Linq;
using InsuranceBackend.DataAccess;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Authentication;
using InsuranceBackend.WebApi.GlobalErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using AutoMapper;

namespace InsuranceBackend.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var notificationMetadata = Configuration.GetSection("NotificationMetadata").Get<NotificationMetadata>();
            services.AddSingleton<IUnitOfWork>(option => new InsuranceUnitOfWork(
                Configuration.GetConnectionString("Insurance")
                ));
            services.AddSingleton<NotificationMetadata>(notificationMetadata);

            var tokenProvider = new JwtProvider("issuer", "audience", "developer_fal");
            services.AddSingleton<ITokenProvider>(tokenProvider);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = tokenProvider.GetValidationParameters();
                });
            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddDirectoryBrowser();
            services.AddAutoMapper(typeof(Startup));

            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // using Microsoft.Extensions.FileProviders;
            // using System.IO;
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(env.ContentRootPath, "Resources", "DigitalizedFiles")),
                RequestPath = "/DigitalizedFiles"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "Resources", "DigitalizedFiles")),
                RequestPath = "/DigitalizedFiles"
            });

            app.UseAuthentication();
            app.ConfigureExceptionHandler();
            app.UseCors("MyPolicy");
            app.UseMvc();
        }
    }
}
