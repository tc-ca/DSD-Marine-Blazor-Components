using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.AddressComplete
{
    public static class Interop
    {
        internal static ValueTask<object> RunNamedScript(IJSRuntime jsRuntime, ElementReference element, 
            string javaScriptFunctionName, object[] args = null)
        {
            return jsRuntime.InvokeAsync<object>(javaScriptFunctionName, element, args);
        }

        internal static ValueTask<object> AddKeyDownEventListener(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<object>("lookup.addKeyDownEventListener", element);
        }
    }
}
