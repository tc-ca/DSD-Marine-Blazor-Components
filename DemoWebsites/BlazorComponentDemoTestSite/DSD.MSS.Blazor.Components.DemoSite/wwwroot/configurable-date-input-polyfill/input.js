import Picker from './picker';
import Localisation from './localisations';
import DateFormat from './dateformat';

class Input {
    getLocaleLabels(locale) {
        let localeLabels;

        Object.keys(Localisation).forEach((key) => {
            const localeSet = key;
            let localeList = localeSet.split('_');
            localeList = localeList.map((el) => el.toLowerCase());

            if (localeList.indexOf(locale) >= 0 || localeList.indexOf(locale.substr(0, 2)) >= 0) {
                localeLabels = Localisation[localeSet];
            }
        });
        return localeLabels;
    }

    // determines if min and max values are given
    getDateRange(minAttribute, maxAttribute) {
        const defaultMinDate = new Date("1800");
        const defaultMaxDate = new Date("3000");

        let minDate = defaultMinDate;
        let maxDate = defaultMaxDate;

        // If min Attribute is set
        if (minAttribute && Date.parse(minAttribute)) {
            const givenDate = new Date(minAttribute);
            givenDate.setHours(0, 0, 0, 0);
            minDate = givenDate;
        }
        // If max Attribute is set
        if (maxAttribute && Date.parse(maxAttribute)) {
            const givenDate = new Date(maxAttribute);
            givenDate.setHours(0, 0, 0, 0);
            maxDate = givenDate;
        }

        // in case of invalid input
        if (minDate > maxDate) {
            minDate = defaultMinDate;
            maxDate = defaultMaxDate;
        }

        if (minDate < new Date("0001")) {
            minDate = new Date("0001");
        }

        return [minDate, maxDate];
    }

    // Return false if the browser does not support input[type="date"].
    supportsDateInput() {
        const input = document.createElement('input');
        input.setAttribute('type', 'date');

        const notADateValue = 'not-a-date';
        input.setAttribute('value', notADateValue);

        return !(input.value === notADateValue);
    }

