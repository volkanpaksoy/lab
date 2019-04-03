import { TestBed } from '@angular/core/testing';
import { TldService } from './tld.service';
import { FetchHtmlService } from './fetch-html.service';

describe('TldService', () => {
  beforeEach(() => TestBed.configureTestingModule({

  }));

  it('should be created', () => {
    const service: TldService = TestBed.get(TldService);
    expect(service).toBeTruthy();
  });

  it('should parse html and extract TLD list', () => {
    const fetchHtmlService: FetchHtmlService = new FetchHtmlService();
    spyOn(fetchHtmlService, 'getRawHtml').and.returnValue('<a href="registrar-tld-list.html#academy">.academy</a>');
    const service: TldService = new TldService(fetchHtmlService);
    expect(service.getAllSupportedTlds().length).toBe(1);
  });
});



