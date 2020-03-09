using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.API.Services;
using Product.Domain.Interfaces;
using Products.Infrastructure.Entities;
using Products.Infrastructure.MapProfiles;
using Products.Infrastructure.Repository;

namespace Product.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddAutoMapper(_ => GetAutoMapperConfiguration(), AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<ProductDatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("productDb"),
                    option => option.EnableRetryOnFailure()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private Action<IMapperConfigurationExpression> GetAutoMapperConfiguration()
        {
            return configuration =>
            {
                configuration.AddProfile<ProductMapProfile>();
            };
        }
    }
}
