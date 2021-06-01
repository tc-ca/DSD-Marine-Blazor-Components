namespace DSD.MSS.Blazor.Components.Core.Components
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Linq.Expressions;

    public partial class FormInputDate
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
        /// Specifies the Field Label
        /// </summary>
        [Parameter] 
        public bool Time { get; set; }

        /// <summary>
        /// Define the validation expression.
        /// </summary>
        [Parameter] 
        public Expression<Func<DateTime?>> ValidationFor { get; set; }

        /// <summary>
        /// Specifies whether this field is required.
        /// </summary>
        [Parameter] 
        public string Required { get; set; }

        protected override bool TryParseValueFromString(string value, out DateTime? result, out string validationErrorMessage)
        {
            result = CurrentValue;
            validationErrorMessage = "";
            return true;
        }
    }
}
