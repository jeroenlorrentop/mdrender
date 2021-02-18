using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Markdown.Builder;
using Markdown.Render;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Markdown
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        IEnumerable<Type> GetTypesWith<TAttribute>(bool inherit)
            where TAttribute : System.Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where t.IsDefined(typeof(TAttribute), inherit)
                select t;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var pageDescriptions = GetTypesWith<MarkdownDescription>(true)
                .Select(x => new
                {
                    Description = (MarkdownDescription) x.GetCustomAttribute(typeof(MarkdownDescription)),
                    Type = x
                });

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                foreach (var pageDescription in pageDescriptions)
                {
                    endpoints.MapGet($"/{pageDescription.Description.Path}", context => GetContent(pageDescription.Type, context));
                }
            });
        }

        public async Task GetContent(Type type, HttpContext context)
        {
            await Task.CompletedTask;

            var ctor = type.GetConstructor(new[] {typeof(MarkDownBuilder)}); // todo use dependency injection

            if (ctor == null)
            {
                throw new Exception("no ctor found");
            }

            dynamic instance = ctor.Invoke(new object[]
            {
                new MarkDownBuilder()
            });

            string content = instance.GetContent();

            var html = Markdig.Markdown.ToHtml(content);

            await context.Response.WriteAsync(html);

        }
    }
}
