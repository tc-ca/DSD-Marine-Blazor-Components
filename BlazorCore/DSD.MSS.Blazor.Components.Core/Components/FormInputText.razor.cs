namespace DSD.MSS.Blazor.Components.Core.Components
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using System;
    using System.Linq.Expressions;

    public partial class FormInputText<T> : InputBase<T>
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
        /// Specifies the field icon(inside the input area)
        /// </summary>
        [Parameter]
        public string InputAreaIcon { get; set; }

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            if (typeof(T) == typeof(string))
            {
                result = (T)(object)value;
                validationErrorMessage = null;

                return true;
            }

            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(T)}'.");
        }
    }
}
