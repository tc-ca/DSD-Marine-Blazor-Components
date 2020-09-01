using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;
using DSD.MSS.Blazor.Components.Core.Demo.Utilities;

namespace DSD.MSS.Blazor.Components.Core.Demo.Pages
{
    public class BasePageComponent : ComponentBase
    {
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] IJSRuntime jsRuntime { get; set; }
        [Inject] SharedResourceMgt localizer { get; set; }

        public string currentRelativePath;
        public string PageTitle;
        [Parameter]
        public string LanguageCode { get; set; } = "en";

        void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            var newRelativePath = navigationManager.ToBaseRelativePath(navigationManager.Uri).ToLower();

            if ((currentRelativePath.StartsWith("en", StringComparison.OrdinalIgnoreCase) && newRelativePath.StartsWith("fr", StringComparison.OrdinalIgnoreCase)) ||
               (currentRelativePath.StartsWith("fr", StringComparison.OrdinalIgnoreCase) && newRelativePath.StartsWith("en", StringComparison.OrdinalIgnoreCase)))
            {
                navigationManager.NavigateTo(navigationManager.Uri, true);
                return;
            }
        }

        public void SetSwitchLanguage()
        {
            navigationManager.LocationChanged += LocationChanged;
            string path = navigationManager.ToBaseRelativePath(navigationManager.Uri);
            currentRelativePath = path.ToLower();
            String[] pathArray = currentRelativePath.Split('/');
            LanguageCode = string.IsNullOrEmpty(pathArray[0]) ? "en" : pathArray[0];
            string switchLanguage = LanguageCode.Equals("en", StringComparison.OrdinalIgnoreCase) ? "fr" : "en";
            string switchCulture = LanguageCode.Equals("en", StringComparison.OrdinalIgnoreCase) ? "fr-CA" : "en-CA";
            path = path.Length >= 2 ? path.Substring(2).Replace("#", "") : "";

            navigationManager.NavigateTo($"{switchLanguage}{path}", true);         
        }

        protected override void OnInitialized()
        {
            string path = navigationManager.ToBaseRelativePath(navigationManager.Uri);
            currentRelativePath = path.ToLower();
            String[] pathArray = currentRelativePath.Split('/');
            LanguageCode = string.IsNullOrEmpty(pathArray[0]) ? "en" : pathArray[0];
            base.OnInitialized();
        }
    }
}