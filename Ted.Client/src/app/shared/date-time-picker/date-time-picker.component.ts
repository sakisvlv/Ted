import { Component, OnInit, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { DatePipe } from '@angular/common'

import { DatepickerOptions } from 'ng2-datepicker';
import * as en from 'date-fns/locale/en';

import { AmazingTimePickerService, ɵa } from 'amazing-time-picker';

@Component({
  selector: 'date-time-picker',
  templateUrl: './date-time-picker.component.html',
  styleUrls: ['./date-time-picker.component.scss']
})
export class DateTimePickerComponent implements OnInit, DoCheck {

  isDatePickerOpened: boolean = false;

  options: DatepickerOptions = {
    minYear: 1970,
    maxYear: 2030,
    displayFormat: 'DD/MM/YYYY',
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
    private atp: AmazingTimePickerService,
    private datepipe: DatePipe
  ) { }

  ngOnInit() {
  }

  openTimePicker() {
    const amazingTimePicker = this.atp.open({
      time: this.datepipe.transform(this.date, 'HH:mm')
    });
    amazingTimePicker.afterClose().subscribe(time => {
      this._date.setHours(+time.substring(0, 2));
      this._date.setMinutes(+time.substring(3, 5));
    });
  }

  ngDoCheck() {
    this.showDate = new Date(this.date);
  }

}
