namespace DSD.MSS.Blazor.Components.Core.Components
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public partial class FormInputNumber<T> : FormInputComponentBase<T>
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

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            if (typeof(T) == typeof(int))
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                result = (T)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T) == typeof(double))
            {
                int.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue);
                result = (T)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T) == typeof(float))
            {
                int.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue);
                result = (T)(object)parsedValue;
                validationErrorMessage = null;

                return true;
            }
            else if (typeof(T) == typeof(decimal))
            {
                int.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue);
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

            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(T)}'.");
        }
    }
}
