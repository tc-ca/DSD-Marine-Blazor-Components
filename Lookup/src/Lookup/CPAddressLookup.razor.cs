using DSD.MSS.Blazor.Components.AddressComplete.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Resources;
using System.Threading.Tasks;
using System.Timers;

namespace DSD.MSS.Blazor.Components.AddressComplete
{
#pragma warning disable CS0169, CS0649, IDE0044, IDE0051
    public partial class CPAddressLookup<TValue> : ComponentBase, IDisposable
    {
        #region fields
        private EditContext _editContext;
        private FieldIdentifier _fieldIdentifier;
        private Timer _debounceTimer;
        private string _searchText = string.Empty;
        private bool _eventsHookedUp = false;
        private ElementReference _searchInput;
        private ElementReference _mask;
        private const string NEXT_FIND_VALUE = "Find";
        private const int MAX_ADDRESS_GROUP_SUGGESTIONS = 128;
        private const string MIU_DEFAULT_COUNTRY_VALUE = "tcr";
        private const string CANADA_ISO3 = "CAN";
        private const string V = "CAN";
        #endregion

        #region private properties

        private ResourceManager Localizer { get; set; } = new ResourceManager(typeof(Resources.Index));

        private bool IsSearching { get; set; } = false;
        private bool HasError { get; set; } = false;
        private bool IsShowingSuggestions { get; set; } = false;
        private bool AddressGroupFound { get; set; } = false;
        private string AdressGroupSearchText;
        private string AddressGroupLastId { get; set; }
        private CPQuickLookup[] Suggestions { get; set; } = new CPQuickLookup[0];
        private CPCountry[] Countries { get; set; } = new CPCountry[0];
        private int SelectedIndex { get; set; }
        private string HelpMessage { get; set; }
        private bool ShowHelp { get; set; } = false;

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
                if (value.Length == 0)
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

        private string CountryDropdownValue
        {
            get => _selectedCountry;

            set { _selectedCountry = value == MIU_DEFAULT_COUNTRY_VALUE ? null : value; }
        }

        private string FieldCssClasses => _editContext?.FieldCssClass(_fieldIdentifier) ?? "";

        private IDictionary<string, CPQuickLookup> cpLookup;

        #endregion

        #region overrides

        protected override void OnInitialized()
        {
            if (CanadaPostApiKey == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a {nameof(CanadaPostApiKey)} parameter.");
            }

            if (CountriesMethod == null)
            {
                CountriesMethod = () => CPAddressCompleteService.GetCountries();
            }

            if (ResultConverter == null)
            {
                if (typeof(string) != typeof(TValue))
                {
                    string message = $"{GetType()} requires a {nameof(ResultConverter)} parameter.";
                    Logger?.LogError(message);
                    throw new InvalidOperationException(message);
                }

                ResultConverter = item => item is TValue value ? value : default;
            }

            if (ListConverter == null)
            {
                if (typeof(string) != typeof(TValue))
                {
                    string message = $"{GetType()} requires a {nameof(ListConverter)} parameter.";
                    Logger?.LogError(message);
                    throw new InvalidOperationException(message);
                }

                ListConverter = item => item is TValue value ? value : default;
            }

            if (!ShowCountrySelector)
            {
                if (CountryIso3 == null)
                {
                    string message = $"{GetType()} requires a {nameof(CountryIso3)} parameter when {nameof(ShowCountrySelector)} is false.";
                    Logger?.LogError(message);
                    throw new InvalidOperationException(message);
                }
            }

            if (ValueFormatExpression == null)
            {
                ValueFormatExpression = (value) => value.ToString();
                this.Logger?.LogInformation($"No {nameof(ValueFormatExpression)} provided. Default will be used. ");
            }

            CountryDropdownValue = DefaultLookupCountryIso3 ?? CANADA_ISO3;

            if (MaximumSuggestionsFromGroupedAddresses == default) MaximumSuggestionsFromGroupedAddresses = MAX_ADDRESS_GROUP_SUGGESTIONS;
            _debounceTimer = new Timer
            {
                Interval = Debounce,
                AutoReset = false
            };
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
        [Inject] public IHttpClientFactory ClientFactory { get; set; }
        [CascadingParameter] private EditContext CascadedEditContext { get; set; }


        [Parameter]
        public TValue Value
        {
            get => _value;
            set
            {
                _value = value;

                if (typeof(string) == typeof(TValue))
                {
                    _searchText = Value != null ? ValueFormatExpression?.Invoke(Value) : _searchText;
                }
            }
        }

        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter] public EventCallback<string> CountryChanged { get; set; }

