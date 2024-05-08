export * from './default.service';
import { DefaultService } from './default.service';
export * from './reservation.service';
import { ReservationService } from './reservation.service';
export * from './vehicle.service';
import { VehicleService } from './vehicle.service';
export const APIS = [DefaultService, ReservationService, VehicleService];
