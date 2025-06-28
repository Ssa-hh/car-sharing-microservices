import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { isValidDate } from "rxjs/internal/util/isDate";

export class CustomValidators {
    static minDate(minDate: Date): ValidatorFn {
  
        return (control: AbstractControl): ValidationErrors | null => {
            const value = control.value;
            if (!value)
                return null;

        let valueAsDate = new Date(value.year, value.month -1, value.day);
        let isDateValid = valueAsDate.getTime() >= minDate.getTime();
        
        return !isDateValid ? {lessThanMinDate: true}: null;
    }
  };

  static minTime(minTime: {hour: number, minute:number}): ValidatorFn {
  
        return (control: AbstractControl): ValidationErrors | null => {
            const value = <{hour: number, minute:number}>control.value;
            let isTimeValid:boolean;

            if (!value)
                return null;

            if(value.hour > minTime.hour)
                isTimeValid = true;
            else if(value.hour < minTime.hour)
                isTimeValid = false;
            else
                isTimeValid = value.minute >= minTime.minute

            return !isTimeValid ? {lessThanMinTime: true}: null;
    }
  };

}