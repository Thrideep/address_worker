using AddressWorker.Api.Models;
using AddressWorker.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AddressWorker.Api
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
            services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Include);
            services.AddApiVersioning();
            //services.AddApiVersioning(config => {
            //    config.AssumeDefaultVersionWhenUnspecified = true;
            //    config.DefaultApiVersion = new ApiVersion(1, 0);
            //    config.ReportApiVersions = true;
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AddressWorker.Api", Version = "v1" });
            });

            services.AddSingleton(_ =>
            {
                ServiceApiOptions serviceApiOptions = new();
                Configuration.Bind("ServiceApiOptions", serviceApiOptions);
                return serviceApiOptions;
            });
            services.AddSingleton<IAddressAnalyzeService, AddressAnalyzeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddressWorker.Api v1"));

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddressWorker.Api v1"));
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