        [Parameter] public string DefaultLookupCountryIso3 { get; set; }

        [Parameter] public EventCallback<CPCompleteAddress> AddressRetrieved { get; set; }

        [Parameter] public Expression<Func<TValue>> ValueExpression { get; set; }

        [Parameter] public Expression<Func<string>> ValidationFor { get; set; }
        [Parameter] public string Id { get; set; }

        [Parameter] public string CanadaPostApiKey { get; set; }

        [Parameter] public string Language { get; set; }

        [Parameter] public string Label { get; set; }

        [Parameter] public string RefererURL { get; set; }

        [Parameter] public string CountryNullHelpMessage { get; set; }

        [Parameter] public string CountryIso3 { get; set; }

        [Parameter] public string AddressLine2Prefix { get; set; }

        [Parameter] public EventCallback<IList<string>> ValuesChanged { get; set; }

        [Parameter] public Func<Task<IEnumerable<CPCountry>>> CountriesMethod { get; set; }

        [Parameter] public string NotFoundMessage { get; set; }
        [Parameter] public string ErrorMessage { get; set; }

        [Parameter] public RenderFragment<CPQuickLookup> ListTemplate { get; set; }

        /// <summary>
        /// Function which takes bound value of type TValue
        /// And returns formatted string for display in input.
        /// </summary>
        [Parameter] public Func<TValue, string> ValueFormatExpression { get; set; }

        [Parameter] public RenderFragment CountryOptionsTemplate { get; set; }

        /// <summary>
        /// Provide logger of type ILogger<lookup<TIndex, TValue>> to write logs to
        /// </summary>
        [Parameter] public ILogger<CPAddressLookup<TValue>> Logger { get; set; }

        /// <summary>
        /// Displays a dropdown caret which displays results if any, when clicked
        /// </summary>
        [Parameter] public bool ShowDropDownCaret { get; set; } = false;

        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter] public int MinimumLength { get; set; } = 1;
        [Parameter] public int Debounce { get; set; } = 300;

        [Parameter] public int MaximumSuggestions { get; set; } = 10;

