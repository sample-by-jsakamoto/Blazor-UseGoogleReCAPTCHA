using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorReCaptchaSample.Server
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<reCAPTCHAVerificationOptions>(Configuration.GetSection("reCAPTCHA"));
            services.AddHttpClient();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //var clientBlazorWebRootPath = default(string);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            //else
            //{
            //    if (env.WebRootPath != null)
            //    {
            //        var pathOfIndex = Path.Combine(env.WebRootPath, "index.html");
            //        var pathOfContent = Path.Combine(env.WebRootPath, "_content");
            //        if (!File.Exists(pathOfIndex) && Directory.Exists(pathOfContent))
            //        {
            //            clientBlazorWebRootPath = Directory.GetDirectories(pathOfContent).FirstOrDefault();
            //            if (clientBlazorWebRootPath != null)
            //            {
            //                env.WebRootPath = clientBlazorWebRootPath;
            //            }
            //        }
            //    }
            //}

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToFile("index.html");
            });

            //if (clientBlazorWebRootPath != null)
            //{
            //    app.UseStaticFiles(new StaticFileOptions
            //    {
            //        FileProvider = new PhysicalFileProvider(clientBlazorWebRootPath)
            //    });
            //}
        }
    }
}
