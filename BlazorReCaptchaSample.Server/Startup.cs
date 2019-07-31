using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;

namespace BlazorReCaptchaSample.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            var clientBlazorWebRootPath = default(string);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }
            else
            {
                if (env.WebRootPath != null)
                {
                    var pathOfIndex = Path.Combine(env.WebRootPath, "index.html");
                    var pathOfContent = Path.Combine(env.WebRootPath, "_content");
                    if (!File.Exists(pathOfIndex) && Directory.Exists(pathOfContent))
                    {
                        clientBlazorWebRootPath = Directory.GetDirectories(pathOfContent).FirstOrDefault();
                        if (clientBlazorWebRootPath != null)
                        {
                            env.WebRootPath = clientBlazorWebRootPath;
                        }
                    }
                }
            }

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Client.Startup>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });

            if (clientBlazorWebRootPath != null)
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(clientBlazorWebRootPath)
                });
            }
        }
    }
}
