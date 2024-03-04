using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagementApi.Application.Services.EmailService
{
    public class IEmailTemplateRenderer
    {
        private readonly IRazorLightEngine _razorEngine;

        public IEmailTemplateRenderer(IRazorLightEngine razorEngine)
        {
            _razorEngine = razorEngine;
        }

        public async Task<string> RenderEmailTemplateAsync<TModel>(string templatePath, TModel model)
        {
            return await _razorEngine.CompileRenderAsync(templatePath, model);
        }
    }
}
