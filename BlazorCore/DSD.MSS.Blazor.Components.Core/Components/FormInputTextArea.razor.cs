﻿namespace DSD.MSS.Blazor.Components.Core.Components
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Linq.Expressions;

    public partial class FormInputTextArea<T> : FormInputComponentBase<T>
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
        /// Specifies the rows of text area.
        /// </summary>
        [Parameter] 
        public int Rows { get; set; }

        /// <summary>
        /// Specifies the cols of text area.
        /// </summary>
        [Parameter] 
        public int Cols { get; set; }

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
