export class Ride {
    constructor(id: string, startsAtUtc: string, endsAtUtc: string, pickupCity: string, dropOffCity: string,
        numberOfProposedSeats: number, numberOfAvailableSeats: number, driverFirstName: string, driverLastName: string){
        this.id = id;
        this.startsAtUtc = startsAtUtc; 
        this.endsAtUtc = endsAtUtc; 
        this.pickupCity = pickupCity; 
        this.dropOffCity = dropOffCity;
        this.numberOfProposedSeats = numberOfProposedSeats;
        this.numberOfAvailableSeats = numberOfAvailableSeats;
        this.driverFirstName = driverFirstName;
        this.driverLastName = driverLastName;
    }

    id: string;
    startsAtUtc: string; 
    endsAtUtc: string; 
    pickupCity: string; 
    dropOffCity: string;
    numberOfProposedSeats: number;
    numberOfAvailableSeats: number;
    driverFirstName: string;
    driverLastName: string;
}