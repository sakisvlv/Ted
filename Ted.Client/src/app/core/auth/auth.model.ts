export class LoginData {
    Email: string;
    Password: string;
    RememberMe: boolean;
    constructor(){
        this.Email = "";
        this.Password = "";
        this.RememberMe = true;
    }
}

export class RegisterData {
    Email: string;
    Password: string;
    FirstName: string;
    LastName: string;
    PhoneNumber: string;
}