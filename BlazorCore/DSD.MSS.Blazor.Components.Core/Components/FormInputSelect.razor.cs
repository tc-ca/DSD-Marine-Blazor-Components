namespace DSD.MSS.Blazor.Components.Core.Components
{
    using DSD.MSS.Blazor.Components.Core.Models;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public partial  class FormInputSelect<T> 
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
        public string Label { get; set; }

        /// <summary>
        /// Specifies the Label for default selection
        /// </summary>
        [Parameter] 
        public string SelectLabel { get; set; }

        /// <summary>
        /// Define the validation expression.
        /// </summary>
        [Parameter] 
        public Expression<Func<T>> ValidationFor { get; set; }

        /// <summary>
        /// Specifies whether this field is required.
        /// </summary>
        [Parameter] 
        public string Required { get; set; }

        /// <summary>
        /// Specifies whether this field is required.
        /// </summary>
        [Parameter]
        public bool IsRequired { get; set; }

        /// <summary>
        /// The callback that is called when selected a new value.
        /// </summary>
        [Parameter] 
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        /// <summary>
        /// Child content
        /// </summary>
        [Parameter] 
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies if defalut selection should be displayed.
        /// </summary>
        [Parameter] 
        public bool ShowDefaultOption { get; set; } = true;

        /// <summary>
        /// List of selection options
        /// </summary>
        [Parameter]
        public List<SelectListItem> SelectionList { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Will specify the first item in the selection list as the default value
        /// </summary>
        [Parameter]
        public bool UseFirstItemAsDefault { get; set; }


        protected override void OnInitialized()
        {
            if (UseFirstItemAsDefault && this.SelectionList.Count > 0)
            {
                this.CurrentValueAsString = SelectionList[0].Id;
            }

            base.OnInitialized();

        }

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            if (typeof(T) == typeof(string))
            {
                result = (T)(object)value;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T) == typeof(int))
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                result = (T)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T) == typeof(Guid))
            {
                Guid.TryParse(value, out var parsedValue);
                result = (T)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T).IsEnum)
            {
                // There's no non-generic Enum.TryParse (https://github.com/dotnet/corefx/issues/692)
                try
                {
                    result = (T)Enum.Parse(typeof(T), value);
                    validationErrorMessage = null;

                    return true;
                }
                catch (ArgumentException)
                {
                    result = default;
                    validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";

                    return false;
                }
            }

            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(T)}'.");
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
