export class User {
    tokenExpirationDate: Date;
    constructor(private _accessToken: string, private _expiresIn: number, public firstName: string, public lastName: string){
        this.tokenExpirationDate = new Date();
        this.tokenExpirationDate.setSeconds(this.tokenExpirationDate.getSeconds() + this._expiresIn);
    }

    get accessToken(): string|null {
        if(!this.tokenExpirationDate || this.tokenExpirationDate < new Date())
            return null;

        return this._accessToken;
    }
}
