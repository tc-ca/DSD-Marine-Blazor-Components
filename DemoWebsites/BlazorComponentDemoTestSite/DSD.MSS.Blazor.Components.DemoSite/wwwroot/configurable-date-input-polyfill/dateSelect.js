export class DateSelect {
    constructor() {
        this.dateSelectWrapper = document.createElement('div');
        this.dateSelectWrapper.className = 'select-wrapper';

        this.toggleUp = document.createElement('button');
        this.toggleUp.className = 'control up';

        this.toggleDown = document.createElement('button');
        this.toggleDown.className = 'control down';

        this.optionWrapper = document.createElement('div');
        this.optionWrapper.className = 'option-wrapper';

        this.dateSelectWrapper.appendChild(this.toggleUp);
        this.dateSelectWrapper.appendChild(this.optionWrapper);
        this.dateSelectWrapper.appendChild(this.toggleDown);

        this.date = new Date();
    }

    calculateDateOffset(dateArray, targetDate) {
        const dateArrayLength = dateArray.length;
        let calculatedDateOffset = 0;

        switch (true) {
            case (targetDate < dateArray[2]):
                calculatedDateOffset = (dateArrayLength - dateArray[2]) + targetDate;
                break;
            case (targetDate === dateArray[2]):
                // do nothing because default value fits
                break;
            case (targetDate > dateArray[2]):
                calculatedDateOffset = targetDate - dateArray[2];
                break;
            default:
                // console.log('Error in MonthSelect calculateDateOffset');
                break;
        }

        return calculatedDateOffset;
    }

    rotate(array, times) {
        let timesToRotate = times;
        while (timesToRotate > 0) {
            const temp = array.shift();
            array.push(temp);
            timesToRotate -= 1;
        }

        return array;
    }

    returnDateSelectWrapper() {
        return this.dateSelectWrapper;
    }
}

export class YearSelect extends DateSelect {
    constructor() {
        super();
        this.dateSelectWrapper.className = 'select-wrapper year-select';
        this.yearArray = [];

        /* start Function */
        for (let i = 0; i < 5; i += 1) {
            const option = document.createElement('div');
            option.className = `option option-${i}`;

            this.optionWrapper.appendChild(option);
        }

        /* downClick Function */
        this.toggleDown.addEventListener('click', (buttonObject) => {
            const activeButton = buttonObject;
            // update array order
            this.yearArray.shift();
            this.yearArray.push(this.yearArray[this.yearArray.length - 1] + 1);

            for (let i = 0; i < 5; i += 1) {
              activeButton.target.previousElementSibling.getElementsByClassName('option')[i].innerHTML = this.yearArray[i];
            }
        });

        /* upClick Function */
        this.toggleUp.addEventListener('click', (buttonObject) => {
            const activeButton = buttonObject;
            if (this.yearArray[2] === 1) {
                return;
            }
            // update array order
            this.yearArray.pop();
            this.yearArray.unshift(this.yearArray[0] - 1);
            if (this.yearArray[0] < 1) {
                this.yearArray[0] = '';
            }

            for (let i = 0; i < 5; i += 1) {
                activeButton.target.nextElementSibling.getElementsByClassName('option')[i].innerHTML = this.yearArray[i];
            }
        });
    }

    toggleByInput(value) {
        let targetYear = value;
        // clear current values
        this.yearArray.length = 0;
        // create siblings
        targetYear -= 2;
        for (let i = 0; i < 5; i += 1) {
            if (targetYear < 1) {
                this.yearArray.push('');
            } else {
                this.yearArray.push(targetYear);
            }
            targetYear += 1;
        }

        for (let i = 0; i < 5; i += 1) {
            this.optionWrapper.getElementsByClassName('option')[i].innerHTML = this.yearArray[i];
        }
    }

    returnSelectedYear() {
        return this.yearArray[2];
    }
}

export class MonthSelect extends DateSelect {
    constructor(targetLocaleArray) {
        super();
        this.dateSelectWrapper.className = 'select-wrapper month-select';
        this.currentMonth = this.date.getMonth();

        this.monthArray = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

        this.selectedLocaleArray = targetLocaleArray;

        this.monthStringArray = this.returnMonthStringArray(this.monthArray);

        /* start Function */
        for (let i = 0; i < 5; i += 1) {
            const option = document.createElement('div');
            option.className = `option option-${i}`;
            option.innerHTML = this.monthStringArray[i];

            this.optionWrapper.appendChild(option);
        }

        /* downClick Function */
        this.toggleDown.addEventListener('click', (buttonObject) => {
            const activeButton = buttonObject;
            // update array order
            this.monthArray.push(this.monthArray.shift());

            const monthStringArray = this.returnMonthStringArray(this.monthArray);

            for (let i = 0; i < 5; i += 1) {
                activeButton.target.previousElementSibling.getElementsByClassName('option')[i].innerHTML = monthStringArray[i];
            }
        });

        /* upClick Function */
        this.toggleUp.addEventListener('click', (buttonObject) => {
            const activeButton = buttonObject;
            // update array order
            this.monthArray.unshift(this.monthArray.pop());

            const monthStringArray = this.returnMonthStringArray(this.monthArray);

            for (let i = 0; i < 5; i += 1) {
                activeButton.target.nextElementSibling.getElementsByClassName('option')[i].innerHTML = monthStringArray[i];
            }
        });
    }

    returnMonthStringArray(monthArray) {
        const monthStringArray = [];
        const localeArray = this.selectedLocaleArray;

        if (!Array.isArray(monthArray)) {
            return this.selectedLocaleArray[monthArray];
        }

        monthArray.forEach((index) => {
            monthStringArray.push(localeArray[index].substring(0, 3));
        });

        return monthStringArray;
    }

    toggleByInput(value) {
        if (value !== this.monthArray[2]) {
            this.monthArray = this.rotate(this.monthArray,
                this.calculateDateOffset(this.monthArray, value));
            this.monthStringArray = this.returnMonthStringArray(this.monthArray);

            for (let i = 0; i < 5; i += 1) {
                this.optionWrapper.getElementsByClassName('option')[i].innerHTML = this.monthStringArray[i];
            }
        }
    }

    toggleByMatrix(mode) {
        switch (mode) {
            case 'next':
                this.monthArray = this.rotate(this.monthArray, 1);
                break;
            case 'prev':
                this.monthArray = this.rotate(this.monthArray, 11);
                break;
            default:
            // console.log('mode is not defined in toggleMonthByMatrix');
        }

        this.monthStringArray = this.returnMonthStringArray(this.monthArray);

        for (let i = 0; i < 5; i += 1) {
            this.optionWrapper.getElementsByClassName('option')[i].innerHTML = this.monthStringArray[i];
        }
    }

    returnSelectedMonthAsLabel() {
        return this.returnMonthStringArray(this.monthArray[2]);
    }

    returnSelectedMonth() {
        return this.monthArray[2];
    }
}
