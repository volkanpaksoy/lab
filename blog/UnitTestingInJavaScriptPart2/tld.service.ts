import { Injectable } from '@angular/core';
import { FetchHtmlService } from './fetch-html.service';

@Injectable({
  providedIn: 'root'
})
export class TldService {

  constructor(private fetchHtmlService: FetchHtmlService) { }

  getAllSupportedTlds(): string[] {
    const rawHtml = this.fetchHtmlService.getRawHtml();
    const supportedTlds = this.parseHtml(rawHtml);
    return supportedTlds;
  }

  private parseHtml(html: string): string[] {
    console.log(html);

    const pattern = /<a href="registrar-tld-list.html#.+?">[.](.+?)<\/a>/g;
    const regEx = new RegExp(pattern);
    let tldList = [];

    let match = regEx.exec(html);
    console.log(match);

    while ( match !== null) {
      tldList.push(match[1]);
      match = regEx.exec(html);
    }

    return tldList;
  }
}
