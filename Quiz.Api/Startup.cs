using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quiz.Api.Jwt;
using Quiz.Api.Repositories;
using Quiz.Api.Repositories.Interfaces;
using Quiz.Api.SignalR.Hubs;
using System.Linq;

namespace Quiz.Api
{
    public class Startup
    {
        private readonly string CorsPolicyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();

            string[] allowedHosts = Configuration.GetSection("CorsAllowedHosts").Get<string[]>();
            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(allowedHosts);
            }));

            services.AddSingleton<JwtManager>();
            ConfigureRepositories(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(CorsPolicyName);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<QuizHub>("/QuizHub");
            });

            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            logger.LogInformation("Startup completed, listening on addresses {addresses}", serverAddressesFeature.Addresses.ToList());
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddSingleton<IRoomRepository, InMemoryRoomRepository>();
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<IScoreRepository, InMemoryScoreRepository>();
        }
    }
}
