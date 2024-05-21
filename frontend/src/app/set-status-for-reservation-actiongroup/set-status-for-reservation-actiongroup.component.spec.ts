import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetStatusForReservationActiongroupComponent } from './set-status-for-reservation-actiongroup.component';

describe('SetStatusForReservationActiongroupComponent', () => {
  let component: SetStatusForReservationActiongroupComponent;
  let fixture: ComponentFixture<SetStatusForReservationActiongroupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetStatusForReservationActiongroupComponent]
    });
    fixture = TestBed.createComponent(SetStatusForReservationActiongroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
