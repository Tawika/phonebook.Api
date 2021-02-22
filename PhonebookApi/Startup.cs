using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PhonebookApi.Extensions;

namespace PhonebookApi
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
      services.AddDataServices();
      services.AddDatabaseServicesInMemorySQLlite(Configuration);

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhonebookApi", Version = "v1" });
      });
      services.AddCors(options =>
      {
        options.AddPolicy("AllowMyOrigin", builder =>
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
         .AllowAnyHeader());
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhonebookApi v1"));
      }

      ILogger<Startup> logger = loggerFactory.CreateLogger<Startup>();
      app.ConfigureExceptionHandler(logger);

      app.UseHttpsRedirection();

      app.UseCors("AllowMyOrigin");

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}