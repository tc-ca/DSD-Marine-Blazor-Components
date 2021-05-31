namespace DSD.MSS.Blazor.Components.Core.Components
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Threading.Tasks;

    public partial class FormInputCheckbox : FormInputComponentBase<bool>
    {
        /// <summary>
        /// Specifies the Field ID
        /// </summary>
        [Parameter] 
        public string Id { get; set; }

        /// <summary>
        /// Specifies the Field Label
        /// </summary>
        [Parameter] 
        public string Text { get; set; }

        /// <summary>
        /// The callback that is called when the checkbox is checked or unchecked.
        /// </summary>
        [Parameter] 
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        protected override bool TryParseValueFromString(string value, out bool result, out string validationErrorMessage)
        {
            if (typeof(bool) == typeof(string))
            {
                result = (bool)(object)bool.Parse(value);
                validationErrorMessage = null;

                return true;
            }

            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(bool)}'.");
        }

        /// <summary>
        /// Handle onchange event.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual async Task OnChangeAsync(ChangeEventArgs e)
        {
            await this.OnChange.InvokeAsync(e);
        }
    }
}
