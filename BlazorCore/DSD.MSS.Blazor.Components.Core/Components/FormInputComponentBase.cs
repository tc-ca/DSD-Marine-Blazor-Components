namespace DSD.MSS.Blazor.Components.Core.Components
{
    using DSD.MSS.Blazor.Components.Core.Components.Interfaces;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using System;
    using System.Collections.Generic;

    public class FormInputComponentBase<T> : InputBase<T>, IReadonlyInputComponent
    {
        /// <summary>
        /// Gets or sets whether the component is readonly
        /// </summary>
        [Parameter]
        public bool IsReadonly { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (this.IsReadonly)
            {
                this.AdditionalAttributes = new Dictionary<string, object>()
                {
                    { "readonly", "" }
                };
            }
        }
        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
