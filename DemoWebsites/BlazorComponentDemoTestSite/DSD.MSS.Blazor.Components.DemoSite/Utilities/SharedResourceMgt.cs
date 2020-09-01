using DSD.MSS.Blazor.Components.Core.Demo.Resources;

namespace DSD.MSS.Blazor.Components.Core.Demo.Utilities
{
    public class SharedResourceMgt
    {
        public string this[string name]
        {
            get
            {
                return SharedResource.ResourceManager.GetString(name, SharedResource.Culture);
            }
        }

    }
}
