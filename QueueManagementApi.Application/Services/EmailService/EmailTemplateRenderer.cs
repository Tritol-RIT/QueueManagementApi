using RazorLight;
using System.Reflection;

namespace QueueManagementApi.Application.Services.EmailService
{
    public class IEmailTemplateRenderer
    {
        private readonly IRazorLightEngine _razorEngine;

        public IEmailTemplateRenderer()
        {
            _razorEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderEmailTemplateAsync<TModel>(string templatePath, TModel model)
        {
            return await _razorEngine.CompileRenderAsync(templatePath, model);
        }
    }
}
