namespace DSD.MSS.Blazor.Components.Core.Components.Buttons
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;

    public partial class LoadingButton
    {
        /// <summary>
        /// Gets or sets the render fragment for the button before loading.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the button's text.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets whether the buttin is loading or not.
        /// </summary>
        [Parameter]
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets or sets the event call back for the button click.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnClickCallback { get; set; }

        /// <summary>
        /// Gets or sets the event callback for the is loading value changed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> IsLoadingChanged { get; set; }

        private async Task OnButtonClick()
        {
            this.IsLoading = true;
            await this.IsLoadingChanged.InvokeAsync(this.IsLoading);
            if (this.OnClickCallback.HasDelegate)
            {
                await this.OnClickCallback.InvokeAsync(this.IsLoading);
            }
        }
    }
}
