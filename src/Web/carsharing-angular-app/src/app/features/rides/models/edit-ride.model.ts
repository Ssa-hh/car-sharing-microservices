import { Car } from "../../../shared/models/car.model";

export interface EditRide {
    id: string;
    startsAtUtc: Date; 
    endsAtUtc: Date; 
    pickupCity: string; 
    dropOffCity: string;
    numberOfProposedSeats: number;
    car: Car;
}