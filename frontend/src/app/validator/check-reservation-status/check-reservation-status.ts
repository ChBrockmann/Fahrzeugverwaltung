import {ReservationService} from "../../api";
import {AbstractControl, AsyncValidatorFn, ValidationErrors} from "@angular/forms";
import {firstValueFrom, map, Observable, of} from "rxjs";

// export class CheckReservationStatus {
//   static createValidator(reservationService: ReservationService) : AsyncValidatorFn {
//     return (control: AbstractControl<Partial<{ startDate: moment. Moment | null; endDate: moment. Moment | null; requestingUser: string | null; requestedVehicleId: string | null; }>>) => {
//       console.log("TEST");
//       console.log(control);
//       console.log(typeof(control));
//
//       return new Promise((resolve) => resolve(null));
//     }
//   }
// }


export class CheckReservationStatus {
  static checkVehicleAvailability(reservationService: ReservationService): (control: AbstractControl) => Observable<ValidationErrors | null> {
    return (formGroup: AbstractControl): Observable<ValidationErrors | null> => {
      const startDate = formGroup.get('startDate')?.value;
      const endDate = formGroup.get('endDate')?.value;
      const requestedVehicleId = formGroup.get('requestedVehicleId')?.value;

      if(startDate == null || endDate == null || requestedVehicleId == null || requestedVehicleId == "") {
        return of(null);
      }

      return reservationService.checkAvailabilityForVehicleAndTimespanEndpoint(requestedVehicleId, startDate.format("YYYY-MM-DD"), endDate.format("YYYY-MM-DD")).pipe(
        map(value => {
          if (value.availability == "Available") {
            return null;
          }
          else {
            return {notAvailable: true};
          }
        })
      );
    };
  }
}
