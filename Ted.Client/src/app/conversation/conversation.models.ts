export class Conversation {
    ToUser: UserSmall;
    Id: string;
}

export class UserSmall {
    Id: string;
    FirstName: string;
    LastName: string;
    CurrentState: string;
    IsFriend: boolean;
}