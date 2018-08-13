import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {

  public loading = false;

  constructor() { }

  public show(delay: number = 0): void {
    setTimeout(() => {
      this.loading = true;
    }, delay)
  }

  public hide(delay: number = 0): void {
    setTimeout(() => {
      this.loading = false;
    }, delay)
  }
  
}
