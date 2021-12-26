using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmollApi.Models;
using Microsoft.EntityFrameworkCore;
using SmollApi.Repositories;

namespace SmollApi
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
            services.AddAutoMapper(typeof(Startup));


            services.AddTransient<IPhoneRepository, PhoneRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBanRepository, BanRepository>();
            services.AddTransient<IFavouriteRepository, FavouriteRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddDbContext<EshopDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EshopAppCon")));

            

                services.AddControllers();
                services.AddSwaggerGen(swagger =>
                {
                    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "SmollApi", Version = "v1" });
                    
                });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmollApi v1"));
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
