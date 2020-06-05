using GuardameLugar.Core;
using GuardameLugar.Core.Dacs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GuardameLugar
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
			services.AddControllers();
			services.AddScoped<IClienteService, ClienteService>();
			services.AddScoped<IGarageService, GarageService>();
			services.AddScoped<IGuardameLugarDacService, GuardameLugarDacService>();
			services.AddCors(options =>
			{
				options.AddPolicy("AllowOrigin",
					builder =>
					{
						builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
					}
					);
			});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Guardame Lugar",
					Version = "v1",
					Description = "REST API for Guardame Lugar Integration"
				});

			});


		}
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseSwagger(c =>
			{
				c.RouteTemplate = "GuardameLugar/docs/{documentName}/swagger.json";
			});

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/GuardameLugar/docs/v1/swagger.json", "GuardameLugar");
				c.RoutePrefix = "GuardameLugar/docs";
			});
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
