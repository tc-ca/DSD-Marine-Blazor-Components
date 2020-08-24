
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Demo.Utilities
{
    public class CustomRequestCultureProvider : RequestCultureProvider
    {
        private string _culture = "en-CA";
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.StartsWith("/en", StringComparison.OrdinalIgnoreCase))
            {
                _culture = "en-CA";
            }
            if (httpContext.Request.Path.Value.StartsWith("/fr", StringComparison.OrdinalIgnoreCase))
            {
                _culture = "fr-CA";
            }
            return Task.FromResult(new ProviderCultureResult(_culture));
        }
    }
}
