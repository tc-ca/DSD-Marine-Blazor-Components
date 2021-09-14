namespace DSD.MSS.Blazor.Components.Core.Components
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System;
    using System.Linq.Expressions;

    public partial class FormInputDate
    {

        [Inject]
        IJSRuntime JS { get; set; }

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

        [Parameter]
        public string DateFormatToUse { get; set; }

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

        /// <summary>
        /// Specifies whether this field is required.
        /// </summary>
        [Parameter]
        public bool IsRequired { get; set; }


        private TimeSpan LocalTime = DateTime.Now.TimeOfDay;
        protected string TimeValue { get => GetTimeFromDateTime(CurrentValue); }
        protected async void OnDateChange(DateTime? date)
        {
            if (date != null)
            {
                CurrentValue = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, LocalTime.Hours, LocalTime.Minutes, 0);
            }

            if (!string.IsNullOrWhiteSpace(this.DateFormatToUse))
            {
                await JS.InvokeVoidAsync("test1",this.Id);
            }
        }

        protected void OnTimeChange(string time)
        {
            TimeSpan newTime;
            if (TimeSpan.TryParse(time, out newTime))
            {
                LocalTime = newTime;
                DateTime? newDate = CurrentValue;
                if (newDate != null)
                {
                    CurrentValue = new DateTime(newDate.Value.Year, newDate.Value.Month, newDate.Value.Day, LocalTime.Hours, LocalTime.Minutes, 0);
                }
            }
        }

        private string GetTimeFromDateTime(DateTime? date)
        {
            if (date != null)
            {
                LocalTime = ((DateTime)date).TimeOfDay;
            }
            return date?.ToString("HH:mm");
        }

        protected override bool TryParseValueFromString(string value, out DateTime? result, out string validationErrorMessage)
        {
            result = CurrentValue;
            validationErrorMessage = "";
            return true;
        }
    }
}
