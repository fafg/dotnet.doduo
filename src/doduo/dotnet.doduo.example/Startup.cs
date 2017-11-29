using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace dotnet.doduo.example
{
    public class Startup
    {
        private List<Thread> m_readerThreads = new List<Thread>();
        private List<Thread> m_outGoingThreads = new List<Thread>();
        private ConcurrentQueue<HttpContext> m_inComingQueue = new ConcurrentQueue<HttpContext>();
        private ConcurrentQueue<HttpContext> m_outGoingQueue = new ConcurrentQueue<HttpContext>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddDoduo(x =>
            {
                x.UseRabbitMQ();
            });

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Seed API", Version = $"1.0.0.0" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            RouteBuilder routeBuilder = new RouteBuilder(app);

            routeBuilder.MapGet("api/values/{id}", context =>
            {
                return context.Response.WriteAsync("value");
            });

            IRouter routes = routeBuilder.Build();
            app.UseRouter(routes);
            app.UseDoduo();

            app.UseMvc(routes2 =>
            {
                routes2.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes2.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Comunique-se Template Api");
            });
        }
    }
}
