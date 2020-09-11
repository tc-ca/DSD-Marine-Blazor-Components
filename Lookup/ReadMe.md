# Autocomplete and CPAddressLookup Blazor Components

This lookup component is based on the Blazored Typeahead, by Chris  Sainty. see https://github.com/Blazored/Typeahead

The CPAddressLookup component requires a valid Canada Post Address Complete API key to work. This can be obtained from here https://www.canadapost.ca/pca/login/
3 Projects:
	Demo.Web - demonstrates the use of the CPAddressLookup in a form. This is the contact info page of the MIU ext project, simplified.
	Lookup - Blazor library project of the lookup components
	Lookup.Test - Test project, not used

CPAddressLookup Parameters:

ItemSelectedCallback - event handler for when an item (address or group) is selected from the list of suggestions

SearchMethod - A method which performs the search, takes type TIndex, and returns type TValue

ListTemplate - Formatter for each item displayed in the list of suggestions

ValueFormatExpression - Formatter for the selected value of type TValue, returns a result bound to input

NotFoundTemplate - Markup to display when no results are found

AdditionalAttributes - Additional html attributes

MinimumLength - Minimum characters to type before triggering a search

Debounce - Pause time in milliseconds between search text input, and search start

MaximumSuggestions - Maximum results to display in list

Logger - Optional logger of type ILogger<lookup<TIndex, TValue>> to write logs to

ValidationFor - Validator expression

Id - Unique identifier

Language - Language: 'fr' or 'en' to display default messages in

Label - Label text to display above

ShowDropDownOnFocus - When set to true, displays list of suggestions when input is in focus

ShowDropDownCaret - When true, displays a dropdown caret which displays results if any, when clicked

Disabled - When true, disabled the component

HighlightSelectedItemInList - When true, highlights the currently selected item in the list of suggestions, if any.

NotFoundMessage - Message to be displayed when no suggestions found

MinimumCharactersNotReachedMessage - Message to be displayed when the minimum characters to trigger a search is not reached.