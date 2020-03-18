using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AcademicSystemApi;
using BLL.Impl;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;

namespace SistemaAcademico
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
            #region Dependency injection
            // into controller

            Type[] InterfacesTypes = Assembly.GetAssembly(typeof(AcademyContext)).GetTypes().Where(c => c.IsInterface && c.Name.Contains("Repository") && !c.Name.Contains("IRepository")).ToArray();
            Type[] ClassesTypes = Assembly.GetAssembly(typeof(AcademyContext)).GetTypes().Where(c => c.IsClass && c.Name.Contains("Repository")).ToArray();
            for (int i = 0; i < InterfacesTypes.Length; i++)
            {
                services.AddTransient(InterfacesTypes[i], ClassesTypes[i]);
            }
            Type[] InterfacesTypesServices = Assembly.GetAssembly(typeof(AttendanceService)).GetTypes().Where(c => c.IsInterface && c.Name.Contains("Service") && !c.Name.Contains("IService")).ToArray();
            Type[] ClassesTypesServices = Assembly.GetAssembly(typeof(AttendanceService)).GetTypes().Where(c => c.IsClass && c.Name.Contains("Service")).ToArray();
            for (int i = 0; i < InterfacesTypesServices.Length; i++)
            {
                services.AddTransient(InterfacesTypesServices[i], ClassesTypesServices[i]);
            }
            // services.AddTransient<IUserService, UserService>()
            //or AddScoped (build 1x per request)
            services.AddDbContextPool<AcademyContext>(c => c.UseSqlServer(Configuration["ConnectionString"]));
            #endregion

            services.AddCors();
            services.AddControllers();

            byte[] key = Encoding.UTF8.GetBytes(Settings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Academic System", Version = "v1" });
            });

            services.AddControllers().AddJsonOptions(option => { option.JsonSerializerOptions.PropertyNamingPolicy = null; option.JsonSerializerOptions.MaxDepth = 256; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Academic System V1");
            });

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
