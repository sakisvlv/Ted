export class UserSmall {
    Id: string;
    FirstName: string;
    LastName: string;
    CurrentState: string;
    IsFriend: boolean;
}


export class Notification {
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