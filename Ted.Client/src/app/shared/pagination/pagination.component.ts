import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit {

  @Output() onPageChange = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
  }

  pageChange(page) {
    this.onPageChange.emit(page);
  }

}
