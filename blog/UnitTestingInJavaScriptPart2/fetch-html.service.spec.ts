import { TestBed } from '@angular/core/testing';

import { FetchHtmlService } from './fetch-html.service';

describe('FetchHtmlService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FetchHtmlService = TestBed.get(FetchHtmlService);
    expect(service).toBeTruthy();
  });
});
