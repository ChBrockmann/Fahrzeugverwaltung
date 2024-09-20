import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateManualLogbookEntryComponent } from './create-manual-logbook-entry.component';

describe('CreateManualLogbookEntryComponent', () => {
  let component: CreateManualLogbookEntryComponent;
  let fixture: ComponentFixture<CreateManualLogbookEntryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreateManualLogbookEntryComponent]
    });
    fixture = TestBed.createComponent(CreateManualLogbookEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
