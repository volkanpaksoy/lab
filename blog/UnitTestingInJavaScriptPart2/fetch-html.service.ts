import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FetchHtmlService {

  constructor() { }

  getRawHtml(): string {
    return '';
  }
}
