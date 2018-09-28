export class UserSmall {
    Id: string;
    FirstName: string;
    LastName: string;
    CurrentState: string;
    IsFriend: boolean;
}


export class Notification {
    Id: string;
    Sender: string;
    SenderId: string;
    Type: NotificationType;
    PostId: string;
    IsAcknowledged: boolean;
    DateAdded: Date;
}


export enum NotificationType {
    FriendRequest,
    Subscribe,
    Comment
}