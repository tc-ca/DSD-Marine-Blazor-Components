using Microsoft.AspNetCore.Components;

namespace DSD.MSS.Blazor.Components.Table
{
    /// <summary>
    /// Option for CustomSelect
    /// </summary>
    public partial class CustomSelectOption
    {
        /// <summary>
        /// Parent table
        /// </summary>
        [CascadingParameter(Name = "CustomSelect")]
        public ICustomSelect CustomSelect { get; set; }

        [Parameter]
        public string Key { get; set; }

        [Parameter]
        public object Value { get; set; }

        /// <summary>
        /// When initialized, tell CustomSelect of this item
        /// </summary>
        protected override void OnInitialized()
        {
            CustomSelect.AddSelect(Key, Value);
        }
    }
}
