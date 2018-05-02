using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingSystemApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AccountingSystemApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      //Para crear una BD en memoria:
      //services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("myDB"));
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        Configuration.GetConnectionString("defaultConnection")
      ));

      services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer( options =>
          options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = "yourdomain.com",
              ValidAudience = "yourdomain.com",
              IssuerSigningKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(Configuration["Llave_super_secreta"])),
              ClockSkew = TimeSpan.Zero
            }
         );

      services.AddMvc().AddJsonOptions(ConfigureJson);
    }

    private void ConfigureJson(MvcJsonOptions obj)
    {
      obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext myContext)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      app.UseMvc();


      //if (!myContext.Paises.Any())
      //{
      //  myContext.Paises.AddRange(new List<Pais>()
      //  {
      //    new Pais()
      //    {
      //      Nombre = "República Dominicana",
      //      Provincias = new List<Provincia>()
      //      {
      //        new Provincia(){Nombre = "Azua"}
      //      }
      //    },
      //    new Pais()
      //    {
      //      Nombre = "México",
      //      Provincias = new List<Provincia>()
      //      {
      //        new Provincia(){Nombre = "Puebla"},
      //        new Provincia(){Nombre = "Queretaro"}
      //      }
      //    },
      //    new Pais()
      //    {
      //      Nombre = "Argentina"
      //    }
      //  });

      //  myContext.SaveChanges();
      //}

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("MVC did't find  anything");
      });
      
    }
  }
}
