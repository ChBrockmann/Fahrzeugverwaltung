import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewLogbookComponent } from './view-logbook.component';

describe('ViewLogbookComponent', () => {
  let component: ViewLogbookComponent;
  let fixture: ComponentFixture<ViewLogbookComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewLogbookComponent]
    });
    fixture = TestBed.createComponent(ViewLogbookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