        [Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public bool EnableDropDown { get; set; } = false;

        [Parameter] public bool ShowDropDownOnFocus { get; set; } = false;

        [Parameter] public bool ShowCountrySelector { get; set; } = true;

        [Parameter] public bool PrefixBuildingNumberWithSub { get; set; } = false;

        [Parameter] public int MaximumSuggestionsFromGroupedAddresses { get; set; }

        /// <summary>
        /// Highlights the currently selected item in the list of suggestions, if any.
        /// </summary>
        [Parameter] public bool HighlightSelectedItemInList { get; set; } = true;

        [Parameter] public string NoResultsFoundMessage { get; set; }

        [Parameter] public string MinimumCharactersNotReachedMessage { get; set; }

        /// <summary>
        /// When and item is selected from list (of type CPQuickLookup), 
        /// it retrieves the actual address of type CPAddressComplete matching that lookup value.
        /// This function if supplied converts the CPCompleteAddress to TValue for binding
        /// </summary>
        [Parameter] public Func<CPCompleteAddress, TValue> ResultConverter { get; set; }

        [Parameter] public Func<CPQuickLookup, TValue> ListConverter { get; set; }
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
                _debounceTimer?.Dispose();
            }
        }
        #endregion

        #region private methods

        private void Initialize()
        {
            if (string.IsNullOrEmpty(_searchText))
            {
                _searchText = Value != null ? ValueFormatExpression?.Invoke(Value) : _searchText;
            }
            IsShowingSuggestions = false;


            Task<IEnumerable<CPCountry>> task = CountriesMethod.Invoke();
            Countries = task.Result?.ToArray();

            CountryDropdownValue = CountryIso3 == CANADA_ISO3 ? CountryIso3 : CountryDropdownValue;
        }

        private async Task HandleClear()
        {
            SearchText = "";
            HasError = false;
            await ValueChanged.InvokeAsync(default);

            _editContext?.NotifyFieldChanged(_fieldIdentifier);

            await Task.Delay(250); // Possible race condition here.
            await Interop.RunNamedScript(JSRuntime, _searchInput, "lookup.setFocus");
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
                await SelectRetrieveResult(Suggestions[SelectedIndex]);
            }
            else if (args.Key == "Enter")
            {
                await ShowMaximumSuggestions();
            }
        }

        private async Task HandleKeyup(KeyboardEventArgs args)
        {
            if ((args.Key == "ArrowDown" || args.Key == "Enter") && EnableDropDown && !IsShowingSuggestions)
            {
                await ShowMaximumSuggestions();
            }

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
                await SelectRetrieveResult(Suggestions[SelectedIndex]);
            }
        }

        private async Task HandleInputFocus()
        {
            if (ShowDropDownOnFocus)
            {
                await ShowMaximumSuggestions();
            }
        }

        private bool _resettingControl = false;
        private string _selectedCountry;
        private TValue _value;

        private async Task ResetControl()
        {
            if (!_resettingControl)
            {
                _resettingControl = true;
                await Task.Delay(200);
                Initialize();
                _resettingControl = false;
            }
        }

        private async Task ShowMaximumSuggestions(string searchText = "", string lastId = "")
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
                SearchText = searchText;
                IsSearching = true;
                await InvokeAsync(StateHasChanged);

                Suggestions = (await SearchAddresses(_searchText, lastId)).Take(MaximumSuggestions).ToArray();
                IsSearching = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async void Search(Object source, ElapsedEventArgs e)
        {
            if (!AddressGroupFound && _searchText.Length < MinimumLength)
            {
                HelpMessage = MinimumCharactersNotReachedMessage ??
                    string.Format(Localizer.GetString("MinimumLengthMessage"), MinimumLength);
                ShowHelp = true;
                await InvokeAsync(StateHasChanged);
                return;
            }
            if (CountryDropdownValue == null)
            {
                HelpMessage = CountryNullHelpMessage ?? Localizer.GetString("PickACountryMessage");
                ShowHelp = true;
                await InvokeAsync(StateHasChanged);
                return;
            }

            ShowHelp = false;
            IsSearching = true;
            await InvokeAsync(StateHasChanged);

            if (!AddressGroupFound)
            {
                Suggestions = (await SearchAddresses(_searchText)).Take(MaximumSuggestions).ToArray();
            }
            else
            {
                Suggestions = (await SearchAddresses(AdressGroupSearchText, AddressGroupLastId)).ToArray();
                Suggestions = Suggestions.Count() <= MaximumSuggestionsFromGroupedAddresses ? Suggestions : Suggestions.Take(MaximumSuggestionsFromGroupedAddresses).ToArray();
                AdressGroupSearchText = "";
                AddressGroupFound = false;
                AddressGroupLastId = "";
            }

            IsSearching = false;
            IsShowingSuggestions = true;
            SelectedIndex = 0;
            await InvokeAsync(StateHasChanged);
        }

        internal async Task CountrySelectChanged(ChangeEventArgs e)
        {
            string value = e.Value as string;
            value = value == "Choisissez un pays" || value == "Pick a country" ? default : value;
            CountryDropdownValue = value;

            await CountryChanged.InvokeAsync(value);
            await Task.Delay(250); // Possible race condition here.
            await Interop.RunNamedScript(JSRuntime, _searchInput, "lookup.setFocus");
        }

        internal async Task<IEnumerable<CPQuickLookup>> SearchAddresses(string searchText, string lastId = "")
        {
            IList<CPQuickLookup> cpLookups = new List<CPQuickLookup>();
            HasError = false;
            if (CountryDropdownValue == null) return cpLookups;

            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
            {
                try
                {
                    cpLookups = await CPAddressCompleteService.FindSuggestions(
                                clientFactory: ClientFactory,
                                refererURL: RefererURL,
                                key: CanadaPostApiKey,
                                searchterm: searchText,
                                lastid: lastId,
                                searchfor: "Everything",
                                country: CountryDropdownValue,
                                languagepreference: Language,
                                maxsuggestions: MaximumSuggestions,
                                maxresults: MaximumSuggestions);

                    cpLookup = new Dictionary<string, CPQuickLookup>();
                    if (cpLookups != null && cpLookups.Any())
                    {
                        foreach (var item in cpLookups)
                        {
                            if (item.Next == NEXT_FIND_VALUE)
                            {
                                AddressGroupFound = true;
                                AdressGroupSearchText = searchText;
                            }
                            if (!cpLookup.ContainsKey(item.Id)) cpLookup.Add(item.Id, item);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    HasError = true;
                    Logger.LogError(ex.Message, "Network or Address error on calling address complete API.");
                }
                catch (Exception ex)
                {
                    HasError = true;
                    Logger.LogError(ex.Message, "Internal error on calling address complete API.");
                }
            }

            return await Task.FromResult(cpLookups);
        }

        private string GetSelectedSuggestionClass(CPQuickLookup item, int index)
        {
            const string resultClass = "lookup__active-item";

            TValue value = ListConverter(item);


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

        private async Task SelectFindResult(CPQuickLookup item)
        {
            await InvokeAsync(() => AddressGroupLastId = item.Id);

            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private async Task SelectRetrieveResult(CPQuickLookup item)
        {
            CPCompleteAddress selectedAddress = await AddressSelectedAsync(item.Summary);

            Value = ResultConverter.Invoke(selectedAddress);

            //await ValueChanged.InvokeAsync(Value); -- This will cause validation fail 
            _editContext?.NotifyFieldChanged(_fieldIdentifier);

            await AddressRetrieved.InvokeAsync(selectedAddress);

            _searchText = Value != null ? ValueFormatExpression?.Invoke(Value) : _searchText;
            IsShowingSuggestions = false;

            await Task.Delay(250);
        }

        private async Task<CPCompleteAddress> AddressSelectedAsync(string addressSummary)
        {
            var item = cpLookup.FirstOrDefault(i => i.Value.Summary == addressSummary);
            if (item.Key == null) return null;

            CPCompleteAddress address = null;
            try
            {
                var selectedId = item.Key;
                var addressList = await CPAddressCompleteService.Retrieve(ClientFactory, RefererURL, CanadaPostApiKey, selectedId);
             
                address = addressList.FirstOrDefault();
            }
            catch (HttpRequestException ex)
            {
                HasError = true;
                Logger.LogError(ex.Message, "Network or Address error on calling address complete API.");
            }
            catch (Exception ex)
            {
                HasError = true;
                Logger.LogError(ex.Message, "Internal error on calling address complete API.");
            }

            return await Task.FromResult(address);
        }

        private async Task<CPCompleteAddress> ParseCPAddress(CPCompleteAddress a)
        {
            if (!string.IsNullOrEmpty(a.SubBuilding) && !string.IsNullOrWhiteSpace(a.SubBuilding))
            {
                if (PrefixBuildingNumberWithSub)
                {
                    a.Line1 = $"{a.SubBuilding}-{a.BuildingNumber} {a.Street}";
                    a.Line2 = "";
                }
                else
                {
                    a.Line1 = $"{a.BuildingNumber} {a.Street}";
                    a.Line2 = a.SubBuilding;
                    if (AddressLine2Prefix != null)
                    {
                        a.Line2 = $"{AddressLine2Prefix} {a.Line2}";
                    }
                    else
                    {
                        a.Line2 = Localizer.GetString("Addres2Prefix");// Language == "fr" ? $"Unité {a.Line2}" : $"Unit {a.Line2}";
                    }
                }
            }
            else
            {
                a.Line1 = $"{a.BuildingNumber} {a.Street}";
                a.Line2 = "";
            }

            return await Task.FromResult(a);
        }

        private bool ShouldShowHelp()
        {
            return SearchText?.Length > 0 &&
                ShowHelp;
        }

        private bool ShouldShowSuggestions()
        {
            return IsShowingSuggestions &&
                   Suggestions.Any() && !IsSearchingOrDebouncing;
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
            return SelectRetrieveResult(Suggestions[SelectedIndex]);
        }

        private bool IsSearchingOrDebouncing => IsSearching || _debounceTimer.Enabled;
        private bool ShowNotFound()
        {
            NoResultsFoundMessage = NoResultsFoundMessage ?? Localizer.GetString("NotFoundMessage");// "No results found";
            return IsShowingSuggestions &&
                   !IsSearchingOrDebouncing &&
                   !Suggestions.Any();
        }

        #endregion

    }
}
