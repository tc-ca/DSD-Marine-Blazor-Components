using DSD.MSS.Blazor.Components.AddressComplete.Resources;
using System.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Timers;
using System.Globalization;

namespace DSD.MSS.Blazor.Components.AddressComplete
{
#pragma warning disable CS0169, CS0649
    public partial class Autocomplete<TIndex, TValue> : ComponentBase, IDisposable
    {
        #region fields
        private EditContext _editContext;
        private FieldIdentifier _fieldIdentifier;
        private Timer _debounceTimer;
        private string _searchText = string.Empty;
        private bool _eventsHookedUp = false;
        private bool _resettingControl = false;
        private ElementReference _searchInput;
        private ElementReference _mask;
        private TValue _value;
        #endregion

        #region private properties

        private ResourceManager Localizer { get; set; } = new ResourceManager(typeof(Index));

        private string FieldCssClasses => _editContext?.FieldCssClass(_fieldIdentifier) ?? "";
        private bool IsSearching { get; set; } = false;
        private bool IsShowingSuggestions { get; set; } = false;

        private TIndex[] Suggestions { get; set; } = new TIndex[0];
        private bool IsSearchingOrDebouncing => IsSearching || _debounceTimer.Enabled;
        private int SelectedIndex { get; set; }
        private string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;

