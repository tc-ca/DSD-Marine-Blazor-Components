namespace DSD.MSS.Blazor.Components.Core.Components
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

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

        /// <summary>
        /// The C# format of the date
        /// </summary>
        [Parameter]
        public string DateFormatToUse { get; set; }

        /// <summary>
        /// The jquery format to use. This format is different than C#'s format. Example C# - MM/dd/yy is jQuery - mm/dd/y.
        /// Both return the same format but are written differently
        /// </summary>
        [Parameter]
        public string JQueryDateFormat { get; set; }

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

        /// <summary>
        /// If true it displays blank else it displays the current time.
        /// </summary>
        [Parameter]
        public bool ShowDefaultValue { get; set; }

        private TimeSpan LocalTime;

        protected string DateFormattedValue
        {
          get => CurrentValue.HasValue ? CurrentValue.Value.ToString(this.DateFormatToUse) : ShowDefaultValue ? "" : DateTime.Now.ToString(this.DateFormatToUse);
        }

        protected string TimeValue;

        protected override async void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                if (!string.IsNullOrWhiteSpace(this.DateFormatToUse) && !string.IsNullOrWhiteSpace(this.JQueryDateFormat))
                {
                    await JS.InvokeVoidAsync("jqueryDatepicker", this.Id, JQueryDateFormat, "");
                }
            }
        }

        protected void OnDateChange(DateTime? date)
        {
            if (date != null)
            {
                CurrentValue = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, LocalTime.Hours, LocalTime.Minutes, 0);
            }
        }

        protected void OnDateChange(string dateValue)
        {
            if (!string.IsNullOrWhiteSpace(dateValue))
            {
                DateTime date;
                if (JQueryDateFormat.Count(f => f == 'y') == 1)
                {
                    // Displays as 07/13/ | so month/day/
                    var dateString = dateValue.Substring(0, 6);
                    if (int.TryParse(dateValue.Substring(6), out int year))
                    {
                        dateString += CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(year);
                        if (DateTime.TryParse(dateString, out date))
                        {
                            CurrentValue = date;
                        }
                    }
                }
                else if (DateTime.TryParse(dateValue, out date))
                {
                    CurrentValue = date;
                }
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

        protected override void OnInitialized()
        {
            TimeValue = CurrentValue?.ToString("HH:mm");
        }

        protected override bool TryParseValueFromString(string value, out DateTime? result, out string validationErrorMessage)
        {
            result = CurrentValue;
            validationErrorMessage = "";
            return true;
        }
    }
}
