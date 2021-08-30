namespace DSD.MSS.Blazor.Components.Core.Demo.Pages
{
    using System.Threading.Tasks;

    public partial class Buttons
    {
        public bool Button1Loading { get; set; }

        public bool Button2Loading { get; set; }

        protected async Task Button1OnClick(bool isLoading)
        {
            await Task.Delay(5000);
            this.Button1Loading = false;
        }
        protected async Task Button2OnClick(bool isLoading)
        {
            await Task.Delay(3000);
            this.Button2Loading = false;
        }
    }
}
