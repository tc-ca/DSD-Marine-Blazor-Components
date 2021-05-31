namespace DSD.MSS.Blazor.Components.Core.Components.Interfaces
{
    using Microsoft.AspNetCore.Components;

    public interface IReadonlyInputComponent
    {
        /// <summary>
        /// Gets or sets whether the component is readonly
        /// </summary>
         [Parameter]
         public bool IsReadonly { get; set; }
    }
}
