import './configurable-date-input-polyfill.scss';
import Input from './input';

const addPolyfillPickers = () => {
    Input.addPickerToOtherInputs();
    // Check if type="date" is supported. feature disabled!
    if (!Input.supportsDateInput()) {
        Input.addPickerToDateInputs();
    }
};

// Run the above code on any <input type="date"> in the document, also on dynamically created ones.
addPolyfillPickers();

document.addEventListener('DOMContentLoaded', () => {
    addPolyfillPickers();
});

// This is also on mousedown event so it will capture new inputs that might
// be added to the DOM dynamically.
document.querySelector('body').addEventListener('mousedown', () => {
    addPolyfillPickers();
});