    prepareInputForPicker(input) {
        const inputObject = input;
        inputObject.setAttribute('data-has-picker', '');

        let locale = inputObject.getAttribute('lang')
            || document.body.getAttribute('lang')
            || 'en';

        let dateFormat = inputObject.getAttribute('date-format')
            || document.body.getAttribute('date-format')
            || inputObject.getAttribute('data-date-format')
            || document.body.getAttribute('data-date-format')
            || 'yyyy-mm-dd';

        let minAttribute = inputObject.getAttribute('min')
            || inputObject.getAttribute('data-min');
        let maxAttribute = inputObject.getAttribute('max')
            || inputObject.getAttribute('data-max');

        inputObject.firstDayOfWeek = inputObject.getAttribute('data-first-day')
            || document.body.getAttribute('data-first-day')
            || 'su';

        inputObject.dateRange = this.getDateRange(minAttribute, maxAttribute);
        inputObject.localeLabels = this.getLocaleLabels(locale);

        Object.defineProperties(
            inputObject,
            {
                valueAsDate: {
                    get: () => {
                        if (!inputObject.value) {
                            return null;
                        }
                        const format = dateFormat || 'yyyy-mm-dd';
                        const parts = inputObject.value.match(/(\d+)/g);
                        let i = 0;
                        const fmt = {};

                        format.replace(/(yyyy|dd|mm)/g, (part) => {
                            fmt[part] = i;
                            i += 1;
                        });

                        // return null if no parts found
                        if (!parts) {
                            return null;
                        }

                        // create absolute date of given input
                        const valueAsDate = new Date();
                        valueAsDate.setFullYear(parts[fmt.yyyy], parts[fmt.mm] - 1, parts[fmt.dd]);
                        valueAsDate.setHours(0, 0, 0, 0);

                        // return null in case of invalid date
                        if (isNaN(valueAsDate)) {
                            return null;
                        }

                        return valueAsDate;
                    },
                    set: (val) => {
                        inputObject.value = DateFormat(val, dateFormat);
                        // trigger change event to execute event listeners on the date element
                        let event;
                        // IE event support check
                        if (typeof (Event) === 'function') {
                            event = new Event('change', { bubbles: true });
                        } else {
                            event = document.createEvent('Event');
                            event.initEvent('change', true, true);
                        }

                        inputObject.dispatchEvent(event);
                    },
                },
                valueAsNumber: {
                    get: () => {
                        if (!inputObject.value) {
                            return NaN;
                        }

                        return inputObject.valueAsDate.valueOf();
                    },
                    set: (val) => {
                        inputObject.valueAsDate = new Date(val);
                    },
                },
            },
        );

        // watch for element attribute changes
        if ("MutationObserver" in window) {
            const mutationObserver = new MutationObserver((mutations) => {
                mutations.forEach((mutation) => {
                    if (mutation.attributeName.indexOf('min') !== -1 || mutation.attributeName.indexOf('max') !== -1) {
                        minAttribute = inputObject.getAttribute('min')
                            || inputObject.getAttribute('data-min');
                        maxAttribute = inputObject.getAttribute('max')
                            || inputObject.getAttribute('data-max');
                        inputObject.dateRange = this.getDateRange(minAttribute, maxAttribute);
                    } else if (mutation.attributeName === 'lang') {
                        locale = inputObject.getAttribute(mutation.attributeName);
                        inputObject.localeLabels = this.getLocaleLabels(locale);
                    } else if (mutation.attributeName === 'data-first-day') {
                        inputObject.firstDayOfWeek = inputObject
                                .getAttribute(mutation.attributeName);
                    } else if (mutation.attributeName === 'data-date-format' || mutation.attributeName === 'date-format') {
                        const date = inputObject.valueAsDate;
                        dateFormat = inputObject.getAttribute(mutation.attributeName);
                        if (date) {
                            inputObject.valueAsDate = date; // reset date to update the format
                        }
                    }
                });
            });

            mutationObserver.observe(inputObject, {
                attributes: true,
            });
        }

        // Open the picker when the input get focus,
        // also on various click events to capture it in all corner cases.
        // use const didAttach = Picker.attachTo(elm); to double check
        const showPicker = () => {
            Picker.attachTo(inputObject);
        };

        inputObject.addEventListener('focus', showPicker);
        inputObject.addEventListener('mouseup', showPicker);

        const minDate = inputObject.dateRange[0];
        const maxDate = inputObject.dateRange[1];

        // Update the picker if the date changed manually in the input.
        inputObject.addEventListener('keydown', (e) => {
            let date = new Date();

            switch (e.keyCode) {
                case 9: // hide on tab
                case 27:
                    // hide on esc
                    Picker.hide();
                    break;
                case 37:
                    // arrow left
                    if (inputObject.valueAsDate) {
                        date = Picker.returnCurrentDate();
                        date.setDate(date.getDate() - 1);

                        if (date >= minDate) {
                            inputObject.valueAsDate = date;
                        }
                    }
                    break;
                case 38:
                    // arrow up
                    if (inputObject.valueAsDate) {
                        date = Picker.returnCurrentDate();
                        date.setDate(date.getDate() - 7);

                        if (date >= minDate) {
                            inputObject.valueAsDate = date;
                        }
                    }
                    break;
                case 39:
                    // arrow right
                    if (inputObject.valueAsDate) {
                        date = Picker.returnCurrentDate();
                        date.setDate(date.getDate() + 1);

                        if (date <= maxDate) {
                            inputObject.valueAsDate = date;
                        }
                    }
                    break;
                case 40:
                    // arrow down
                    if (inputObject.valueAsDate) {
                        date = Picker.returnCurrentDate();
                        date.setDate(date.getDate() + 7);

                        if (date <= maxDate) {
                            inputObject.valueAsDate = date;
                        }
                    }
                    break;
                default:
                    break;
            }

            // remove to improve performance
            Picker.syncPickerWithInput();
        });

        inputObject.addEventListener('keyup', () => {
            Picker.syncPickerWithInput();
        });
    }

    // Will add the Picker to all inputs in the page.
    addPickerToDateInputs() {
        // Get and loop all input[type="date"]s that do not have '[data-has-picker]' yet.
        const dateInputs = document.querySelectorAll('input[type="date"]:not([data-has-picker])');
        const { length } = dateInputs;

        if (!length) {
            return false;
        }

        for (let i = 0; i < length; i += 1) {
            this.prepareInputForPicker(dateInputs[i]);
        }

        return true;
    }

    addPickerToOtherInputs() {
        // Get and loop all the input[type="text"]s with
        // class date-polyfill that do not have '[data-has-picker]' yet.
        const dateInputs = document.querySelectorAll('input[type="text"].date-polyfill:not([data-has-picker])');
        const { length } = dateInputs;

        if (!length) {
            return false;
        }

        for (let i = 0; i < length; i += 1) {
            this.prepareInputForPicker(dateInputs[i]);
        }

        return true;
    }
}

export default new Input();
