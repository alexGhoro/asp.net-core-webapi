using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingSystemApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountingSystemApi
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("myDB"));

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

      app.UseMvc();


      if (!myContext.Paises.Any())
      {
        myContext.Paises.AddRange(new List<Pais>()
        {
          new Pais()
          {
            Nombre = "República Dominicana",
            Provincias = new List<Provincia>()
            {
              new Provincia(){Nombre = "Azua"}
            }
          },
          new Pais()
          {
            Nombre = "México",
            Provincias = new List<Provincia>()
            {
              new Provincia(){Nombre = "Puebla"},
              new Provincia(){Nombre = "Queretaro"}
            }
          },
          new Pais()
          {
            Nombre = "Argentina"
          }
        });

        myContext.SaveChanges();
      }



      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("MVC did't find  anything");
      });
      
    }
  }
}
