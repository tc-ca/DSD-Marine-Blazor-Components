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

        /// <summary>
        /// Specifies whether this field is required.
        /// </summary>
        [Parameter]
        public bool IsRequired { get; set; }

        private TimeSpan LocalTime = DateTime.Now.TimeOfDay;
        protected string TimeValue { get => GetTimeFromDateTime(CurrentValue); }
        protected void OnDateChange(DateTime? date)
        {
            if (date != null)
            {
                CurrentValue = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, LocalTime.Hours, LocalTime.Minutes, 0);
            }
        }

        protected void OnTimeChange(string time)
        {
            if (TimeSpan.TryParse(time, out LocalTime))
            {
                DateTime? newDate = CurrentValue.Value;
                if (newDate != null)
                {
                    CurrentValue = new DateTime(newDate.Value.Year, newDate.Value.Month, newDate.Value.Day, LocalTime.Hours, LocalTime.Minutes, 0);
                }
            }
        }

        private string GetTimeFromDateTime(DateTime? date)
        {
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
