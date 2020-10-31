using System.Data;
using System.Data.SqlClient;
using System.Text;
using AutoMapper;
using ItHappend.RestAPI.Authentication;
using ItHappend.RestAPI.Extensions;
using ItHappend.RestAPI.Filters;
using ItHappened.Application;
using ItHappened.Domain.Repositories;
using ItHappened.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ItHappend.RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EventMapperProfile());
                mc.AddProfile(new TrackMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfiguration>();
            
            services.AddSingleton(jwtConfig);
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                        ValidateLifetime = true
                    };
                });
            
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(LoggingFilter));
            });
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionAttribute));
            });
            
            ConfigureMapper(services);
            RegisterDapperRepository(services);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITracksService, TracksService>();
            services.AddScoped<IEventService, EventService>();
            services.AddSingleton<IJwtIssuer, JwtIssuer>();
            services.AddControllers();
        }
        
        private void RegisterDapperRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDbConnection>(
                serviceProvider => new SqlConnection(GetConnectionString()));
            serviceCollection.AddScoped<IUserRepository, UserRepositoryDapper>();
            serviceCollection.AddScoped<ITrackRepository, TrackRepositoryDapper>();
            serviceCollection.AddScoped<IEventRepository, EventRepositoryDapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private string GetConnectionString()
        {
            return Configuration.GetValue<string>("ConnectionString");
        }
    }
}