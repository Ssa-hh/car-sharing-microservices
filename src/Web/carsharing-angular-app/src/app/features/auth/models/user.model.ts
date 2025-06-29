import { Car } from "../../../shared/models/car.model";

export class User {
    constructor(private _accessToken: string, private _tokenExpirationDate: Date, public firstName: string
        , public lastName: string, public cars: Car[]){
    }

    get accessToken(): string|null {
        if(!this._tokenExpirationDate || this._tokenExpirationDate < new Date())
            return null;

        return this._accessToken;
    }
}