                if (typeof(string) == typeof(TValue))
                {
                    Value = ((TValue)(_searchText as object));
                    ValueChanged.InvokeAsync(Value);
                }
                if (value?.Length == 0)
                {
                    _debounceTimer.Stop();
                    SelectedIndex = -1;
                }
                else
                {
                    _debounceTimer.Stop();
                    _debounceTimer.Start();
                }
            }
        }
        private string HelpMessage { get; set; }
        private bool ShowHelp { get; set; } = false;
        #endregion

        #region overrides
        protected override void OnInitialized()
        {
            if (SearchMethod == null)
            {
                string message = $"{GetType()} requires a {nameof(SearchMethod)} parameter.";
                Logger?.LogError(message);
                throw new InvalidOperationException(message);
            }

            if (ConvertMethod == null)
            {
                if (typeof(TIndex) != typeof(TValue))
                {
                    string message = $"{GetType()} requires a {nameof(ConvertMethod)} parameter.";
                    Logger?.LogError(message);
                    throw new InvalidOperationException(message);
                }

                ConvertMethod = item => item is TValue value ? value : default;

            }

            ValueFormatExpression = ValueFormatExpression ?? ((value) => value.ToString());
            this.Logger?.LogInformation($"No {nameof(ValueFormatExpression)} provided. Default will be used. ");

            _debounceTimer = new Timer();
            _debounceTimer.Interval = Debounce;
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += Search;

            _editContext = CascadedEditContext;
            _fieldIdentifier = FieldIdentifier.Create(ValueExpression);

            Initialize();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if ((firstRender && !Disabled) || (!_eventsHookedUp && !Disabled))
            {
                await Interop.AddKeyDownEventListener(JSRuntime, _searchInput);
                _eventsHookedUp = true;
            }
        }

        protected override void OnParametersSet()
        {
            Initialize();
        }

        #endregion

        #region parameters
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [CascadingParameter] private EditContext CascadedEditContext { get; set; }

        [Parameter] public TValue Value 
        { 
            get => _value; 
            set { 
                _value = value;

                if (typeof(string) == typeof(TValue))
                {
                    _searchText = Value != null ? ValueFormatExpression?.Invoke(Value) : _searchText;
                }
            }
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Callback handler for item selected event
        /// </summary>
        [Parameter] public EventCallback<ChangeEventArgs> ItemSelectedCallback { get; set; }

        /// <summary>
        /// Function which returns a TValue to bind to the component.
        /// </summary>
        [Parameter] public Expression<Func<TValue>> ValueExpression { get; set; }

        /// <summary>
        /// Function which takes a search string argument and returns a list of TIndex
        /// </summary>
        [Parameter] public Func<string, Task<IEnumerable<TIndex>>> SearchMethod { get; set; }

        /// <summary>
        /// When Type returned by Search TIndex differs from type bound to control TValue
        /// A function is required which takes a TIndex and returns a TValue
        /// </summary>
        [Parameter] public Func<TIndex, TValue> ConvertMethod { get; set; }

        /// <summary>
        /// Format to present returned results in
        /// </summary>
        [Parameter] public RenderFragment<TIndex> ListTemplate { get; set; }

        /// <summary>
        /// Function which takes bound value of type TValue 
        /// And returns formatted string of the selected result.
        /// For example, control bound to a person entity(@bind-Value="FormModel.SelectedPerson"): ResultFormatExpression="@((p) => $"{p.Firstname} {p.Location}")"
        /// </summary>
        [Parameter] public Func<TValue, string> ValueFormatExpression { get; set; }


        /// <summary>
        /// Markup to display when no results are found
        /// </summary>
        [Parameter] public RenderFragment NotFoundTemplate { get; set; }

        /// <summary>
        /// Additional html attributes
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        /// <summary>
        /// Minimum characters to type before triggering a search
        /// </summary>
        [Parameter] public int MinimumLength { get; set; } = 1;

        /// <summary>
        /// Pause time in milliseconds between search text input, and search start
        /// </summary>
        [Parameter] public int Debounce { get; set; } = 300;

        /// <summary>
        /// Maximum results to display in list
        /// </summary>
        [Parameter] public int MaximumSuggestions { get; set; } = 10;


        /// <summary>
        /// Provide logger of type ILogger<lookup<TIndex, TValue>> to write logs to
        /// </summary>
        [Parameter] public ILogger<Autocomplete<TIndex, TValue>> Logger { get; set; }

        [Parameter] public Expression<Func<string>> ValidationFor { get; set; }
        [Parameter] public string Id { get; set; }
        /// <summary>
        /// Language: 'fr' or 'en' to display default messages in
        /// </summary>
        [Parameter] public string Language { get; set; }
        [Parameter] public string Label { get; set; }
        /// <summary>
        /// Displays list of suggestions when input is in focus
        /// </summary>
        [Parameter] public bool ShowDropDownOnFocus { get; set; } = false;

        /// <summary>
        /// Displays a dropdown caret which displays results if any, when clicked
        /// </summary>
        [Parameter] public bool ShowDropDownCaret { get; set; } = false;

        /// <summary>
        /// Disables the component
        /// </summary>
        [Parameter] public bool Disabled { get; set; } = false;
        
        /// <summary>
        /// Highlights the currently selected item in the list of suggestions, if any.
        /// </summary>
        [Parameter] public bool HighlightSelectedItemInList { get; set; } = true;

        [Parameter] public string NotFoundMessage { get; set; }

        [Parameter] public string MinimumCharactersNotReachedMessage { get; set; }
        #endregion

        #region public methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_debounceTimer != null)
                {
                    _debounceTimer.Dispose();
                }
            }
        }
        #endregion

        #region private methods
        private bool ShouldShowSuggestions()
        {
            return IsShowingSuggestions &&
                   Suggestions.Any() && !IsSearchingOrDebouncing;
        }

        private string GetSelectedSuggestionClass(TIndex item, int index)
        {
            const string resultClass = "lookup__active-item";

            TValue value = ConvertMethod(item);


            if (Equals(value, Value))
            {
                if (index == SelectedIndex)
                {
                    return "lookup__selected-item-highlighted";
                }

                return "lookup__selected-item";
            }

            if (index == SelectedIndex)
            {
                return resultClass;
            }

            return Equals(value, Value) ? resultClass : string.Empty;
        }

        private async Task SelectResult(TIndex item)
        {
            IsShowingSuggestions = false;
            var value = ConvertMethod(item);

            Value = value;
            await ValueChanged.InvokeAsync(value);

            _searchText = Value != null ? ValueFormatExpression?.Invoke(Value) : _searchText;
            _editContext?.NotifyFieldChanged(_fieldIdentifier);

            await Task.Delay(250);
            await Interop.RunNamedScript(JSRuntime, _searchInput, "lookup.blurElement");

            var args = new ChangeEventArgs();
            args.Value = item;
            await ItemSelectedCallback.InvokeAsync(args);
        }

        private async Task HandleInputFocus()
        {
            if (ShowDropDownOnFocus)
            {
                await ShowMaximumSuggestions();
            }
        }

        private async Task ShowMaximumSuggestions()
        {
            if (_resettingControl)
            {
                while (_resettingControl)
                {
                    await Task.Delay(150);
                }
            }

            IsShowingSuggestions = !IsShowingSuggestions;

            if (IsShowingSuggestions)
            {
                //SearchText = "";
                IsSearching = true;
                await InvokeAsync(StateHasChanged);

                Suggestions = (await SearchMethod?.Invoke(_searchText)).Take(MaximumSuggestions).ToArray();

                IsSearching = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private bool ShowNotFound()
        {
            NotFoundMessage = NotFoundMessage ?? Localizer.GetString("NotFoundMessage");
            return IsShowingSuggestions &&
                   !IsSearchingOrDebouncing &&
                   !Suggestions.Any();
        }

        private async void Search(Object source, ElapsedEventArgs e)
        {
            if (_searchText == null || _searchText.Length < MinimumLength)
            {
                HelpMessage = MinimumCharactersNotReachedMessage ??
                    string.Format(Localizer.GetString("MinimumLengthMessage"), MinimumLength);

                ShowHelp = true;
                await InvokeAsync(StateHasChanged);
                return;
            }

            ShowHelp = false;
            IsSearching = true;
            await InvokeAsync(StateHasChanged);
            Suggestions = (await SearchMethod?.Invoke(_searchText)).Take(MaximumSuggestions).ToArray();

            IsSearching = false;
            IsShowingSuggestions = true;
            SelectedIndex = 0;
            await InvokeAsync(StateHasChanged);
        }

        private async Task ResetControl()
        {
            if (!_resettingControl)
            {
                _resettingControl = true;
                await Task.Delay(200);
                IsShowingSuggestions = false;

                Initialize();
                _resettingControl = false;
            }
        }

        private async Task HandleClear()
        {
            SearchText = "";
            await ValueChanged.InvokeAsync(default);
            

            _editContext?.NotifyFieldChanged(_fieldIdentifier);

            await Task.Delay(250); // Possible race condition here.
            await Interop.RunNamedScript(JSRuntime, _searchInput, "lookup.setFocus");
        }

        private void Initialize()
        {
            
            if (string.IsNullOrEmpty(_searchText))
            {
                _searchText = Value != null ? ValueFormatExpression?.Invoke(Value) : _searchText;
            }
            IsShowingSuggestions = false;            
        }

        private bool ShouldShowHelp()
        {
            return SearchText.Length > 0 &&
                ShowHelp && HelpMessage != null;
        }

        private async Task HandleKeyup(KeyboardEventArgs args)
        {
            if ((args.Key == "ArrowDown" || args.Key == "Enter") && !IsShowingSuggestions)
            {
                await ShowMaximumSuggestions();
            }

            if (args.Key == "ArrowDown")
            {
                MoveSelection(1);
            }
            else if (args.Key == "ArrowUp")
            {
                MoveSelection(-1);
            }
            else if (args.Key == "Escape")
            {
                Initialize();
            }
            else if (args.Key == "Enter" && Suggestions.Count() == 1)
            {
                await SelectTheFirstAndOnlySuggestion();
            }
            else if (args.Key == "Enter" && SelectedIndex >= 0 && SelectedIndex < Suggestions.Count())
            {
                await SelectResult(Suggestions[SelectedIndex]);
            }
        }

        private async Task HandleKeyUpOnShowDropDown(KeyboardEventArgs args)
        {
            if (args.Key == "ArrowDown")
            {
                MoveSelection(1);
            }
            else if (args.Key == "ArrowUp")
            {
                MoveSelection(-1);
            }
            else if (args.Key == "Escape")
            {
                Initialize();
            }
            else if (args.Key == "Enter" && Suggestions.Length == 1)
            {
                await SelectTheFirstAndOnlySuggestion();
            }
            else if (args.Key == "Enter" && SelectedIndex >= 0 && SelectedIndex < Suggestions.Length)
            {
                await SelectResult(Suggestions[SelectedIndex]);
            }
            else if (args.Key == "Enter")
            {
                await ShowMaximumSuggestions();
            }
        }

        private void MoveSelection(int count)
        {
            var index = SelectedIndex + count;

            if (index >= Suggestions.Length)
            {
                index = 0;
            }

            if (index < 0)
            {
                index = Suggestions.Length - 1;
            }

            SelectedIndex = index;
        }
        
        private Task SelectTheFirstAndOnlySuggestion()
        {
            SelectedIndex = 0;
            return SelectResult(Suggestions[SelectedIndex]);
        }

        #endregion
    }
}
