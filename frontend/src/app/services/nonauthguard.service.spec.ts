import { TestBed } from '@angular/core/testing';

import { NonauthguardService } from './nonauthguard.service';

describe('NonauthguardService', () => {
  let service: NonauthguardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NonauthguardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
