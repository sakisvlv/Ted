import { Component, OnInit, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { DatePipe } from '@angular/common'

import { DatepickerOptions } from 'ng2-datepicker';
import * as en from 'date-fns/locale/en';

@Component({
  selector: 'date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss']
})
export class DatePickerComponent implements OnInit, DoCheck {

  isDatePickerOpened: boolean = false;

  options: DatepickerOptions = {
    minYear: 1970,
    maxYear: 2030,
    displayFormat: 'MMMM d, y',
    barTitleFormat: 'MMMM YYYY',
    dayNamesFormat: 'dd',
    firstCalendarDay: 1, // 0 - Sunday, 1 - Monday
    locale: en,
    //minDate: new Date(Date.now()), // Minimal selectable date
    maxDate: new Date(Date.now()),  // Maximal selectable date
    barTitleIfEmpty: 'Click to select a date',
    placeholder: 'Click to select a date', // HTML input placeholder attribute (default: '')
    addClass: '', // Optional, value to pass on to [ngClass] on the input field
    addStyle: {}, // Optional, value to pass to [ngStyle] on the input field
    fieldId: 'my-date-picker', // ID to assign to the input field. Defaults to datepicker-<counter>
    useEmptyBarTitle: false // Defaults to true. If set to false then barTitleIfEmpty will be disregarded and a date will always be shown
  };
  showDate;

  private _date: Date;
  @Input()
  set date(value: Date) {
    let oldValue = this._date;
    this._date = value;
    if (value != undefined && oldValue != undefined) {
      this._date = oldValue;
      this._date.setDate(value.getDate());
      this._date.setMonth(value.getMonth());
      this._date.setFullYear(value.getFullYear());
      this.isDatePickerOpened = false;
    }
    this.dateChange.emit(this._date);
  }
  get date(): Date {
    return this._date;
  }

  @Output() dateChange = new EventEmitter<any>();

  constructor(
    private datepipe: DatePipe
  ) { }

  ngOnInit() {
  }

  ngDoCheck() {
    this.showDate = new Date(this.date);
  }

}
