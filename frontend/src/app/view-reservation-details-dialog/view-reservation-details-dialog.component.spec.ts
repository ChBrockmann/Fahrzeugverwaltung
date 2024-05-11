import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewReservationDetailsDialogComponent } from './view-reservation-details-dialog.component';

describe('ViewReservationDetailsDialogComponent', () => {
  let component: ViewReservationDetailsDialogComponent;
  let fixture: ComponentFixture<ViewReservationDetailsDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewReservationDetailsDialogComponent]
    });
    fixture = TestBed.createComponent(ViewReservationDetailsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
